using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MoverBtnCtrl : MonoBehaviour
{

    [SerializeField, Header("Set Edit HUD")]    private GameObject editHUD;
    [SerializeField, Header("Set Edit HUD")]    private GameObject getObjectGO;

    private GetObject getObject;

    private void Start()
    {

        getObject = getObjectGO.GetComponent<GetObject>();
    }



    public void ClickMoverDel()
    {

        getObject.IsDelAllow = true;


    }


    public async void ClickBackEdit()
    {

        await Task.Delay(250);

        gameObject.SetActive(false);
        editHUD.SetActive(true);

    }


}
