using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickHealth : BossHealth
{
    [SerializeField] AIAgent[] _secondStageAgents;
    [SerializeField] AIAgent[] _thirdStageAgents;

    protected override void NextStage()
    {
        if(_index == 1)
        {
            foreach(AIAgent agent in _secondStageAgents)
            {
                agent.gameObject.SetActive(true);
                agent.Aggrevate();
            }
        }
        else if (_index == 2)
        {
            foreach (AIAgent agent in _thirdStageAgents)
            {
                agent.gameObject.SetActive(true);
                agent.Aggrevate();
            }
        }
    }

}
