using Harris.Inventories;
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

    Equipment _equipment;


    private bool _isDead = false;

    public UnityEvent _OnDie;

    private void Awake()
    {
        _equipment = GetComponent<Equipment>();
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


    public void TakeDamage(float amount)
    {
        //Stops the Character from taking damage if they don't need to.
        if (!_canBeHarmed) return;
        if (_isDead) return;


        //Armor can block 90% of incoming damage. 10% of damage will always come through
        //The remaining 90% of damage is then blocked based on how much armor the player has
        //Armor is in a range of 0 to 100

        //Calculates Armor Protection
        int armor = 0;

        if (_equipment != null)
        {
            armor = _equipment.GetTotalArmor();
        }

        //Gets 10% of the damage
        float damageToGoThrough = amount / 10.0f;

        //Takes away 10% of damage from the amount
        float leftOverDamage = amount - damageToGoThrough;

        //Stores how much percent the armor should block
        float armorBlocks = 0.0f;
        if (armor > 0)
        {
            //Calculates the percentage of damage blocked
            armorBlocks = (leftOverDamage / 100) * armor;
        }

        //Left over damage is now updated to use the armor and the damage to go through
        leftOverDamage = leftOverDamage - armorBlocks + damageToGoThrough;

        //Take away the left over damage and get the minimum from damage or 0 and set health to this
        _currentHealth = Mathf.Max(_currentHealth -= leftOverDamage, 0f);

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
