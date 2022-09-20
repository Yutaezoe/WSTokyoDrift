using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    [SerializeField, Range(1, 10)]
    private int lineWeight=1;


    public Transform getAPosition { get { return pointA; } }
    public Transform getBPosition { get { return pointB; } }
    public int getLineWeight { get { return lineWeight; } }


}
