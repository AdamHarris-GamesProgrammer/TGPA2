using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    List<AIAgent> _agentInZone;
    List<CoverController> _coversInZone;

    public List<AIAgent> AgentsInZone { get { return _agentInZone; } }
    public List<CoverController> CoversInZone { get { return _coversInZone; } }

    private void Awake()
    {
        _agentInZone = new List<AIAgent>();
        _coversInZone = new List<CoverController>();

        _agentInZone = GetComponentsInChildren<AIAgent>().ToList<AIAgent>();
        _coversInZone = GetComponentsInChildren<CoverController>().ToList<CoverController>();
    }

    public List<AIAgent> GetAliveEnemies()
    {
        List<AIAgent> aliveEnemies = new List<AIAgent>();

        foreach (AIAgent enemy in _agentInZone)
        {
            //Don't add the enemy if the there dead
            if (enemy.GetHealth.IsDead) continue;

            aliveEnemies.Add(enemy);
        }

        return aliveEnemies;
    }
}
