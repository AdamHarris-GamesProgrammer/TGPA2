using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health
{
    protected AIAgent _aiAgent;

    protected override void OnStart()
    {
        _aiAgent = GetComponent<AIAgent>();
    }

    protected override void OnDeath()
    {
        //Adds score to the level stats
        FindObjectOfType<LevelStats>().SendMessage("AIDead", 250);
        _aiAgent?.stateMachine.ChangeState(AiStateId.Death);
    }

    protected override void OnDamage()
    {
        //Aggrevate Enemies on hit.
        _aiAgent.Aggrevate();
    }
}
