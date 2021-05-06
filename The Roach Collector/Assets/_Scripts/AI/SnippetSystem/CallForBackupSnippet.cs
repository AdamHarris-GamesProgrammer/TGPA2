using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForBackupSnippet : CombatSnippet
{
    List<AIAgent> _enemiesInRange;
    AIAgent _agent;

    bool _hasBackup = false;

    string Name = "CallForBackupSnippet";
    public void Action()
    {
        
        foreach(AIAgent enemy in _enemiesInRange)
        {
            //Debug.Log(agent.transform.name + " is aggravating: " + enemy.transform.name);
            enemy.Aggrevate();
            
        }
        _hasBackup = true;
    }

    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Backup snippet");
        _agent.PlayBackupSound();
    }

    public int Evaluate()
    {
        if (_hasBackup) return 0;

        int returnScore = 0;

        _enemiesInRange = _agent.Zone.GetAliveEnemies();

        if (_enemiesInRange.Count >= 3)
        {
            return 80;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _agent = agent;
        _enemiesInRange = new List<AIAgent>();
    }

    public bool IsFinished()
    {
        return _hasBackup;
    }
    public string GetName()
    {
        return Name;
    }
}
