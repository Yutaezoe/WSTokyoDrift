using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SimSpawner : MonoBehaviour
{
    [SerializeField, Header("Set Bus Master")] private Transform BusMaster;
    [SerializeField, Header("Set Bus Prefab")] private GameObject BusPrefab;
    [SerializeField, Header("Set Target Master")] private Transform TargetMaster;
    [SerializeField, Header("Set Target Prefab")] private GameObject TargetPrefab;
    [SerializeField, Header("Set Goal Prefab")] private GameObject GoalPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3[] loadVector3;
        GameObject childGameObject;

        // Target
        Vector3[] loadTargetVector3;
        GameObject childTargetGameObject;

        // Target
        Vector3 loadGoalVector3;
        GameObject goalGameObject;


        SaveManager.Load();
        

        if (SaveManager.saveData.moverVector != null)
        {
            int attachID = 0;
            loadVector3 = SaveManager.saveData.moverVector;
            foreach (Vector3 load in loadVector3)
            {
                childGameObject = Instantiate(BusPrefab, load, Quaternion.identity);
                childGameObject.transform.SetParent(BusMaster, true);
                Mover mover = childGameObject.GetComponent<Mover>();
                mover.PropertyMoveID = attachID;
                attachID++;
            }
        }

        if (SaveManager.saveData.targetGoalVector != null)
        {
            loadGoalVector3 = SaveManager.saveData.targetGoalVector;

            goalGameObject = Instantiate(GoalPrefab, loadGoalVector3, Quaternion.identity);
            goalGameObject.transform.SetParent(TargetMaster, true);

        }

        if (SaveManager.saveData.targetVector != null)
        {
            loadTargetVector3 = SaveManager.saveData.targetVector;
            foreach (Vector3 load in loadTargetVector3)
            {
                childTargetGameObject = Instantiate(TargetPrefab, load, Quaternion.identity);
                childTargetGameObject.transform.SetParent(TargetMaster, true);

            }
        }

      
    }

}
