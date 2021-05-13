using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AlarmController>())
        {
            GetComponentInParent<AIAgent>().CanActivateAlarm = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AlarmController>())
        {
            GetComponentInParent<AIAgent>().CanActivateAlarm = false;
        }
    }
}
