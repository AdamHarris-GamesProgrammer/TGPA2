using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAlarmSnippet : CombatSnippet
{
    public void Action(AIAgent agent)
    {
    }

    public void EnterSnippet()
    {

    }

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        //TODO Implement an alarm 


        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
    }

    public bool IsFinished()
    {
        //Implement finish logic
        return true;
    }
}
