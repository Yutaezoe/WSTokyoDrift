using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class Atest : MonoBehaviour
{
    [SerializeField]
    private GameObject gc;
    private Target target;
    private Transform[] targetGC;

    

    // Start is called before the first frame update
    void Start()
    {
        target = gc.GetComponent<Target>();
        target.SetStatusOfPikkingACTIVE();
        target.SetStatusOfPikkingTEMPORARY();

        Debug.Log(target.GoalPoint);
        Debug.Log(target.StatusOfPikking);


        //targetGC = GetChildren(gc.transform);
        targetGC = ComFunctions.GetChildren(gc.transform);
        Debug.Log(targetGC[1].gameObject.name);

        Debug.Log((targetGC.Length));

    }

    // Update is called once per frame
    void Update()
    {
        
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




