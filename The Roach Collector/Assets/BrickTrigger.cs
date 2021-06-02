using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTrigger : MonoBehaviour
{
    static bool _aggroed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!_aggroed)
        {
            FindObjectOfType<BrickAI>().Aggrevate();
            _aggroed = true;
        }
    }
}
