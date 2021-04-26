using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;

    Ragdoll _ragDoll;
    

    private bool _isDead = false;

    public UnityEvent _OnDie;

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetHealthRatio()
    {
        return _currentHealth / _maxHealth;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    void Start()
    {
        
        _currentHealth = _maxHealth;
        _ragDoll = GetComponent<Ragdoll>();
        _OnDie.AddListener(OnDeath);
        
        OnStart();
    }


    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;

        if (_currentHealth <= 0.0f)
        {
            _isDead = true;
            _OnDie.Invoke();
        }

        OnDamage();
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnDeath()
    {

    }

    protected virtual void OnDamage()
    {

    }

}
