using System.Collections;
using System.Collections.Generic;
using TGP.Resources.UI;
using UnityEngine;
using UnityEngine.Events;

namespace TGP.Resources
{
    public class Health : MonoBehaviour
    {
        [Min(0f)] [SerializeField] private float _maxHealth = 100.0f;

        private HealthUI _healthUI;

        private float _currentHealth;

        private bool _isDead = false;

        public UnityEvent OnHealthChanged;
        public UnityEvent OnDeath;

        private void Awake()
        {
            OnDeath.AddListener(OnDie);

            _currentHealth = _maxHealth;

            _healthUI = GetComponent<HealthUI>();

            if(_healthUI != null)
            {
                _healthUI.UpdateUI(_currentHealth, _maxHealth);
            }
        }

        private void OnDie()
        {
            if (_isDead) return;

            if (!gameObject.CompareTag("Player"))
            {
                //Disables the collider
                Collider col = GetComponent<Collider>();
                col.enabled = false;
            }

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
            return _currentHealth;
        }

        public void TakeDamage(float damageIn)
        {
            if (_isDead) return;


            _currentHealth = Mathf.Clamp(_currentHealth - damageIn, 0, _maxHealth);

            OnHealthChanged.Invoke();

            

            if (_currentHealth == 0.0f)
            {
                OnDeath.Invoke();
            }

            if (_healthUI != null)
            {
                _healthUI.UpdateUI(_currentHealth, _maxHealth);
            }

        }
    }
}

