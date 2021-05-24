using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSnippet : CombatSnippet
{
    AIWeapons _aiWeapon;
    AIAgent _agent;

    public void Action()
    {
        //Debug.Log(_agent.transform.name + " is reloading");
        if (!_aiWeapon.GetEquippedWeapon().IsReloading)
        {
            _aiWeapon.GetEquippedWeapon().Reload();
        }
    }

    public void EnterSnippet()
    {
        Debug.Log(_agent.transform.name + " Reload Snippet");
        _aiWeapon.SetFiring(false);
    }

    public int Evaluate()
    {
        int returnScore = 0;


        if (_aiWeapon.GetEquippedWeapon().NeedToReload)
        {
            returnScore = 100;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _aiWeapon = agent.GetComponent<AIWeapons>();
        _agent = agent;
    }

    public bool IsFinished()
    {
        //If we no longer need to reload.
        return !_aiWeapon.GetEquippedWeapon().NeedToReload;
    }

}
