using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Health _health;


    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponentInParent<Health>();
    }

    public void OnRaycastHit(RaycastWeapon weapon, Vector3 rayDirection)
    {
        _health.TakeDamage(weapon._weaponConfig.Damage);
    }
}
