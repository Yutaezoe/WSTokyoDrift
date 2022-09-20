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


            // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
            var lineRenderer = LineGO.GetComponent<LineRenderer>();

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
            new Vector3(pointAPosition.x, pointAPosition.y, pointAPosition.z),               // �J�n�_
            new Vector3(pointBPosition.x, pointBPosition.y, pointBPosition.z),               // �I���_
            };

            // ���������ꏊ���w�肷��
            lineRenderer.SetPositions(positions);

        }

    }

#endif


}