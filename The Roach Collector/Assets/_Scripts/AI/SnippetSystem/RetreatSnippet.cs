using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatSnippet : CombatSnippet
{
    AIHealth _aiHealth;

    public void Action(AIAgent agent)
    {

    }

    public void EnterSnippet()
    {

    }

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        bool shouldRetreat = (UnityEngine.Random.Range(0.0f, 1.0f) < agent._config._retreatChance);

        //We will retreat if our health ratio is less than 0.1f and we pass the retreat check
        if (_aiHealth.GetHealthRatio() < 0.1f && shouldRetreat)
        {
            //Until I figure out how to build a retreat the ai will just never succeed the retreat check
            //returnScore = 110;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _aiHealth = agent.GetComponent<AIHealth>();
    }

    public bool IsFinished()
    {
        //TODO: Implement finish logic
        return true;
    }
}
