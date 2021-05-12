using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSnippet : CombatSnippet
{
    AIWeapons _aiWeapons;
    AIAgent _agent;

    public void Action()
    {
        //Debug.Log(_agent.transform.name + " is reloading");
        _aiWeapons.GetEquippedWeapon().Reload();
    }

    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Reload Snippet");
    }

    public int Evaluate()
    {
        int returnScore = 0;


        if (_aiWeapons.GetEquippedWeapon().NeedToReload)
        {
            returnScore = 100;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _aiWeapons = agent.GetComponent<AIWeapons>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //If we no longer need to reload.
        return !_aiWeapons.GetEquippedWeapon().NeedToReload;
    }
}
