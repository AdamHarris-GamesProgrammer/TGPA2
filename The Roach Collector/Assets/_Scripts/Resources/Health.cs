using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Min(0f)][SerializeField] private float _health = 100.0f;

    private bool _isDead = false;

    public UnityEvent OnHealthChanged;
    public UnityEvent OnDeath;

    public bool IsDead()
    {
        return _isDead;
    }

    public float GetHealth()
    {
        return _health;
    }

    void TakeDamage(float damageIn)
    {
        _health -= damageIn;

        OnHealthChanged.Invoke();

        if(_health <= 0.0f)
        {
            _isDead = true;
            OnDeath.Invoke();
        }
    }
}
