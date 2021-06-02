using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStabCheck : MonoBehaviour
{
    [SerializeField] private bool _isStabbing = false;

    public void SetStabbing(bool stabbing)
    {
        _isStabbing = stabbing;
    }

    public bool GetStabbing()
    {
        Debug.Log("Stabbing: " + _isStabbing);
        return _isStabbing;
    }
}
