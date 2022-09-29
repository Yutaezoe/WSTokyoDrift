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
    private EditTarget getEditTarget;

    // Mover
    private bool _isMoverEditMode;
    private bool _isMoverDelAllow;

    // Target
    private bool _isTargetEditMode;
    private bool _isTargetDelAllow;


    // Mover 
    public bool IsMoverEditMode{set{this._isMoverEditMode = value;}}
    public bool IsMoverDelAllow{set{this._isMoverDelAllow = value;}}

    // Target
    public bool IsTargetEditMode { set { this._isTargetEditMode = value; } }
    public bool IsTargetDelAllow { set { this._isTargetDelAllow = value; } }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        {
            LeftClickFunction();
        }
        
        // Todo 冗長

        // Mover
        MoverDelCtrl();
        MoverDisable();

        // Target
        TargetDelCtrl();
        TargetDisable();

    }


    private void MoverDelCtrl()
    {
        // When the mover is selected and the HUD button is pressed.
        if (getEditMover != null) //Mover was clicked
        {
            //Debug.Log(this._isMoverDelAllow);
            if (_isMoverDelAllow)// HUD Del pressed By MoverBtnCtrl.cs
            {
                getEditMover.IsDelAllowMover = true;
                _isMoverDelAllow = false;
            }
        }
        else
        {
            _isMoverDelAllow = false;
        }
    }

    private void MoverDisable()
    {
        if (_isMoverEditMode == false & getEditMover != null)
        {
            getEditMover.IsMoveAllow = false;
            getEditMover = null;
        }
    }


    private void TargetDelCtrl()
    {
        // When the mover is selected and the HUD button is pressed.
        if (getEditTarget != null) //Mover was clicked
        {
            if (_isTargetDelAllow)// HUD Del pressed By TargetBtnCtrl.cs
            {
                getEditTarget.IsDelAllowTarget = true;
                _isTargetDelAllow = false;
            }
        }
        else
        {
            _isTargetDelAllow = false;
        }
    }

    private void TargetDisable()
    {
        if (_isTargetEditMode == false & getEditTarget != null)
        {
            getEditTarget.IsTargetAllow = false;
            getEditTarget = null;
        }
    }





    private void LeftClickFunction()
    {

        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入

        if (Physics.Raycast(ray, out hit))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
        {

            if (hit.collider.gameObject.name!="Plane")
            {



                // Moverをクリックしたときの処理
                try
                {
                if (getEditMover == null) // コンポーネントが存在するか
                {
                    hitObject = hit.collider.gameObject;
                    getEditMover = hitObject.GetComponent<EditMover>();
                    getEditMover.IsMoveAllow = true;
                }
                else if (hitObject.transform.position == hit.collider.gameObject.transform.position)
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


                // Targetをクリックしたときの処理
                try
                {
                    if (getEditTarget == null) // コンポーネントが存在するか
                    {
                        hitObject = hit.collider.gameObject;
                        getEditTarget = hitObject.GetComponent<EditTarget>();
                        getEditTarget.IsTargetAllow = true;
                    }
                    else if (hitObject.transform.position == hit.collider.gameObject.transform.position)
                    {
                        getEditTarget.IsTargetAllow = false;
                        getEditTarget = null;
                    }
                    else
                    {
                        getEditTarget.IsTargetAllow = false;
                        hitObject = hit.collider.gameObject;
                        getEditTarget = hitObject.GetComponent<EditTarget>();
                        getEditTarget.IsTargetAllow = true;
                    }
                }
                catch // other object touch
                {

                    if (getEditTarget != null)
                    {
                        getEditTarget.IsTargetAllow = false;
                        getEditTarget = null;
                    }
                }



            }

        }


        
    }

}



