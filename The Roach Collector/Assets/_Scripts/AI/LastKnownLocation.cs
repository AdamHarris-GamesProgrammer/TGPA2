﻿using System.Collections;
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


    public Vector3 GeneratePointInRange(float distance)
    {
        Vector3 dist = transform.position;

        bool successful = false;
        
        //Debug.Log("check player state");

        //Brute force a position in range of the player.
        do
        {
            Vector2 point = Random.insideUnitCircle;
            float x = point.x;
            float y = point.y;

            x *= 2.0f;
            y *= 2.0f;

            x -= 1.0f;
            y -= 1.0f;

            point.x = x;
            point.y = y;

            //Debug.Log("Point: " + point);

            Vector3 destination = new Vector3(point.x * _playerRadius / 2, transform.position.y, point.y * _playerRadius / 2);

            //Debug.Log("Destination: " + destination);

            dist = transform.position + destination;

            //Debug.Log("Final Destination: " + finalDestination);

            successful = Physics.Raycast(dist, transform.position - dist);

            //Debug.Log("Successful: " + successful);

        } while (!successful);

        return dist;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}