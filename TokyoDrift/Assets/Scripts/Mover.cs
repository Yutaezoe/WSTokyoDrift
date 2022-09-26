using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Linq;

public class Mover : MonoBehaviour
{

    [SerializeField]
    private int MoveID;

    private int[] TargetID;

    // Initial Position
    [SerializeField]
    private GameObject startNodePoint;
    private Node startUID;

    // Target Lists
    [SerializeField]
    private Transform targetMaster;



    //[SerializeField]
    private Transform[] nodePoints;

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

    //Setting Property
    public int PropertyMoveID => MoveID;
    public int[] PropertyTargetID { get { return TargetID; } set { this.TargetID = value; } }

    private void Awake()
    { 
        // Ezoe Edit
        lineChildren = ComFunctions.GetChildren(lineMaster.transform);
        //
        // SakoSako Edit
        nodeChildren = ComFunctions.GetChildren(nodeMaster.transform);
        //
        targetChildren = ComFunctions.GetChildren(targetMaster.transform);
        //
    }


    // Start is called before the first frame update
    void Start()
    {
        /////マネージャーへの受け渡し用配列定義
        Manager manager = new();
        List<int> targetIDList = new List<int>();
        List<int> minDistance = new List<int>();
        /////

        startUID = startNodePoint.GetComponent<Node>();
        Debug.Log(startUID.getNodeUID);
        //Ezoe
        CalcDikstra(startUID.getNodeUID, 11);
        //Ezoe
        SettingComponent();

        /////////マネージャーへの受け渡し用配列作成
        for (int setTarget = 0; setTarget < targetNearNodeId.Length; setTarget++)
        {
            targetIDList.Add(targetNearNodeId[setTarget]);
            minDistance.Add((int)costReturn[targetNearNodeId[setTarget]]);
        }

        TargetID = targetIDList.ToArray();
        eachTargetDistance = minDistance.ToArray();

        for (int t = 0; t < targetNearNodeId.Length; t++)
        {
            Debug.Log($"TargetID:{TargetID[t]} ,Cost:{eachTargetDistance[t]} ");
        }
        /////////

        //マネージャーに各ターゲットへのデータ渡し
        manager.distancePassive(MoveID,TargetID,eachTargetDistance);

        //Sako 
        RouteSetting();
    }
        
    // Update is called once per frame
    void Update()
    {
        if (nodeCounter != nodePoints.Length)
        {
            MoveMobility();
        }
    }

    private void SettingComponent()
    {
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
            Debug.Log(tempNearNode);
        }

        targetNearNodeId = targetNearNodeList.ToArray();
    }

    private void MoveMobility()
    {

            transform.position = Vector3.MoveTowards(transform.position, nodeVector[nodeCounter], 1.5f * Time.deltaTime);

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
        List<Transform> nodeTrans = new List<Transform>();

        for (int t = 0; t < routeReturn.Length; t++)
        {
            foreach (Transform setNode in nodeChildren)
            {
                moverNodeGO = setNode.gameObject;
                moverNodeArray = moverNodeGO.GetComponent<Node>();
                point = moverNodeArray.getNodeUID;

                if (point == routeReturn[t])
                {
                    nodeTrans.Add(setNode);
                }
            }
            nodePoints = nodeTrans.ToArray();
        }

        startNodeVector = nodePoints[0].position;
        startNodeVector.y = 0.25f;
        transform.position = startNodeVector;

        nodeVector = new Vector3[nodePoints.Length];

        for (int i = 0; i < nodeVector.Length; i++)
        {
            nodeVector[i] = nodePoints[i].position;
            nodeVector[i].y = 0.25f;
        }
    }

}
