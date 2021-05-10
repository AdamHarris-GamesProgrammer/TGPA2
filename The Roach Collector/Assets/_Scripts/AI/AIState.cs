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
    SearchForPlayer
}

public interface AIState
{
    AiStateId GetID();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
