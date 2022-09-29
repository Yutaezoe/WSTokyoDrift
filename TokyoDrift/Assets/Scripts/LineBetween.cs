using System;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class LineBetween : MonoBehaviour
{

    private Vector3 pointAPosition;
    private Vector3 pointBPosition;


    private Transform[] lineChildren;
    private GameObject LineGO;
    private Line LineArray;



    void Start()
{

        lineChildren = ComFunctions.GetChildren(gameObject.transform);
        if (lineChildren == null)
        {
            return;
        }

        foreach (Transform setTrans in lineChildren)
        {

            LineGO = setTrans.gameObject;

            LineArray = LineGO.GetComponent<Line>();

            pointAPosition = LineArray.getAPosition.position;
            pointBPosition = LineArray.getBPosition.position;
            int lineWeight = LineArray.getLineWeight;


            // LineRendererコンポーネントをゲームオブジェクトにアタッチする
            var lineRenderer = LineGO.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var positions = new Vector3[]{
        new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // 開始点
        new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // 終了点
    };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);

        lineRenderer.startWidth = 0.1f;                   // 開始点の太さを0.1にする
        lineRenderer.endWidth = 0.1f;                     // 終了点の太さを0.1にする
         
            Color weightColor = new Color(0.5f, 0.5f, 0, 1);
            float correctionFront = 0.5f - (float)lineWeight/10f;
            float correctionEnd =  0.5f * (float)lineWeight/10f;
            if (lineWeight <= 5)
            {
                weightColor = new Color(0.5f - correctionFront, 0.5f + correctionFront, 0, 1);
            }else
            {
                weightColor = new Color((float)lineWeight/10f, 0, 0, 1);
            }
        lineRenderer.startColor = weightColor;
        lineRenderer.endColor = weightColor;


        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
                
        if (lineChildren == null)
        {
            lineChildren = ComFunctions.GetChildren(gameObject.transform);
            return;
        }

        foreach (Transform setTrans in lineChildren)
        {

            LineGO = setTrans.gameObject;

            LineArray = LineGO.GetComponent<Line>();

            pointAPosition = LineArray.getAPosition.position;
            pointBPosition = LineArray.getBPosition.position;

            LineRenderer lineRenderer = LineGO.GetComponent<LineRenderer>();

            Vector3[] positions = new Vector3[]{
            new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // 開始点
            new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // 終了点
            };

            // 線を引く場所を指定する
            lineRenderer.SetPositions(positions);

        }

    }

#endif


}