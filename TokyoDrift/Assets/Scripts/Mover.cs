using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private int MoveID;

    private int TargetID = 2;

    [SerializeField]
    private Transform startNodePoint;
    [SerializeField]
    private Transform[] nodePoints;

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


}
