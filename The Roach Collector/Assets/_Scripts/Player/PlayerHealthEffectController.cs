using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthEffectController : MonoBehaviour
{
    HealthScreenEffect _healthShader;

    
    [SerializeField] public Image[] _splatterImages;

    private void Awake()
    {
        _healthShader = Camera.main.GetComponent<HealthScreenEffect>();
    }


    public void CalculateEffect(float percentageOfHealth)
    {
        percentageOfHealth = Mathf.Clamp(percentageOfHealth, 0.0f, 1.0f);

        float damagePercentage = 1.0f - percentageOfHealth;

        _healthShader._greyScaleAmount = damagePercentage;

        float increment = 1.0f / _splatterImages.Length;
        int i = 0;
        for(float f = 0; f < 1.0; f += increment)
        {
            
            if (damagePercentage < f) return;

            _splatterImages[i]?.gameObject.SetActive(true);
            i++;
        }

    }

}
