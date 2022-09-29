using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;
public class EditBtnMainCtrl : MonoBehaviour
{

    [SerializeField, Header("Set Target HUD")]
    private GameObject TargetHUD;
    [SerializeField, Header("Set Node HUD")]
    private GameObject NodeHUD;
    [SerializeField, Header("Set Line HUD")]
    private GameObject LineHUD;
    [SerializeField, Header("Set Mover HUD")]
    private GameObject MoverHUD;


    private MoverBtnCtrl moverBtnCtrl;
    private TargetBtnCtrl targetBtnCtrl;

    private void Start()
    {
        moverBtnCtrl = MoverHUD.GetComponent<MoverBtnCtrl>();
        targetBtnCtrl = TargetHUD.GetComponent<TargetBtnCtrl>();
    }


    public async void ClickNodeEdit()
    {

        await Task.Delay(250);

        gameObject.SetActive(false);
        NodeHUD.SetActive(true);

    }

    public async void ClickLineEdit()
    {

        await Task.Delay(250);

        gameObject.SetActive(false);
        LineHUD.SetActive(true);

    }

    public async void ClickMoverEdit()
    {

        await Task.Delay(250);

        gameObject.SetActive(false);
        MoverHUD.SetActive(true);
        moverBtnCtrl.IsMoverHUDActive = true;



    }

    public async void ClickTargetEdit()
    {

        await Task.Delay(250);

        gameObject.SetActive(false);
        TargetHUD.SetActive(true);
        targetBtnCtrl.IsTargetHUDActive = true;


    }


    public void BackChangeScene()
    {
        SceneManager.LoadScene("InitialScene");
    }

}
