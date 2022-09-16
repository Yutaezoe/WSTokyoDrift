using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{


    [SerializeField]
    private Transform startNodePoint;
    [SerializeField]
    private Transform[] nodePoints;
    [SerializeField]
    private float speed;


    private Vector3[] nodeVector;
    private Vector3 startNodeVector;

    private void Awake()
    {

        startNodeVector = startNodePoint.position;

        startNodeVector.y = 0.25f;

        transform.position = startNodeVector;
    }


    // Start is called before the first frame update
    void Start()
    {

        nodeVector = new Vector3[nodePoints.Length];

        for(int i = 0; i < nodeVector.Length; i++)
        {

            nodeVector[i] = nodePoints[i].position;
            nodeVector[i].y = 0.25f;
            Debug.Log(nodeVector[i]);
        }

        
        



    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, nodeVector[0], speed * Time.deltaTime);
        //transform.position

    }
}
