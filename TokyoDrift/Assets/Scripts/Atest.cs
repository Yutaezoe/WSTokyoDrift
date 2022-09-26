using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Linq;

public class Atest : MonoBehaviour
{
    [SerializeField]
    private GameObject gc;
    [SerializeField]
    private GameObject gc2;
    private Target target;
    private Transform[] targetGC;
    private GameObject targetGO;
    private Target targetArray;

    [SerializeField]
    private GameObject gc3;
    private Transform[] lineChildren;
    private GameObject LineGO;
    private Line LineArray;
    private GameObject NodeGO;
    private Node NodeArray;


    [SerializeField]
    private GameObject nodeMasterGO;
    private Transform[] nodeChildren;
    



    // Start is called before the first frame update
    void Start()
    {
        target = gc.GetComponent<Target>();
        target.SetStatusOfPikkingACTIVE();
        target.SetStatusOfPikkingTEMPORARY();

        //Debug.Log(target.GoalPoint);
        //Debug.Log(target.StatusOfPikking);


        //targetGC = GetChildren(gc.transform);
        targetGC = ComFunctions.GetChildren(gc2.transform);
        targetGO = targetGC[0].gameObject;
        targetArray = targetGO.GetComponent<Target>();

        
        //Debug.Log(targetArray.TargetUid);

        //Debug.Log(targetGC[1].gameObject.name);

        //Debug.Log((targetGC.Length));


        lineChildren = ComFunctions.GetChildren(gc3.transform);


        nodeChildren = ComFunctions.GetChildren(nodeMasterGO.transform);


        int[] nodep = new int[nodeChildren.Length*2];

        //Debug.Log(nodep.Length);

        foreach (Transform setTrans in lineChildren)
        {

            //LineGO = setTrans.gameObject;

            //LineArray = LineGO.GetComponent<Line>();

            //NodeGO = LineArray.getAPosition.gameObject;
            //NodeArray = NodeGO.GetComponent<Node>();


            //ComFunctions.GetChildrenComponent(setTrans, out LineArray);

            LineArray = ComFunctions.GetChildrenComponent<Line>(setTrans);

            NodeArray = ComFunctions.GetChildrenComponent<Node>(LineArray.getAPosition);

            //Debug.Log(LineArray);
            //Debug.Log(NodeArray);

        };


    }


    private Transform[] GetChildren(Transform parent)
    {
        // 子オブジェクトを格納する配列作成
        var children = new Transform[parent.childCount];
        var childIndex = 0;

        // 子オブジェクトを順番に配列に格納
        foreach (Transform child in parent)
        {
            children[childIndex++] = child;
        }

        // 子オブジェクトが格納された配列
        return children;
    }


}




