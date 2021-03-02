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

    private void OnDie()
    {
        if (_isDead) return;

        //Disables the collider
        Collider col = GetComponent<Collider>();
        col.enabled = false;

        //Stops the rigid body from moving
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

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

        OnHealthChanged.Invoke();

        if(_health <= 0.0f)
        {
            OnDeath.Invoke();
        }
    }
}
