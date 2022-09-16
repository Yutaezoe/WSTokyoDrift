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
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var positions = new Vector3[]{
        new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // �J�n�_
        new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // �I���_
    };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);

        lineRenderer.startWidth = 0.1f;                   // �J�n�_�̑�����0.1�ɂ���
        lineRenderer.endWidth = 0.1f;                     // �I���_�̑�����0.1�ɂ���
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {



        pointAPosition = pointA.position;
        pointBPosition = pointB.position;
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        LineRenderer lineRenderer = lineRendererObject;

        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        var positions = new Vector3[]{
        new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // �J�n�_
        new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // �I���_
        };

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);

        //lineRenderer.startWidth = 0.1f;                   // �J�n�_�̑�����0.1�ɂ���
        //lineRenderer.endWidth = 0.1f;                     // �I���_�̑�����0.1�ɂ���
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.green;


    }


#endif


}