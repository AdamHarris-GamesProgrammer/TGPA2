using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetAlarmSnippet : CombatSnippet
{
    AlarmController[] _alarmsInScene;
    List<AlarmController> _alarmsInUsableRange;
    AlarmController _closestAlarm;

    List<AIAgent> _enemiesInUsableRange;

    NavMeshAgent _navAgent;

    bool _isTrying = false;

    bool _isFinished = false;

    bool _alarmsLeft = true;

    AlarmController _currentAlarm;

    AIAgent _agent;

    public void Action()
    {
        if (_alarmsInScene[0].IsSet) _isFinished = true;

        if(_isTrying)
        {
            _navAgent.SetDestination(_currentAlarm.ActivationPoint.position);

            if (_navAgent.remainingDistance <= 1.5f)
            {
                _isTrying = false;
                if (_currentAlarm.IsDisabled)
                {
                    //Removes the front of the element
                    Debug.Log("Alarms in usable range before deletion: ");
                    foreach(AlarmController alarm in _alarmsInUsableRange)
                    {
                        Debug.Log("Alarm Name: " + alarm.gameObject.name);
                    }

                    _alarmsInUsableRange.RemoveAt(0);

                    Debug.Log("Alarms in usable range after deletion: ");
                    foreach (AlarmController alarm in _alarmsInUsableRange)
                    {
                        Debug.Log("Alarm Name: " + alarm.gameObject.name);
                    }
                }
                else
                {
                    //TODO: Trigger a animation here

                    if (_agent.CanActivateAlarm)
                    {
                        if (_currentAlarm.ActivateAlarm())
                        {
                            _isFinished = true;
                        }
                    }
                }
            }
        }
        else
        {
            if(_alarmsInUsableRange.Count == 0)
            {
                //_isFinished = true;
                _alarmsLeft = false;
            }
            else
            {
                //Debug.Log("Alarms: ");
                foreach(AlarmController alarm in _alarmsInUsableRange)
                {
                    //Debug.Log("Alarm Name: " + alarm.transform.name);
                }

                _currentAlarm = _alarmsInUsableRange[0];
                _isTrying = true;
            }
        }
    }

    public void EnterSnippet()
    {
        _agent.PlayAlarmPrompt();

        //Debug.Log("Alarm Snippet");

        _isFinished = false;
        _isTrying = false;

        _navAgent.stoppingDistance = 1.0f;
    }

    public int Evaluate()
    {
        int returnScore = 0;


        if (_alarmsInScene.Length == 0) return 0;
        if (_alarmsInScene[0].IsSet) return 0;
        if (!_alarmsLeft) return 0;

        _alarmsInUsableRange = _agent.GetAlarmsInRange(_agent._config._alarmUsableRange);
        _enemiesInUsableRange = _agent.GetEnemiesInRange(_agent._config._alarmUsableEnemyRange);

        //No alarms in usable range
        if (_alarmsInUsableRange.Count == 0) return 0;

        if (_enemiesInUsableRange.Count >= 3) returnScore = 50;
        else returnScore = 70;


        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _agent = agent;

        _alarmsInScene = GameObject.FindObjectsOfType<AlarmController>();
        _alarmsInUsableRange = new List<AlarmController>();

        _enemiesInUsableRange = new List<AIAgent>();

        _closestAlarm = null;
        float closestDistance = 10000.0f;
        //Find the closest alarm
        foreach(AlarmController alarm in _alarmsInScene)
        {
            float distance = Vector3.Distance(agent.transform.position, alarm.transform.position);

            //We only care about alarms that are in the enemies usable area
            if (distance < agent._config._alarmUsableRange)
            {
                _alarmsInUsableRange.Add(alarm);
            }
            else
            {
                continue;
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
                _closestAlarm = alarm;
            }
        }

        _navAgent = agent.GetComponent<NavMeshAgent>();
    }

    public bool IsFinished()
    {
        return !_alarmsLeft;
    }
}
