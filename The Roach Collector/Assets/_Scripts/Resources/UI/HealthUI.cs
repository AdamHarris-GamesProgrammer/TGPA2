using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TGP.Resources.UI
{
    public class HealthUI : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private Text _healthText;

        public void UpdateUI(float min, float max)
        {
            _healthText.text = min + "/" + max;
        }

    }
}

