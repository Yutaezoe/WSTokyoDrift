using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Linq;

public class Mover : MonoBehaviour
{

    [SerializeField]
    private int _MoveID;

    private int[] _TargetID;

    // Initial Position
    [SerializeField]
    private GameObject startNodePoint;
    private int startUID;

    // Goal Position
    private int _nextUID;
    private int goalUID;

    // Target Lists
    [SerializeField]
    private Transform targetMaster;

    //[SerializeField]
    private Vector3[] nodePoints;

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
    public int PropertyMoveID => _MoveID;
    public int PropertyTargettingPoint => _nextUID;
    public int[] PropertyTargetID { get { return _TargetID; } set { this._TargetID = value; } }

    private void Awake()
    { 
        // Ezoe Edit
        lineChildren = ComFunctions.GetChildren(lineMaster.transform);
        nodeChildren = ComFunctions.GetChildren(nodeMaster.transform);
        //targetChildren = ComFunctions.GetChildren(targetMaster.transform);
        //

        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ezoe
        //Field�̏�Ԃ�c��
        SettingComponent();

        //Sako
        //�X�^�[�g�n�_�ƃS�[���n�_���`
        SettingStartAndGoal();

        //Ezoe
        //�}�l�[�W���[�֑���p�̃_�C�N�X�g���v�Z
        CalcDikstra(startUID, _nextUID);

        //Sako
        //�}�l�[�W���[�Ɋe�^�[�Q�b�g�ւ̃f�[�^�n��
        Manager manager = new();
        SettingEachTargetDistance();
        manager.distancePassive(_MoveID, _TargetID,eachTargetDistance);

        DecesionTarget();

        //Sako 
        RouteSetting();
    }
        
    // Update is called once per frame
    void Update()
    {
       
            //�펞�A�c�^�[�Q�b�g���m�F
       SettingComponent();

       if (nodeCounter != nodePoints.Length)
       {
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

        // Line���UID �d�� �ڑ��ʒu�̎擾���s��
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

        // Target�̈ʒu�̎擾���s��
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
        startUID = startNodePoint.GetComponent<Node>().getNodeUID;

        foreach (Transform goalTarget in targetChildren)
        {
            Target goalCheck;
            goalCheck = ComFunctions.GetChildrenComponent<Target>(goalTarget);

            if (goalCheck.GoalPoint)
            {
                goalUID = targetNearNodeId[goalCheck.TargetUid];
                _nextUID = goalUID;
            }
        }
    }

    //Sako
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
            //�q���C������A���C���d�݁ApoinA,B��z��Ɋi�[
            
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

        //�s�v�ł́H
        for (int i = 0; i < nodeVector.Length; i++)
        {
            nodeVector[i] = nodePoints[i];
            nodeVector[i].y = 0.25f;
        }
    }

    //Sako
    private void SettingEachTargetDistance()
    {
        /////�}�l�[�W���[�ւ̎󂯓n���p�z���`
        List<int> targetIDList = new List<int>();
        List<int> minDistance = new List<int>();
        /////
        /////////�}�l�[�W���[�ւ̎󂯓n���p�z��쐬
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

        //�_�C�N�X�g���̃v���v�Z
        CalcDikstra(startUID, _nextUID);

        //�^�[�Q�b�g�̒�����ŒZ�o�H�̂��̂𒊏o
        long costRetrunTemp = 100000;
        foreach(int tar in targetNearNodeId)
        {
            //�c�^�[�Q�b�g��2�ȏ��goalUID�͌�₩�珜�O
            if ( tar == goalUID && targetChildren.Length != 1 )
            {
                continue;
            }
            
            //�ł��R�X�g�̒ႢTarget�𒊏o
            if(costReturn[tar] < costRetrunTemp)
            {
                _nextUID = tar;
            }
            costRetrunTemp = costReturn[tar];
        }

        //���̃^�[�Q�b�g�^�[�Q�b�g�܂ł̍ŒZ�o�H�擾
        CalcDikstra(startUID, _nextUID);
    }

}