using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CombatSnippet
{
    public void Initialize(AIAgent agent);

    public int Evaluate(AIAgent agent);

    public void Action(AIAgent agent);

    public bool IsFinished();
}
