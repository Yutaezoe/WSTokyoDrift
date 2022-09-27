using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObject : MonoBehaviour
{
    [SerializeField]
    public Camera cameraObject; //カメラを取得



    private RaycastHit hit; //レイキャストが当たったものを取得する入れ物
    private GameObject hitObject;
    private EditMover getEditMover;

    private bool _isDelAllow;

    public bool IsDelAllow
    {
        set
        {
            this._isDelAllow = value;
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        {
            LeftClickFunction();
        }

        if (getEditMover != null)
        {
            if (_isDelAllow)
            {
                getEditMover.IsDelAllowMover = true;
                _isDelAllow = false;
            }
        }
        else
        {
            _isDelAllow = false;
        }


    }




    private void LeftClickFunction()
    {

        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入

        if (Physics.Raycast(ray, out hit))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
        {


            // 目標物以外をクリックした場合エラースロー
            try
            {

                if (getEditMover == null) // コンポーネントが存在するか
                {
                    hitObject = hit.collider.gameObject;
                    getEditMover = hitObject.GetComponent<EditMover>();
                    getEditMover.IsMoveAllow = true;
                }
                else if (hitObject.name == hit.collider.gameObject.name)
                {
                    getEditMover.IsMoveAllow = false;
                    getEditMover = null;
                }
                else
                {
                    getEditMover.IsMoveAllow = false;
                    hitObject = hit.collider.gameObject;
                    getEditMover = hitObject.GetComponent<EditMover>();
                    getEditMover.IsMoveAllow = true;
                }

            }
            catch // other object touch
            {

                if (getEditMover != null)
                {
                    getEditMover.IsMoveAllow = false;
                    getEditMover = null;
                }
            }
        }
    }
}
