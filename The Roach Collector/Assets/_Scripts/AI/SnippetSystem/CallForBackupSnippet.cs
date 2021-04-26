using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallForBackupSnippet : CombatSnippet
{
    GameObject[] _enemiesInScene;
    List<AIAgent> _enemiesInRange;

    public void Action(AIAgent agent)
    {
        _enemiesInRange.Clear();

    }

    public int Evaluate(AIAgent agent)
    {
        int returnScore = 0;

        foreach (GameObject enemy in _enemiesInScene)
        {
            if (Vector3.Distance(agent.transform.position, enemy.transform.position) < 25.0f)
            {
                //Added the enemy
                _enemiesInRange.Add(enemy.GetComponent<AIAgent>());
            }
        }

        if (_enemiesInRange.Count > 3)
        {
            return 30;
        }

        return returnScore;
    }

    public void Initialize(AIAgent agent)
    {
        _enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        _enemiesInRange = new List<AIAgent>();
    }

    public bool IsFinished()
    {
        //TODO: Implement behaviored
        return true;
    }
}
