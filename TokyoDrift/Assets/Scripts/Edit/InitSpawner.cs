using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class InitSpawner : MonoBehaviour
{
    [SerializeField, Header("Set Edit Bus Master")] private Transform editBusMaster;
    [SerializeField, Header("Set Edit Bus Prefab")] private GameObject editBusPrefab;
    [SerializeField, Header("Set Edit Target Master")] private Transform editTargetMaster;
    [SerializeField, Header("Set Edit Target Prefab")] private GameObject editTargetPrefab;


    // Start is called before the first frame update
    void Awake()
    {
        Vector3[] loadVector3;
        GameObject childGameObject;

        // Target
        Vector3[] loadTargetVector3;
        GameObject childTargetGameObject;


        SaveManager.Load();

        if (SaveManager.saveData.moverVector != null)
        {
            loadVector3 = SaveManager.saveData.moverVector;
            foreach (Vector3 load in loadVector3)
            {
                childGameObject = Instantiate(editBusPrefab, load, Quaternion.identity);
                childGameObject.transform.SetParent(editBusMaster, true);

            }
        }

        if (SaveManager.saveData.targetVector != null)
        {
            loadTargetVector3 = SaveManager.saveData.targetVector;
            foreach (Vector3 load in loadTargetVector3)
            {
                childTargetGameObject = Instantiate(editTargetPrefab, load, Quaternion.identity);
                childTargetGameObject.transform.SetParent(editTargetMaster, true);

            }
        }


    }


}
