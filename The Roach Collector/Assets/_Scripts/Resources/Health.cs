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

    private void Awake()
    {
        OnDeath.AddListener(OnDie);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(10.0f);
        }
    }

    private void OnDie()
    {
        if (_isDead) return;

        _isDead = true;
        GetComponent<Animator>().SetBool("isDead", true);
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public float GetHealth()
    {
        return _health;
    }

    public void TakeDamage(float damageIn)
    {
        if (_isDead) return;

        _health -= damageIn;

        Debug.Log("Health: " + _health);

        OnHealthChanged.Invoke();

        if(_health <= 0.0f)
        {
            OnDeath.Invoke();
        }
    }
}
