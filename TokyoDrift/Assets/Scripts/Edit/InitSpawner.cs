using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class InitSpawner : MonoBehaviour
{
    [SerializeField, Header("Set Edit Bus Master")] private Transform editBusMaster;
    [SerializeField, Header("Set Edit Bus Prefab")] private GameObject editBusPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3[] loadVector3;
        GameObject childGameObject;

        SaveManager.Load();

        if (SaveManager.saveData.moverVector != null)
        {
            loadVector3 = SaveManager.saveData.moverVector;
            Debug.Log(loadVector3);
            foreach (Vector3 load in loadVector3)
            {
                childGameObject = Instantiate(editBusPrefab, load, Quaternion.identity);
                childGameObject.transform.SetParent(editBusMaster, true);

            }
        }

        

        


    }


}
