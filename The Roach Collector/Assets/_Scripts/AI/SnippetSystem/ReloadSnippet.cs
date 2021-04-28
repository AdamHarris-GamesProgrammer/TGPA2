using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSnippet : CombatSnippet
{
    public void Action(AIAgent agent)
    {

    }

    public void EnterSnippet(AIAgent agent)
    {
        Debug.Log("Reload Snippet");
    }

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        //Implement the concept of ammo usage


        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {

    }

    public bool IsFinished()
    {
        //TODO: Implement finish logic
        return true;
    }
}
