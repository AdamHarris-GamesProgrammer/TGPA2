
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;

    [SerializeField] protected bool _canBeHarmed = true;

    public bool CanBeHarmed {  get { return _canBeHarmed; } set { _canBeHarmed = value; } }


    protected bool _isDead = false;

    public UnityEvent _OnDie;

    private void Awake()
    {
    }

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
        Debug.Log("Healing by: " + amount);
        //Stops the health from going above maximum.
        _currentHealth = Mathf.Min(_currentHealth += amount, _maxHealth);
        OnHeal();
    }

    protected virtual void OnHeal()
    {

    }

    void Start()
    {
        _currentHealth = _maxHealth;
        _OnDie.AddListener(OnDeath);
        
        OnStart();
    }


    public virtual void TakeDamage(DamageType type, float amount)
    {
        if (_isDead) return;
        if (!_canBeHarmed) return;

        //Take away the left over damage and get the minimum from damage or 0 and set health to this
        _currentHealth = Mathf.Max(_currentHealth -= amount, 0f);

        if (_currentHealth == 0.0f)
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
