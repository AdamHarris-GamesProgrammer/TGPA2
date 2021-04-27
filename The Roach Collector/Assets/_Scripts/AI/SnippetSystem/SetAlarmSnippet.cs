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
    AIAgent[] _enemiesInScene;

    float _enemyUsableRange = 10.0f;
    float _alarmUsableRange = 15.0f;

    NavMeshAgent _agent;

    bool _isTrying = false;

    bool _isFinished = false;

    AlarmController _currentAlarm;

    public void Action(AIAgent agent)
    {
        if (_alarmsInScene[0].IsSet) _isFinished = true;

        if(_isTrying)
        {
            _agent.SetDestination(_currentAlarm.ActivationPoint.position);

            if (_agent.remainingDistance <= 1.5f)
            {
                if (_currentAlarm.IsDisabled)
                {
                    _isTrying = false;
                    //Removes the front of the element
                    _alarmsInUsableRange.RemoveAt(0);
                }
                else
                {
                    //TODO: Trigger a animation here

                    if (agent.CanActivateAlarm)
                    {
                        if (_currentAlarm.ActivateAlarm())
                        {
                            _isFinished = true;
                        }
                        else
                        {
                            _isTrying = false;
                        }
                    }
                }
            }
        }
        else
        {
            if(_alarmsInUsableRange.Count == 0)
            {
                _isFinished = true;
            }
            else
            {
                Debug.Log("Alarms: ");
                foreach(AlarmController alarm in _alarmsInUsableRange)
                {
                    Debug.Log("Alarm Name: " + alarm.transform.name);
                }

                _currentAlarm = _alarmsInUsableRange[0];
                _isTrying = true;
            }
        }
    }

    public void EnterSnippet()
    {
        //Debug.Log("Alarm Snippet");

        _isFinished = false;
        _isTrying = false;

        _agent.stoppingDistance = 1.0f;
    }

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        if (_alarmsInScene[0].IsSet) return 0;

        TestAlarmsInUsableRange(agent);
        TestEnemiesInUsableRange(agent);

        //No alarms in usable range
        if (_alarmsInUsableRange.Count == 0) return 0;

        if (_enemiesInUsableRange.Count >= 3) returnScore = 50;
        else returnScore = 70;


        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _alarmsInScene = GameObject.FindObjectsOfType<AlarmController>();
        _alarmsInUsableRange = new List<AlarmController>();

        _enemiesInScene = GameObject.FindObjectsOfType<AIAgent>();
        _enemiesInUsableRange = new List<AIAgent>();

        _closestAlarm = null;
        float closestDistance = 10000.0f;
        //Find the closest alarm
        foreach(AlarmController alarm in _alarmsInScene)
        {
            float distance = Vector3.Distance(agent.transform.position, alarm.transform.position);

            //We only care about alarms that are in the enemies usable area
            if (distance < _alarmUsableRange)
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
                Debug.Log("Closest alarm is set");
            }
        }

        _agent = agent.GetComponent<NavMeshAgent>();
    }

    void TestAlarmsInUsableRange(AIAgent agent)
    {
        _alarmsInUsableRange.Clear();

        foreach(AlarmController alarm in _alarmsInScene)
        {
            //Finds the alarms which are in the AIs range
            if(Vector3.Distance(agent.transform.position, alarm.transform.position) < _alarmUsableRange)
            {
                _alarmsInUsableRange.Add(alarm);
            }
        }
    }

    void TestEnemiesInUsableRange(AIAgent agent)
    {
        _enemiesInUsableRange.Clear();

        //Cycle through each enemy in the scene and see if any are in a usable range. 
        foreach(AIAgent enemy in _enemiesInScene)
        {
            //Any dead enemies won't get added
            if (enemy.GetHealth().IsDead()) continue;

            if(Vector3.Distance(agent.transform.position, enemy.transform.position) < _enemyUsableRange) {
                //Add any enemies that are in usable range to this list
                _enemiesInUsableRange.Add(enemy);
            }
        }

    }

    public bool IsFinished()
    {
        return _isFinished;
    }
}
