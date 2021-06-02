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
            Debug.Log("Second Stage");
            foreach(AIAgent agent in _secondStageAgents)
            {
                agent.gameObject.SetActive(true);
                agent.Aggrevate();
            }
        }
        else if (_index == 2)
        {
            Debug.Log("Third Stage");
            foreach (AIAgent agent in _thirdStageAgents)
            {
                agent.gameObject.SetActive(true);
                agent.Aggrevate();
            }
        }
    }

}
