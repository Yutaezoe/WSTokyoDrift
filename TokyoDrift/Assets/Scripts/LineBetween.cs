using System;
using UnityEngine;
using UnityEngine.UI;

public class LineBetween : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField]
    private LineRenderer lineRendererObject;

    private Vector3 pointAPosition;
    private Vector3 pointBPosition;


    void Start()
{
        pointAPosition = pointA.position;
        pointBPosition = pointB.position;
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var positions = new Vector3[]{
        new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // 開始点
        new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // 終了点
    };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);

        lineRenderer.startWidth = 0.1f;                   // 開始点の太さを0.1にする
        lineRenderer.endWidth = 0.1f;                     // 終了点の太さを0.1にする
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {



        pointAPosition = pointA.position;
        pointBPosition = pointB.position;
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        LineRenderer lineRenderer = lineRendererObject;

        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var positions = new Vector3[]{
        new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // 開始点
        new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // 終了点
        };

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);

        //lineRenderer.startWidth = 0.1f;                   // 開始点の太さを0.1にする
        //lineRenderer.endWidth = 0.1f;                     // 終了点の太さを0.1にする
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.green;


    }


#endif


}