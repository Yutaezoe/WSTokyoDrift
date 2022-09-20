using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
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

        
        Debug.Log(targetArray.TargetUid);

        //Debug.Log(targetGC[1].gameObject.name);

        //Debug.Log((targetGC.Length));


        lineChildren = ComFunctions.GetChildren(gc3.transform);

        foreach (Transform setTrans in lineChildren)
        {

            LineGO = setTrans.gameObject;

            LineArray = LineGO.GetComponent<Line>();

            NodeGO = LineArray.getAPosition.gameObject;
            NodeArray = NodeGO.GetComponent<Node>();
            Debug.Log(NodeArray.getNodeUID);
            
        };


    }


    private Transform[] GetChildren(Transform parent)
    {
        // �q�I�u�W�F�N�g���i�[����z��쐬
        var children = new Transform[parent.childCount];
        var childIndex = 0;

        // �q�I�u�W�F�N�g�����Ԃɔz��Ɋi�[
        foreach (Transform child in parent)
        {
            children[childIndex++] = child;
        }

        // �q�I�u�W�F�N�g���i�[���ꂽ�z��
        return children;
    }


}




