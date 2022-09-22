using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class test2 : MonoBehaviour
{
    [SerializeField]
    GameObject manager;
    int mo = 1;
    int[] ta = new int[3] { 0, 1, 2 };
    int[] di = new int[3] { 15, 25, 35 };
    void Start()
    {
        var compo = manager.GetComponent<Manager>();
        compo.distancePassive(mo, ta, di);
    }
}

