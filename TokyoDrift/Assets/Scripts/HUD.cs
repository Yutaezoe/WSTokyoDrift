using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    TextMeshProUGUI remainingText;

    [SerializeField]
    GameObject targetMaster;
    [SerializeField]
    GameObject timer;

    const string TIMEPREFIX = "Time: ";
    const string REMAININGPREFIX = "Remaining: ";
    float timeleft = 1.0f;

    private Transform[] arrayTarget;


    // Start is called before the first frame update
    void Start()
    {
    Timer timercompo = timer.GetComponent<Timer>();
    timeText.text = TIMEPREFIX + timercompo.GetTimerCount;

        // TargetMaster targetMasterCompo = targetMaster.GetComponent<TargetMaster>();
        arrayTarget=ComFunctions.GetChildren(targetMaster.transform);
        remainingText.text = REMAININGPREFIX + arrayTarget.Length;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
    Timer timercompo = timer.GetComponent<Timer>();
    timeText.text = TIMEPREFIX + timercompo.GetTimerCount;

        if (timeleft <= 0)
        {
            arrayTarget = ComFunctions.GetChildren(targetMaster.transform);
            remainingText.text = REMAININGPREFIX + arrayTarget.Length;
            timeleft = 1.0f;
        }

    }
}
