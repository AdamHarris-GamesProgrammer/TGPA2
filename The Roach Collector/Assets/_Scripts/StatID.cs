using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use this enum to add different types of stats to the player 
public enum StatID
{
    DAMAGE_RESISTANCE,
    DAMAGE_BOOST,
    PROJECTILE_RESISTANCE,
    MELEE_RESISTANCE,
    MOVE_SPEED,
    NONE
}

[System.Serializable]
public struct StatValues
{
    public StatValues(StatID id, float val)
    {
        _id = id;
        _value = val;
    }

    public StatID _id;
    public float _value;
}
