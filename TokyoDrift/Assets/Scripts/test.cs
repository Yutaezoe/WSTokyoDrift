using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class test : MonoBehaviour
{
    //���̃I�u�W�F�N�g���݂�F��
    [SerializeField]
    GameObject manager;
    int mo = 0;
    int[] ta = new int[3] { 0, 1, 2 };
    int[] di = new int[3] { 10, 20, 30 };
    void Awake()
    {
        //��xGetcomponent���Ȃ��ƃX�N���v�g�Q�Ƃł��Ȃ�
        var compo = manager.GetComponent<Manager>();
        compo.distancePassive(mo, ta, di);
    }
}
