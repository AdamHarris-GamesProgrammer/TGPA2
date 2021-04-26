using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    List<AlarmController> _alarmsInScene;

    bool _isDisabled = false;
    bool _isActivated = false;

    [SerializeField] Material _disabledMaterial;
    [SerializeField] Material _standbyMaterial;
    [SerializeField] Material _activatedMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //if player
        //send signal that player can disable alarm
        //if enemy
        //if alarm is disabled 
        //make enemy enable it

        //send signal that enemy can enable the alarm

        if (other.CompareTag("Player"))
        {
            if (_isDisabled) return;
            other.SendMessage("DisplayAlarm", true);
            other.GetComponent<PlayerController>().Alarm = this;
            other.GetComponent<PlayerController>().CanDisableAlarm = true;
        }
        else if(other.CompareTag("Enemy"))
        {

        }


    }

    private void OnTriggerExit(Collider other)
    {
        //if player
            //send signal that player can no longer disable alarm
        //if enemy
            //send signal that enemy can no longer enable the alarm

        if(other.CompareTag("Player"))
        {
            other.SendMessage("DisplayAlarm", false);
            other.GetComponent<PlayerController>().Alarm = null;
            other.GetComponent<PlayerController>().CanDisableAlarm = false;
        }
        else if (other.CompareTag("Enemy"))
        {
            
        }
    }

    public void DisableAlarm()
    {
        _isDisabled = true;
        GetComponent<MeshRenderer>().material = _disabledMaterial;
    }

    public void EnableAlarm() 
    {
        if (!_isDisabled)
        {
            ActivationBehaviour();

            //Daisy chain alarms
            foreach (AlarmController alarm in _alarmsInScene)
            {
                alarm.ActivateAlarm();
            }
        }
        //enable alarm
        //change material
        //activate all alarms
    }

    public void ActivateAlarm()
    {
        //If the alarm is not disabled, then enable the alarm
        if(!_isDisabled)
        {
            ActivationBehaviour();
        }
    }

    private void ActivationBehaviour()
    {
        _isActivated = true;
        GetComponent<MeshRenderer>().material = _activatedMaterial;
    }
}
