using Harris.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem
{
    protected GameObject _user;

    protected UsableID _id;

    public UsableID GetID()
    {
        return _id;
    }

    public virtual void Update(float deltaTime)
    {
        
    }

    public virtual float GetApplyTimeRemaining()
    {
        return 0.0f;
    }
}
