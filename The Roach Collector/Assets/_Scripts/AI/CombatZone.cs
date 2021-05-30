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
        //Initialize the lists
        _agentInZone = new List<AIAgent>();
        _coversInZone = new List<CoverController>();

        //Fill the lists
        _agentInZone = GetComponentsInChildren<AIAgent>().ToList();
        _coversInZone = GetComponentsInChildren<CoverController>().ToList();
    }

    public List<AIAgent> GetAliveEnemies()
    {
        //Creates a new list containing only the alive enemies
        List<AIAgent> aliveEnemies = new List<AIAgent>();

        foreach (AIAgent enemy in _agentInZone)
        {
            //Don't add the enemy if the there dead
            if (enemy.Health.IsDead) continue;

            //Add the alive enemy to the list
            aliveEnemies.Add(enemy);
        }
        //return the alive enemies
        return aliveEnemies;
    }
}
