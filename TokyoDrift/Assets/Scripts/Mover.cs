using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Linq;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private int MoveID;

    private int TargetID = 2;

    [SerializeField]
    private Transform startNodePoint;
    [SerializeField]
    private Transform[] nodePoints;

    // Ezoe edit
    [SerializeField]
    private GameObject lineMaster;

    private Transform[] lineChildren;
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
        startNodeVector = startNodePoint.position;
        startNodeVector.y = 0.25f;
        transform.position = startNodeVector;
        //  SearchTarget search = new SearchTarget(nodePoints.Length);

        // Ezoe Edit
        lineChildren = ComFunctions.GetChildren(lineMaster.transform);
        //


    }


    // Start is called before the first frame update
    void Start()
    {
        nodeVector = new Vector3[nodePoints.Length];

        for (int i = 0; i < nodeVector.Length; i++)
        {
            nodeVector[i] = nodePoints[i].position;
            nodeVector[i].y = 0.25f;
        }
        nodeCounter = 0;

        //Ezoe
        CalcDikstra(2, 5);
        //Ezoe
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
