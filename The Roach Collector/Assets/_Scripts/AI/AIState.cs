using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    ChasePlayer,
    Death,
    Idle,
    FindWeapon,
    Patrol,
    CombatState,
    GotToPlayerLocation,
    SearchForPlayer,
    Melee,
    BrickMelee
}

public interface AIState
{
    AiStateId GetID();
    void Enter();
    void Update();
    void Exit();
}
