using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Threading.Tasks;

public class TargetBtnCtrl : MonoBehaviour
{
    [SerializeField, Header("Set Edit HUD")] private GameObject editHUD;
    [SerializeField, Header("Set Ray Object")] private GameObject getObjectGO;
    [SerializeField, Header("Set Edit Target Master")] private Transform editTargetMaster;
    [SerializeField, Header("Set Edit Target Prefab")] private GameObject editTargetPrefab;
    [SerializeField, Header("Set Edit Target Prefab")] private Transform editGoalTargetPrefab;

    private GetObject getObject;

    private bool _isTargetHUDActive = false;

    public bool IsTargetHUDActive
    {
        get
        {
            return _isTargetHUDActive;
        }
        set
        {
            this._isTargetHUDActive = value;
        }
    }

    private void Start()
    {
        getObject = getObjectGO.GetComponent<GetObject>();
    }

    private void Update()
    {
        getObject.IsTargetEditMode = _isTargetHUDActive;
    }


    public void ClickTargetSave()
    {
        List<Vector3> listVector3 = new();
        Transform[] childMover;

        childMover = ComFunctions.GetChildren(editTargetMaster);

        foreach (Transform child in childMover)
        {
            listVector3.Add(child.position);
        }

        SaveManager.SaveTargetVector(listVector3.ToArray());


        SaveManager.SaveGoalTargetVector(editGoalTargetPrefab.position);


    }



    public void ClickTargetAdd()
    {
        GameObject childGameObject;

        childGameObject = Instantiate(editTargetPrefab, new Vector3(0, 0.25f, 0), Quaternion.identity);
        childGameObject.transform.SetParent(editTargetMaster, true);
    }


    public void ClickTargetDel()
    {
        getObject.IsTargetDelAllow = true;
    }

    public async void ClickBackEdit()
    {
        await Task.Delay(250);

        gameObject.SetActive(false);
        getObject.IsTargetEditMode = false;
        editHUD.SetActive(true);
    }
}
