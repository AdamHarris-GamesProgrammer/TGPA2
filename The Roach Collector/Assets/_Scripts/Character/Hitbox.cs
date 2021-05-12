using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Health _health;

    [SerializeField] private float _damageMultiplier = 1.0f;


    // Start is called before the first frame update
    void Awake()
    {
        _health = GetComponentInParent<Health>();
    }

    public void OnRaycastHit(RaycastWeapon weapon, Vector3 rayDirection)
    {
        float totalDamage = weapon.Damage * _damageMultiplier;
        //Debug.Log(_health.gameObject.name + " is taking damage");
        //Debug.Log(transform.name + " is taking damage " + weapon.GetDamage() + " multiplied by: " + _damageMultiplier + " giving a total of: " + totalDamage);
        _health.TakeDamage(weapon.DamageType, totalDamage);
    }
}
