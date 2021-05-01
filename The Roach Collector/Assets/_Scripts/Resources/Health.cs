using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;

    [SerializeField] bool _canBeHarmed = true;

    public bool CanBeHarmed {  get { return _canBeHarmed; } set { _canBeHarmed = value; } }

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

    public void Heal(float amount) {
        //Stops the health from going above maximum.
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0.0f, _maxHealth);
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
        //Stops the Character from taking damage if they don't need to.
        if (!_canBeHarmed) return;
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
