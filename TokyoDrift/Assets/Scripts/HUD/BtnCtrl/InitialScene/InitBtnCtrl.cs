using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitBtnCtrl : MonoBehaviour
{

    public void ChangeScene()
    {
        SceneManager.LoadScene("SimScene");
    }

    public void ChangeEditScene()
    {
        SceneManager.LoadScene("EditScene");
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
