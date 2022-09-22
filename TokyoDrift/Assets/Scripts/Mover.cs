using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Linq;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private int MoveID;

    private int TargetID;

    //[SerializeField]
    //private Transform startNodePoint;
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
    private GameObject moverNodeGO;
    private Node moverNodeArray;
    private int point;
    private int[] routeReturn;
    //

    private Vector3[] nodeVector;
    private Vector3 startNodeVector;
    private object nodeVetor;
    private int nodeCounter;
    private int count=0;

    //Setting Property
    public int PropertyMoveID => MoveID;
    public int PropertyTargetID => TargetID;


    private void Awake()
    {
        
        
        // Ezoe Edit
        lineChildren = ComFunctions.GetChildren(lineMaster.transform);
        //

        // SakoSako Edit
        nodeChildren = ComFunctions.GetChildren(nodeMaster.transform);
        //

    }


    // Start is called before the first frame update
    void Start()
    {

        List<Transform> nodeTrans = new List<Transform>();

        //Ezoe
        CalcDikstra(0,11);
        //Ezoe
   

        //Sako 
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
        //

        Debug.Log(nodePoints[0]);

        startNodeVector = nodePoints[0].position;
        startNodeVector.y = 0.25f;
        transform.position = startNodeVector;

        nodeVector = new Vector3[nodePoints.Length];

        for (int i = 0; i < nodeVector.Length; i++)
        {
            nodeVector[i] = nodePoints[i].position;
            nodeVector[i].y = 0.25f;
        }

        //Startを走らせるためだけのカウンター、temp
        nodeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
    
        MoveMobility();

        count += 1;
        if (count > 1000)
        {
            TargetID += 1;
            count = 0;
        }
    }

    private void MoveMobility()
    {
     
        if (nodeCounter != nodePoints.Length)
        {

            transform.position = Vector3.MoveTowards(transform.position, nodeVector[nodeCounter], 1.5f * Time.deltaTime);

            if (transform.position.x == nodePoints[nodeCounter].position.x && transform.position.z == nodePoints[nodeCounter].position.z)
            {
                transform.position = Vector3.MoveTowards(transform.position, nodeVector[nodeCounter], 1.5f * Time.deltaTime);
                nodeCounter++;
            }
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
            Debug.Log(r);
        }

        routeReturn = graph.RouteReturn;

    }


}
