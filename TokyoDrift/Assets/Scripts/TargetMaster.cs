
using UnityEngine;

public class TargetMaster : MonoBehaviour
{
    [SerializeField]
    private Vector3[] setTargetPosition;
    [SerializeField]
    private GameObject TargetGO;


    // Start is called before the first frame update
    void Start()
    {
        int cnt = 0;

        foreach (Vector3 setVector3 in setTargetPosition)
        {
            Instantiate(TargetGO, setTargetPosition[cnt], Quaternion.identity, gameObject.transform);
            cnt++;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {

        if (setTargetPosition==null)
        {
            return;
        }

        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(0, 0, 1, 0.5f);

        int cnt = 0;

        foreach(Vector3 setVector3 in setTargetPosition)
        {
            Gizmos.DrawCube(setTargetPosition[cnt], new Vector3(0.5f, 0.5f, 0.5f));
            cnt++;
        }
        
    }
#endif

}
