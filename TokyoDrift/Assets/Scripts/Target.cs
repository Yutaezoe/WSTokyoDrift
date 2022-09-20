using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Target : MonoBehaviour
{
    [SerializeField]
    private int targetUid = 0 ;
    [SerializeField]
    private bool goalPoint;
    [SerializeField]
    private bool pickPoint;

    public enum TargetStatus
    {
        ACTIVE,
        TEMPORARY,
        COMPLETED
    }

    private TargetStatus statusOfPikking;


    public int TargetUid{ get{return targetUid;}}
    public TargetStatus StatusOfPikking { get { return statusOfPikking; } }
    public bool GoalPoint { get { return goalPoint; } }
    public bool PickPoint { get { return pickPoint; } }


    public void SetStatusOfPikkingACTIVE()
    {
        statusOfPikking = TargetStatus.ACTIVE;
    }
    public void SetStatusOfPikkingCOMPLETED()
    {
        statusOfPikking = TargetStatus.COMPLETED;
    }
    public void SetStatusOfPikkingTEMPORARY()
    {
        statusOfPikking = TargetStatus.TEMPORARY;
    }

    // collision detection
    void OnTriggerEnter(Collider collider)
    {
        // Regular expressions
        bool result = Regex.IsMatch(collider.gameObject.name, "Bus*");
        if (result)
        {
            Debug.Log(collider.gameObject.name);
            //Destroy(gameObject);
        }
        
    }



}
