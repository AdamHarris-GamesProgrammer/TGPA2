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
        //Clamps the percentage
        percentageOfHealth = Mathf.Clamp(percentageOfHealth, 0.0f, 1.0f);

        //Cycle through the splatter images and set them to false
        foreach(Image image in _splatterImages)
        {
            image.gameObject.SetActive(false);
        }

        //Get the damage percentage in 0 to 1
        float damagePercentage = 1.0f - percentageOfHealth;

        //Set the greyscale amount equal to the percentage
        _healthShader._greyScaleAmount = damagePercentage;

        //Calculate the incremental amount between each image
        float increment = 1.0f / _splatterImages.Length;
        int i = 0;
        for(float f = 0; f < 1.0; f += increment)
        {
            if (damagePercentage <= f) return;

            //Sets the image to be active 
            _splatterImages[i]?.gameObject.SetActive(true);
            i++;
        }

    }

}
