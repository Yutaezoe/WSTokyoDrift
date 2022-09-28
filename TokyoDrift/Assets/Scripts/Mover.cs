using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Linq;

public class Mover : MonoBehaviour
{

    #region Mover's Field
    //Managerクラスの呼び出し用
    [SerializeField]
    GameObject manager;

    [SerializeField]
    private int _MoveID;
    private int[] _TargetID;

    // Initial Position
    private int startUID;

    // Goal Position
    private int _nextUID;
    private int goalUID;

    // Target Lists
    [SerializeField]
    private Transform targetMaster;

    //Move
    private Vector3[] nodePoints;
    [SerializeField]
    private float baseSpeed = 2;
    private int modifiedVelocity;

    // Ezoe edit
    [SerializeField]
    private GameObject lineMaster;

    // SakoSako edit
    [SerializeField]
    private GameObject nodeMaster;

    private Transform[] lineChildren;
    private Transform[] nodeChildren;
    private Transform[] targetChildren;

    // Target
    private Target targetComponent;
    private Vector3[] targetVector3;
    private bool IsAsignWait;
    private Target.TargetStatus[] pickStatus; 

    private GameObject moverNodeGO;
    private Node moverNodeArray;
    private int point;
    private int[] routeReturn;
    private long[] costReturn;
    private int[] eachTargetDistance;
    //

    private Vector3[] nodeVector;
    private Vector3 startNodeVector;
    private object nodeVetor;
    private int nodeCounter = 0 ;

    private int[] lineFromNodeUID;
    private int[] lineToNodeUID;
    private int[] lineComponetWeight;
    private Vector3[] lineFromNodeVector;
    private Vector3[] lineToNodeVector;
    private int[] targetNearNodeId;
    private bool IsCalledDistance = false;
    private Vector3 goalPosition;
    #endregion

    #region Setting Property
    public int PropertyMoveID { get { return _MoveID; } }
    public int PropertyTargettingPoint { get { return _nextUID; }}
    public int[] PropertyTargetID { get { return _TargetID; } set { this._TargetID = value; } }
    #endregion

    private void Awake()
    { 
        // Ezoe Edit
        lineChildren = ComFunctions.GetChildren(lineMaster.transform);
        nodeChildren = ComFunctions.GetChildren(nodeMaster.transform);
        //
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ezoe
        //Fieldの状態を把握
        SettingComponent();

        //Sako
        //スタート地点とゴール地点を定義
        SettingStartAndGoal();

        //Ezoe
        //マネージャーへ送る用の情報をダイクストラで計算
        CalcDikstra(startUID, _nextUID);

        List<Target.TargetStatus> targetStatus = new();

        foreach (Transform setTarget in targetChildren)
        {
            targetComponent = ComFunctions.GetChildrenComponent<Target>(setTarget);
            targetStatus.Add(targetComponent.StatusOfPikking);
        }
        pickStatus = targetStatus.ToArray();

        for (int i = 0; i < pickStatus.Length; i++)
        {
            Debug.Log(pickStatus[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Goalに到達した場合はreturnを返す
        float distanceFromGoal = Vector3.Distance(transform.position, goalPosition);
        if (distanceFromGoal < 0.2f　&& targetChildren.Length == 1)
        {
            return;
        }
        else
        {
            if (!IsCalledDistance)
            {
                //Sako
                //マネージャーに各ターゲットへのデータ渡し
                Manager managercompo = manager.GetComponent<Manager>();
                SettingEachTargetDistance();
                bool complete = managercompo.distancePassive(_MoveID, _TargetID, eachTargetDistance);
                if (complete == true)
                {
                    IsCalledDistance = true;
                }
                return;
            }
            else
            {
                //MoverがManagerに一度もアサインされていなければ入る
                if (!IsAsignWait)
                {
                    //Manager側で自身がアサインされたかチェック
                    AssignWait();
                }

                //常時、残ターゲットを確認
                SettingComponent();

                //目的地(_nextUID)にたどり着くまでMoveMobilityを繰り返す
                //到達後、DecesionTargetにて次の目的地を設定し、上記を実施
                if (nodeCounter != nodePoints.Length)
                {
                    ModifyVelocity();
                    MoveMobility();
                }
                else
                {
                    nodeCounter = 0;
                    startUID = _nextUID;
                    DecesionTarget();
                    RouteSetting();
                }
            }
        }
    }

    #region Mover's Methods
    //Ezoe
    private void SettingComponent()
    {
        targetChildren = ComFunctions.GetChildren(targetMaster.transform);

        Line lineComponent;
        Node nodeComponent;

        List<int> lineFromNodeUIDList = new();
        List<int> lineToNodeUIDList = new();
        List<int> weightList = new();
        

        List<Vector3> lineFromNodeVectorList = new();
        List<Vector3> lineToNodeVectorList = new();
        List<Vector3> TargetVector3 = new List<Vector3>();

        // LineよりUID 重さ 接続位置の取得を行う
        foreach (Transform setLineChild in lineChildren)
        {
            lineComponent = ComFunctions.GetChildrenComponent<Line>(setLineChild);

            nodeComponent = ComFunctions.GetChildrenComponent<Node>(lineComponent.getAPosition);
            lineFromNodeUIDList.Add(nodeComponent.getNodeUID);
            lineFromNodeVectorList.Add(lineComponent.getAPosition.position);
            nodeComponent = ComFunctions.GetChildrenComponent<Node>(lineComponent.getBPosition);
            lineToNodeUIDList.Add(nodeComponent.getNodeUID);
            lineToNodeVectorList.Add(lineComponent.getBPosition.position);

            weightList.Add(lineComponent.getLineWeight);
        }
        lineFromNodeUID = lineFromNodeUIDList.ToArray();
        lineToNodeUID = lineToNodeUIDList.ToArray();
        lineComponetWeight = weightList.ToArray();
        lineFromNodeVector = lineFromNodeVectorList.ToArray();
        lineToNodeVector = lineToNodeVectorList.ToArray();

        // Targetの位置の取得を行う
        foreach (Transform setTarget in targetChildren)
        {
            targetComponent = ComFunctions.GetChildrenComponent<Target>(setTarget);
            TargetVector3.Add(targetComponent.NearNodeVector3);
        }
        targetVector3 = TargetVector3.ToArray();

        // Get Target near node
        List<int> targetNearNodeList = new();
        int tempNearNode;
        int cnt;
        float minDistanceVector;
        float dist;



        foreach (Vector3 setTargetVector3 in targetVector3)
        {
            tempNearNode = 0;
            cnt = 0;
            minDistanceVector = float.MaxValue;
            foreach (Vector3 setFromNodeVector in lineFromNodeVector)
            {
                dist = Vector3.Distance(setFromNodeVector, setTargetVector3);
                
                if (minDistanceVector > dist)
                {
                    minDistanceVector = dist;
                    tempNearNode = lineFromNodeUID[cnt];
                }
                cnt++;
            }
            cnt = 0;

            foreach (Vector3 setToNodeVector in lineToNodeVector)
            {
                dist = Vector3.Distance(setToNodeVector, setTargetVector3);
                
                if (minDistanceVector > dist)
                {
                    minDistanceVector = dist;
                    tempNearNode = lineToNodeUID[cnt];
                }
                cnt++;
            }

            targetNearNodeList.Add(tempNearNode);
            //Debug.Log(tempNearNode);
        }

        targetNearNodeId = targetNearNodeList.ToArray();
    }

    //Sako
    private void SettingStartAndGoal()
    {

        //StartSet
        int sta = 0;
        float minDistanceFromStart = float.MaxValue;
        float distance;


        foreach (Vector3 setFromNodeVector in lineFromNodeVector)
        {
            distance = Vector3.Distance(setFromNodeVector, transform.position);

            if (minDistanceFromStart > distance)
            {
                minDistanceFromStart = distance;
                startUID = lineFromNodeUID[sta];
            }
            sta++;
        }

        sta = 0;

        foreach (Vector3 setToNodeVector in lineFromNodeVector)
        {
            distance = Vector3.Distance(setToNodeVector, transform.position);

            if (minDistanceFromStart > distance)
            {
                minDistanceFromStart = distance;
                startUID = lineFromNodeUID[sta];
            }
            sta++;
        }

       

        //startUID = startNodePoint.GetComponent<Node>().getNodeUID;

        foreach (Transform goalTarget in targetChildren)
        {
            Target goalCheck;
            goalCheck = ComFunctions.GetChildrenComponent<Target>(goalTarget);

            if (goalCheck.GoalPoint)
            {
                goalUID = targetNearNodeId[goalCheck.TargetUid];
                _nextUID = goalUID;
                goalPosition = goalCheck.NearNodeVector3;
            }
        }

        Debug.Log(startUID + "" + goalUID+$"({goalPosition})");
    }

    //Sako
    private void MoveMobility()
    {
            transform.position = Vector3.MoveTowards(transform.position
                , nodeVector[nodeCounter]
                , (baseSpeed / (float)modifiedVelocity) * Time.deltaTime);

            if (transform.position.x == nodeVector[nodeCounter].x && transform.position.z == nodeVector[nodeCounter].z)
            {
                nodeCounter++;
            }
    }

    //Ezoe
    private void CalcDikstra(int start,int goal)
    {

        GameObject lineGO;
        GameObject nodeGO;
        Line lineArray;
        Node nodeArray;
        int lineWeight;
        int pointA;
        int pointB;


        List<string> Nodename = new List<string>();

        Dikstra graph = new Dikstra(lineChildren.Length + 1);

        foreach (Transform setTrans in lineChildren)
        {
            //子ラインから、ライン重み、poinA,Bを配列に格納
            
            lineGO = setTrans.gameObject;
            lineArray = lineGO.GetComponent<Line>();
            lineWeight = lineArray.getLineWeight;

            nodeGO = lineArray.getAPosition.gameObject;
            nodeArray = nodeGO.GetComponent<Node>();
            pointA = nodeArray.getNodeUID;
            Nodename.Add(nodeGO.name);

            nodeGO = lineArray.getBPosition.gameObject;
            nodeArray = nodeGO.GetComponent<Node>();
            pointB = nodeArray.getNodeUID;
            Nodename.Add(nodeGO.name);

            graph.Add(pointA, pointB, lineWeight);
            graph.Add(pointB, pointA, lineWeight);
        };
        string[] arrayStrings = Nodename.ToArray();

        IEnumerable<string> enumArray = arrayStrings.Distinct();

        
        long[] minDistaces = graph.GetMinCost(start, goal, enumArray.Count());

        foreach(long c in graph.Cost)
        {
            //Debug.Log(c);
        }
        foreach (int r in graph.RouteReturn)
        {
            //Debug.Log(r);
        }

        costReturn = graph.Cost;
        routeReturn = graph.RouteReturn;

    }

    //Sako
    private void RouteSetting()
    {
        List<Vector3> nodeTrans = new ();

        for (int t = 0; t < routeReturn.Length; t++)
        {
            foreach (int setNode in lineToNodeUID)
            {
                if (lineFromNodeUID[setNode] == routeReturn[t])
                {
                    nodeTrans.Add(lineFromNodeVector[setNode]);
                } 
                else if (lineToNodeUID[setNode] == routeReturn[t])
                {
                    nodeTrans.Add(lineToNodeVector[setNode]);
                }
            }
            nodePoints = nodeTrans.ToArray();
        }

        startNodeVector = nodePoints[0];
        startNodeVector.y = 0.25f;
        transform.position = startNodeVector;

        nodeVector = new Vector3[nodePoints.Length];

        //不要では？
        for (int i = 0; i < nodeVector.Length; i++)
        {
            nodeVector[i] = nodePoints[i];
            nodeVector[i].y = 0.25f;
        }
    }

    //Sako
    private void SettingEachTargetDistance()
    {
        /////マネージャーへの受け渡し用配列定義
        List<int> targetIDList = new List<int>();
        List<int> minDistance = new List<int>();
        /////
        /////////マネージャーへの受け渡し用配列作成
        for (int setTarget = 0; setTarget < targetNearNodeId.Length; setTarget++)
        {
            targetIDList.Add(targetNearNodeId[setTarget]);
            minDistance.Add((int)costReturn[targetNearNodeId[setTarget]]);
        }

        _TargetID = targetIDList.ToArray();
        eachTargetDistance = minDistance.ToArray();

        //for (int t = 0; t < targetNearNodeId.Length; t++)
        //{
        //    Debug.Log($"TargetID:{TargetID[t]} ,Cost:{eachTargetDistance[t]} ");
        //}
        /////////
    }

    //Sako
    private void DecesionTarget()
    {

        //ダイクストラのプリ計算
        CalcDikstra(startUID, _nextUID);

        //ターゲットの中から最短経路のものを抽出
        int stanum = 0;
        long costRetrunTemp = long.MaxValue;
        Target.TargetStatus statusTemp;
        foreach (Transform setTarget in targetChildren)
        {
            statusTemp = ComFunctions.GetChildrenComponent<Target>(setTarget).StatusOfPikking;

            //残ターゲットが2個以上でgoalUIDは候補から除外
            if (targetNearNodeId[stanum] == goalUID && targetChildren.Length != 1)
            {
                stanum++;
                continue;
            }
            else if (statusTemp == Target.TargetStatus.COMPLETED)
            {
                stanum++;
                continue;
            }

            //最もコストの低いTargetを抽出
            if (costReturn[stanum] < costRetrunTemp)
            {
                costRetrunTemp = costReturn[stanum];
                _nextUID = targetNearNodeId[stanum];
            }
            stanum++;
        }
        //if (targetNearNodeId[stanum] != goalUID)
        //{
        //    Debug.Log(" TargetNode " + targetNearNodeId[stanum]);
        //    targetComponent = ComFunctions.GetChildrenComponent<Target>(targetChildren[stanum]);
        //    targetComponent.SetStatusOfPikkingCOMPLETED();
        //}
       

        List<Target.TargetStatus> targetStatus = new();

        foreach (Transform setTargetDebug in targetChildren)
        {
            targetComponent = ComFunctions.GetChildrenComponent<Target>(setTargetDebug);
            targetStatus.Add(targetComponent.StatusOfPikking);
        }
        pickStatus = targetStatus.ToArray();

        //for (int i = 0; i < pickStatus.Length; i++)
        //{
        //    Debug.Log(pickStatus[i]);
        //}

        Debug.Log($"Mover {_MoveID} : next {_nextUID}");

        //次のターゲットターゲットまでの最短経路取得
        CalcDikstra(startUID, _nextUID);
    }

    //Sako
    private void ModifyVelocity()
    {
        for (int v = 0; v < lineComponetWeight.Length; v++)
        {
            if (nodeCounter == 0)
            {
                modifiedVelocity = 1;
                continue;
            }

            if ((lineFromNodeVector[v] == nodePoints[nodeCounter - 1] &&
                lineToNodeVector[v] == nodePoints[nodeCounter])
                || (lineToNodeVector[v] == nodePoints[nodeCounter - 1] &&
                lineFromNodeVector[v] == nodePoints[nodeCounter]))
            {
                modifiedVelocity = lineComponetWeight[v];
            }
        }
    }

    //Sako
    private void AssignWait()
    {
        bool assignTriger;
        //Managerクラスのインスタンスの仕方要検討
        Manager manageComponent = manager.GetComponent<Manager>();
        assignTriger = manageComponent.PropertyAssign;
        if (!assignTriger)
        {
            return;
        }
        else
        {
            _nextUID = manageComponent.GetAssignTarget(_MoveID);
            Debug.Log($"{_MoveID} : First Target {_nextUID} ");
            CalcDikstra(startUID, _nextUID);
            RouteSetting();
            IsAsignWait = true;
        }
    }

    #endregion

}