using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;
using System.IO;
using System.Text;

public class Logger : MonoBehaviour
{


    [SerializeField]
    private GameObject moverMaster;

    private Transform[] arrayMover;
    private GameObject[] mover;

    int count = 0;
    float timeleft = 1.0f;
    List<int> moverID = new List<int>();
    List<int> mover1TargetID = new List<int>();
    List<int> mover1MoverID = new List<int>();

    List<DateTime> mover1Time = new List<DateTime>();
    private void Start()
    {
        arrayMover = ComFunctions.GetChildren(moverMaster.transform);
        mover = new GameObject[arrayMover.Length];
        for (int i = 0; i < arrayMover.Length; i++)
        {
            Debug.Log("arrayMover;  " + arrayMover[0]);
            mover[i] = arrayMover[i].gameObject;
            Debug.Log("Mover;  " + mover);

        }
    }

    void Update()
    {
        //timer for counting 1second
        timeleft -= Time.deltaTime;
        if (timeleft <= 0)
        {
           
            //write moverID, targetID and time
            for (int i = 0; i < mover.Length; i++)
            {
                var moverComponent = mover[i].GetComponent<Mover>();
                mover1MoverID.Add(moverComponent.PropertyMoveID);
                mover1TargetID.Add(moverComponent.PropertyTargetID);
                mover1Time.Add(DateTime.Now);
                Debug.Log("MoverID:" + mover1MoverID[count]);
                Debug.Log("TargetID:" + mover1TargetID[count]);
                Debug.Log("Time:" + mover1Time[count]);
            }

            //write to csvfile
            StreamWriter file = new StreamWriter(@"C:\test\test.csv", true, Encoding.UTF8);
                file.WriteLine(string.Format("{0},{1},{2}", mover1MoverID[count], mover1TargetID[count],mover1Time[count]));
            file.Close();

            //reset counter 
            timeleft = 1.0f;
            count += 1;
        }
    }
}