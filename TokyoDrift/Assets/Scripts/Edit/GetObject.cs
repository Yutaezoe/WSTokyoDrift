using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObject : MonoBehaviour
{
    [SerializeField]
    public Camera cameraObject; //�J�������擾



    private RaycastHit hit; //���C�L���X�g�������������̂��擾������ꕨ
    private GameObject hitObject;
    private EditMover getEditMover;

    private bool _isMoverEditMode;
    
    
    private bool _isMoverDelAllow;




    public bool IsMoverEditMode{set{this._isMoverEditMode = value;}}

    public bool IsMoverDelAllow{set{this._isMoverDelAllow = value;}}


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //�}�E�X���N���b�N���ꂽ��
        {
            LeftClickFunction();
        }
        // When the mover is selected and the HUD button is pressed.
        if (getEditMover != null) //Mover was clicked
        {
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

        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition); //�}�E�X�̃|�W�V�������擾����Ray�ɑ��

        if (Physics.Raycast(ray, out hit))  //�}�E�X�̃|�W�V��������Ray�𓊂��ĉ����ɓ���������hit�ɓ����
        {


            // �ڕW���ȊO���N���b�N�����ꍇ�G���[�X���[
            try
            {

                if (getEditMover == null) // �R���|�[�l���g�����݂��邩
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
