using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField,Header("�������u��")]
    private UnityEvent eventDown;
    [SerializeField, Header("�������u��")]
    private UnityEvent eventUp;
    [SerializeField, Header("������Ă����Ԃœ������u��")]
    private UnityEvent eventEnter;
    [SerializeField, Header("������Ă����Ԃŗ��ꂽ�u��")]
    private UnityEvent eventExit;


    bool isPush = false;
    bool isEnter = false;
    Image image;

    void Start()
    {
        #region test
        image = GetComponent<Image>();

        eventDown.AddListener(() => image.color = new Color(0f, 1f, 0f));
        eventUp.AddListener(() => image.color = new Color(1f, 1f, 1f));
        eventEnter.AddListener(() => image.color = new Color(0f, 1f, 0f));
        eventExit.AddListener(() => image.color = new Color(1f, 1f, 1f));
        #endregion
    }

    void OnEnable()
    {
        isPush = false;
        isEnter = false;
        
    }

    #region event
    public void OnPointerDown(PointerEventData eventData)
    {
        isPush = true;

        if (eventDown != null) eventDown.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if (!isEnter) return;  // �I�u�W�F�N�g���̂ݗL��

        if (eventUp != null) eventUp.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        if (!isPush) return;

        if (eventEnter != null) eventEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        if (!isPush) return;

        if (eventExit != null) eventExit.Invoke();
    }
    #endregion
}