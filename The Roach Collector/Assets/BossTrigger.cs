using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    static bool _aggroed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_aggroed)
            {
                GameObject.FindGameObjectWithTag("Boss").GetComponent<AIAgent>().Aggrevate();
                _aggroed = true;
            }
        }


    }
}
