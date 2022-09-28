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

    private bool _isMoverEditMode;
    
    
    private bool _isMoverDelAllow;




    public bool IsMoverEditMode{set{this._isMoverEditMode = value;}}

    public bool IsMoverDelAllow{set{this._isMoverDelAllow = value;}}


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        {
            LeftClickFunction();
        }
        //Debug.Log(getEditMover);
        
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

        if (_isMoverEditMode == false & getEditMover != null)
        {
            getEditMover.IsMoveAllow = false;
            getEditMover = null;
        }

    }




    private void LeftClickFunction()
    {

        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入

        if (Physics.Raycast(ray, out hit))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
        {

            if (hit.collider.gameObject.name!="Plane")
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

            }

        }
    }


}
