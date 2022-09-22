using UnityEngine;
using System;
using System.Collections;


namespace Common
{
    class ComFunctions
    {

        public static Transform[] GetChildren(Transform parent)
        {
            // 子オブジェクトを格納する配列作成
            Transform[] children = new Transform[parent.childCount];
            int childIndex = 0;

            // 子オブジェクトを順番に配列に格納
            foreach (Transform child in parent)
            {
                children[childIndex++] = child;
            }

            // 子オブジェクトが格納された配列
            return children;
        }

        public static T GetChildrenComponent<T>(Transform childTransform)
        {
            GameObject childGO;
            childGO = childTransform.gameObject;

#pragma warning disable UNT0014 // Invalid type for call to GetComponent
            return childGO.GetComponent<T>();
#pragma warning restore UNT0014 // Invalid type for call to GetComponent
        }

    }

}
