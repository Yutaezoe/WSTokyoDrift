using UnityEngine;



namespace Common
{
    class ComFunctions
    {

        public static Transform[] GetChildren(Transform parent)
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
}
