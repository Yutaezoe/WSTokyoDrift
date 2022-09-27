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
    List<int> moverTargetID = new List<int>();
    List<int> moverMoverID = new List<int>();

    List<DateTime> moverTime = new List<DateTime>();

    private bool loggerStatus = true;
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
            if (loggerStatus == true)
            {

                //write moverID, targetID and time
                for (int i = 0; i < mover.Length; i++)
                {
                    var moverComponent = mover[i].GetComponent<Mover>();
                    moverMoverID.Add(moverComponent.PropertyMoveID);
                    moverTargetID.Add(moverComponent.PropertyTargettingPoint);
                    moverTime.Add(DateTime.Now);
                    //Debug.Log("MoverID:" + moverMoverID[count]);
                    //Debug.Log("TargetID:" + moverTargetID[count]);
                    //Debug.Log("Time:" + moverTime[count]);
                }


                //write to csvfile
                StreamWriter file = new StreamWriter(@"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\CSV\result.csv", true, Encoding.UTF8);
                file.WriteLine(string.Format("{0},{1},{2}", moverMoverID[count], moverTargetID[count], moverTime[count]));
                file.Close();

            }
            //reset counter 
            timeleft = 1.0f;
            count += 1;
        }

    }

    public void StartLogger()
    {
        loggerStatus = true;
    }

}
