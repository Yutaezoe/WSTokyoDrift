using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SetPosition : MonoBehaviour
{
    //TargetGoal
    Vector3 loadGoalVector3;


    // Start is called before the first frame update
    void Awake()
    {
        SaveManager.Load();

        if (SaveManager.saveData.targetGoalVector != new Vector3(0, 0, 0) && gameObject.name == "Goal")
        {
            loadGoalVector3 = SaveManager.saveData.targetGoalVector;

            gameObject.transform.position = loadGoalVector3;
        }
    }
}
