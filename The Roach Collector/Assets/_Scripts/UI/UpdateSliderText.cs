using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text _textToUpdate;
    [SerializeField] Slider _slider; 

    public void UpdateText()
    {
        _textToUpdate.text = _slider.value.ToString("#0");
    }
}
