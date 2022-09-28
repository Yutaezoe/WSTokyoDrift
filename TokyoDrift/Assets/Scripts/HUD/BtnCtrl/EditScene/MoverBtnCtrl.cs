using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Common;

public class MoverBtnCtrl : MonoBehaviour
{
    [SerializeField, Header("Set Edit HUD")] private GameObject editHUD;
    [SerializeField, Header("Set Ray Object")] private GameObject getObjectGO;
    [SerializeField, Header("Set Edit Bus Master")] private Transform editBusMaster;
    [SerializeField, Header("Set Edit Bus Prefab")] private GameObject editBusPrefab;

    private GetObject getObject;

    private bool _isMoverHUDActive = false;

    public bool IsMoverHUDActive
        {
            get
            {
                return _isMoverHUDActive;
            }
            set
            {
                this._isMoverHUDActive = value;
            }
        }


    private void Start()
    {
        getObject = getObjectGO.GetComponent<GetObject>();
    }

    private void Update()
    {
        getObject.IsMoverEditMode = _isMoverHUDActive;
    }


    public void ClickMoverSave()
    {
        List<Vector3> listVector3 = new ();
        Transform[] childMover;
        
        childMover = ComFunctions.GetChildren(editBusMaster);

        foreach(Transform child in childMover)
        {
            listVector3.Add(child.position);
        }

        SaveManager.SaveMoverVector(listVector3.ToArray());

    }



    public void ClickMoverAdd()
    {
        GameObject childGameObject;

        childGameObject = Instantiate(editBusPrefab, new Vector3(0, 0.25f, 0), Quaternion.identity);
        childGameObject.transform.SetParent(editBusMaster, true);
    }


    public void ClickMoverDel()
    {
        getObject.IsMoverDelAllow = true; 
    }

    public async void ClickBackEdit()
    {
        await Task.Delay(250);

        gameObject.SetActive(false);
        getObject.IsMoverEditMode  = false;
        editHUD.SetActive(true);
    }


}
