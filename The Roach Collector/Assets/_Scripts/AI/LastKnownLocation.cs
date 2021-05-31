using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGP.Control;
using UnityEngine;

public class LastKnownLocation : MonoBehaviour
{
    //Holds all the AI Agents in the scene
    List<AIAgent> _agentsInScene;


    private void Awake()
    {
        //Finds all the AI Agents in the scene
        _agentsInScene = FindObjectsOfType<AIAgent>().ToList();
    }

    public List<AIAgent> GetEnemiesInRange(float distance)
    {
        //Creates a new list of AIAgents
        List<AIAgent> agentsInDistance = new List<AIAgent>();

        //Cycle through all agents
        foreach (AIAgent enemy in _agentsInScene)
        {
            //Don't add the enemy if the there dead
            if (enemy.Health.IsDead) continue;

            //Check if the enemy is within distance and add them to the list if they are
            if (Vector3.Distance(transform.position, enemy.transform.position) < distance) agentsInDistance.Add(enemy);
        }

        //Return the list of agents that are within distance
        return agentsInDistance;
    }


    public Vector3 GeneratePointInRange(float distance)
    {
        Vector3 sampledPosition = transform.position;

        bool successful = false;

        int iterations = 0;

        //Brute force a position in range of the player.
        do
        {
            //Generate a point in range 0 to 1
            Vector2 point = Random.insideUnitCircle;
            //Change it to 0 to 2
            point *= 2.0f;
            float x = point.x;
            float y = point.y;

            //Take 1 away now in range -1 to 1
            x -= 1.0f;
            y -= 1.0f;

            //Set the point back
            point.Set(x, y);

            //Multiply the points by the distance parameter to get a point on edge of the circle
            Vector3 destination = new Vector3(point.x * distance / 2, transform.position.y, point.y * distance / 2);

            //Add the destination to the last known location position
            sampledPosition = transform.position + destination;

            //Check a raycast
            successful = Physics.Raycast(sampledPosition, transform.position - sampledPosition);

            //Continue this until we are successful or our iterations are equal to or over 5
        } while (!successful && iterations < 5);

        return sampledPosition;
    }

    public Vector3 GeneratePointInRangeWithRaycast(float distance)
    {
        Vector3 sampledPosition = transform.position;

        bool successful = false;

        int iterations = 0;

        //Brute force a position in range of the player.
        do
        {
            //Generate a point in range 0 to 1
            Vector2 point = Random.insideUnitCircle;
            //Change it to 0 to 2
            point *= 2.0f;
            float x = point.x;
            float y = point.y;

            //Take 1 away now in range -1 to 1
            x -= 1.0f;
            y -= 1.0f;

            //Set the point back
            point.Set(x, y);

            //Multiply the points by the distance parameter to get a point on edge of the circle
            Vector3 destination = new Vector3(point.x * distance / 2, transform.position.y, point.y * distance / 2);

            //Add the destination to the last known location position
            sampledPosition = transform.position + destination;

            //Generate a raycast from this position to this object 
            RaycastHit hit;
            if (Physics.Raycast(sampledPosition, transform.position - sampledPosition, out hit, distance, ~0, QueryTriggerInteraction.Ignore))
            {
                //Check if we have hit the player, meaning the AI will be able to see the player from this point
                if (hit.transform.CompareTag("Player"))
                {
                    successful = true;
                }
            }

            iterations++;

        } while (!successful && iterations < 20);

        return sampledPosition;
    }
}
