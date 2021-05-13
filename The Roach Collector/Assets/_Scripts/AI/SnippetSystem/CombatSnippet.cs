using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CombatSnippet
{
    void Initialize(AIAgent agent);

    int Evaluate();

    void Action();

    bool IsFinished();

    void EnterSnippet();
}
