using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CombatSnippet
{
    /// <summary>
    /// Called When creating a snippet
    /// </summary>
    /// <param name="agent">The owning agent</param>
    void Initialize(AIAgent agent);

    /// <summary>
    /// Evaluates the score of this action
    /// </summary>
    /// <returns>Returns the score of the snippet, the higher it is the more viable it is.</returns>
    int Evaluate();

    /// <summary>
    /// The action of the snippet
    /// </summary>
    void Action();

    /// <summary>
    /// Used to check if the snippet has finished executing
    /// </summary>
    /// <returns></returns>
    bool IsFinished();

    /// <summary>
    /// Called when changing snippets
    /// </summary>
    void EnterSnippet();
}
