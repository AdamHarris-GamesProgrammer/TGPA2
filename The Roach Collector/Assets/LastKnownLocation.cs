using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGP.Control;
using UnityEngine;

public class LastKnownLocation : MonoBehaviour
{
    PlayerController _player;

    List<AIAgent> _agentsInScene;

    [SerializeField] float _playerRadius = 7.5f;
    public float RadiusAroundPlayer { get { return _playerRadius; } }


    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _agentsInScene = GameObject.FindObjectsOfType<AIAgent>().ToList<AIAgent>();
    }

    public List<AIAgent> GetEnemiesInRange(float distance, bool includeDead = false)
    {
        List<AIAgent> agentsInDistance = new List<AIAgent>();

        if (includeDead)
        {
            foreach (AIAgent enemy in _agentsInScene)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                {
                    agentsInDistance.Add(enemy);
                }
            }
        }
        else
        {
            foreach (AIAgent enemy in _agentsInScene)
            {
                //Don't add the enemy if the there dead
                if (enemy.GetHealth().IsDead) continue;

                if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                {
                    agentsInDistance.Add(enemy);
                }
            }
        }

        return agentsInDistance;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
