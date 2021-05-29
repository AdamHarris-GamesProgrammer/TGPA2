using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForBackupSnippet : CombatSnippet
{
    List<AIAgent> _alliesInZone;
    AIAgent _agent;

    bool _hasBackup = false;

    public void Action()
    {
        _alliesInZone.ForEach(ally => ally.Aggrevate());
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

        _alliesInZone = _agent.Zone.GetAliveEnemies();

        if (_alliesInZone.Count >= 3) return 80;

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _agent = agent;
        _alliesInZone = new List<AIAgent>();
    }

    public bool IsFinished()
    {
        return _hasBackup;
    }
}
