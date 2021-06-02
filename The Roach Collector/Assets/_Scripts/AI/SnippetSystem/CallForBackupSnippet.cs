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
        //Aggravates all enemies in the combat zone
        _alliesInZone.ForEach(ally => ally.Aggrevate());
        _hasBackup = true;
    }

    public void EnterSnippet()
    {
        _agent.PlayBackupSound();
    }

    public int Evaluate()
    {
        if (_hasBackup) return 0;

        if (_agent.Zone.GetAliveEnemies().Count >= 3) return 80;

        return 0;
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
