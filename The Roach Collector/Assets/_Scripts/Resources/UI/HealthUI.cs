using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TGP.Resources.UI
{
    [RequireComponent(typeof(Health))]
    public class HealthUI : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private Text _healthText;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.OnHealthChanged.AddListener(UpdateUI);

            
        }

        private void Start()
        {
            //Sets start values
            UpdateUI();
        }

        public void UpdateUI()
        {
            _healthText.text = _health.GetCurrentHealth() + "/" + _health.GetMaxHealth();
        }

    }
}

