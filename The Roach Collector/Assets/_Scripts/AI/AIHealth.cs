using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health
{
    AIAgent _aiAgent;
    UIHealthBar _healthBar;

    protected override void OnStart()
    {
        _aiAgent = GetComponent<AIAgent>();
        _healthBar = GetComponentInChildren<UIHealthBar>();
    }

    protected override void OnDeath()
    {
        FindObjectOfType<LevelStats>().SendMessage("AIDead", 250);
        _aiAgent?.stateMachine.ChangeState(AiStateId.Death);
    }

    protected override void OnDamage()
    {
        _healthBar.SetHealthBarPercentage(_currentHealth / _maxHealth);

        //Aggrevate Enemies on hit.
        _aiAgent.Aggrevate();
    }
}
