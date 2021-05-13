using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    AlarmController[] _alarmsInScene;

    bool _isDisabled = false;
    bool _isActivated = false;

    [SerializeField] Material _disabledMaterial = null;
    [SerializeField] Material _standbyMaterial = null;
    [SerializeField] Material _activatedMaterial = null;

    Transform _activationPoint;

    public Transform ActivationPoint { get { return _activationPoint;} }

    static bool _isSet = false;
    public bool IsSet { get { return _isSet; } }

    public bool IsDisabled { get { return _isDisabled; } set { _isDisabled = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        _alarmsInScene = FindObjectsOfType<AlarmController>();
        _activationPoint = transform.GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        //send signal that enemy can enable the alarm
        if (other.CompareTag("Player"))
        {
            if (_isDisabled) return;
            other.SendMessage("DisplayAlarm", true);
            other.GetComponent<PlayerController>().Alarm = this;
            other.GetComponent<PlayerController>().CanDisableAlarm = true;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            EnableAlarm();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.SendMessage("DisplayAlarm", false);
            other.GetComponent<PlayerController>().Alarm = null;
            other.GetComponent<PlayerController>().CanDisableAlarm = false;
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
    }

    public bool ActivateAlarm()
    {
        //If the alarm is not disabled, then enable the alarm
        if(!_isDisabled)
        {
            ActivationBehaviour();
            return true;
        }
        return false;
    }

    private void ActivationBehaviour()
    {
        _isActivated = true;
        GetComponent<MeshRenderer>().material = _activatedMaterial;

        foreach(AIAgent enemy in GameObject.FindObjectsOfType<AIAgent>())
        {
            enemy.Aggrevate();
        }

        _isSet = true;
    }
}
