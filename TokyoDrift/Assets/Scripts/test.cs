using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class test : MonoBehaviour
{
    //他のオブジェクト存在を認識
    [SerializeField]
    GameObject manager;
    int mo = 0;
    int[] ta = new int[3] { 0, 1, 2 };
    int[] di = new int[3] { 10, 20, 30 };
    void Awake()
    {
        //一度Getcomponentしないとスクリプト参照できない
        var compo = manager.GetComponent<Manager>();
        compo.distancePassive(mo, ta, di);
    }
}
