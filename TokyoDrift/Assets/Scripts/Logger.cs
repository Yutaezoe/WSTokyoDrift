using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;
using System.IO;
using System.Text;

public class Logger : MonoBehaviour
{


    [SerializeField] private GameObject moverMaster;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject manager;


    private Transform[] arrayMover;
    private GameObject[] mover;

    int count = 0;
    float timeleft = 1.0f;
    List<int> moverID = new List<int>();
    List<int> moverTargetID = new List<int>();
    List<int> moverMoverID = new List<int>();
    List<DateTime> moverTime = new List<DateTime>();
    List<string> moverTimeCount = new List<string>();
    List<int> remainingCount = new List<int>();
    List<bool> goalStatus = new List<bool>();



    //state relative path
    static string path = Directory.GetCurrentDirectory();

    private bool loggerStatus = true;
    private void Start()
    {
        arrayMover = ComFunctions.GetChildren(moverMaster.transform);
        mover = new GameObject[arrayMover.Length];
        for (int i = 0; i < arrayMover.Length; i++)
        {
            mover[i] = arrayMover[i].gameObject;
        }
        using (var fileStream = new FileStream(path + @"\Assets\Python\csv\result.csv", FileMode.Open))
        {
            //Delate toPython.csv
            // ストリームの長さを0に設定します。
            // 結果としてファイルのサイズが0になります。
            fileStream.SetLength(0);
        }
    }

    void Update()
    {
        Timer timercompo = timer.GetComponent<Timer>();
        HUD HUDcompo = HUD.GetComponent<HUD>();
        Manager ManagerComp = manager.GetComponent<Manager>();
        //timer for counting 1second
        timeleft -= Time.deltaTime;
        if (timeleft <= 0)
        {
            if (loggerStatus == true)
            {

                //write moverID, targetID and time
                for (int i = 0; i < mover.Length; i++)
                {
                    Mover moverComponent = mover[i].GetComponent<Mover>();
                    moverMoverID.Add(moverComponent.PropertyMoveID);
                    moverTargetID.Add(moverComponent.PropertyTargettingPoint);
                    moverTime.Add(DateTime.Now);
                    moverTimeCount.Add(timercompo.GetTimerCount);
                    remainingCount.Add(HUDcompo.PropetyRemaining);
                    goalStatus.Add(moverComponent.PropertyGoalTrigger);

                    StreamWriter file = new StreamWriter(path + @"\Assets\Python\csv\result.csv", true, Encoding.UTF8);

                    file.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", moverMoverID[count], moverTargetID[count], moverTime[count], moverTimeCount[count], remainingCount[count],goalStatus[count]));
                    file.Close();
                    count += 1;
                }

                ManagerComp.CheckSimComplete();

            }
            //reset counter 
            timeleft = 1.0f;

        }

    }

    public void StartLogger()
    {
        loggerStatus = true;
    }

    public void StopLogger()
    {
        loggerStatus = false;
    }
}
