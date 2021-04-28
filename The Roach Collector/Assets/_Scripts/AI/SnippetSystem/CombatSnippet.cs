using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CombatSnippet
{
    public void Initialize(AIAgent agent);

    public int Evaluate();

    public void Action();

    public bool IsFinished();

    public void EnterSnippet();
}
