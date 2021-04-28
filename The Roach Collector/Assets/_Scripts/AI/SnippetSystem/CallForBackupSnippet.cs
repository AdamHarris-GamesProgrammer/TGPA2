using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForBackupSnippet : CombatSnippet
{
    GameObject[] _enemiesInScene;
    List<AIAgent> _enemiesInRange;
    AIAgent _agent;

    bool _hasBackup = false;

    public void Action()
    {
        //_enemiesInRange.Clear();


        Debug.Log("Backup Action");
        foreach(AIAgent enemy in _enemiesInRange)
        {
            //Debug.Log(agent.transform.name + " is aggravating: " + enemy.transform.name);
            enemy.Aggrevate();
            
        }
        _hasBackup = true;
    }

    public void EnterSnippet()
    {
        _agent.PlayBackupSound();
    }

    public int Evaluate()
    {
        if (_hasBackup) return 0;

        int returnScore = 0;

        foreach (GameObject enemy in _enemiesInScene)
        {
            if (Vector3.Distance(_agent.transform.position, enemy.transform.position) < _agent._config._backupEnemyDistance)
            {
                //Check if this enemy is dead or not 
                if (enemy.GetComponent<AIAgent>().GetHealth().IsDead() || enemy.GetComponent<AIAgent>().Aggrevated) continue;

                //Debug.Log(agent.transform.name + " is adding enemy to range list");
                _enemiesInRange.Add(enemy.GetComponent<AIAgent>());
            }
        }

        if (_enemiesInRange.Count > 3)
        {
            return 80;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        _enemiesInRange = new List<AIAgent>();
    }

    public bool IsFinished()
    {
        return _hasBackup;
    }
}
