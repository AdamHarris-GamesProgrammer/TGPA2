using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : AIHealth
{
    [SerializeField] protected float[] _healthStages;
    protected int _index = 0;

    UIHealthBar _healthBar;


    void Awake()
    {
        _healthBar = GetComponentInChildren<UIHealthBar>();
    }

    protected override void OnDamage()
    {
        _healthBar.SetHealthBarPercentage(_currentHealth / _maxHealth);

        _aiAgent.Aggrevate();
        
        if (_index == _healthStages.Length) return;
        if(HealthRatio <= _healthStages[_index])
        {
            _index++;
            NextStage();
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        _healthBar.gameObject.SetActive(false);
    }

    protected virtual void NextStage()
    {
        //Debug.Log("Progressed to stage: " + _index);

        //Example logic
        //TankHealth.cs
        //override NextStage
        //if _index == 1
            //Disable cover state
            //switch to different animation logic?
        //if _index == 2
            //Enter charge mode

    }
}
