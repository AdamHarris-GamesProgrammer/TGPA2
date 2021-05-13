using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _offset = Vector3.up;
    [SerializeField] private Image _foregroundImage = null;
    [SerializeField] private Image _backgroundImage = null;


    private void Awake()
    {
        _foregroundImage.gameObject.SetActive(true);
        _backgroundImage.gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        Vector3 direction = (_target.position - Camera.main.transform.position).normalized;

        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0;
        if(isBehind)
        {
            _foregroundImage.gameObject.SetActive(false);
            _backgroundImage.gameObject.SetActive(false);
        }
        else
        {
            _foregroundImage.gameObject.SetActive(true);
            _backgroundImage.gameObject.SetActive(true);
        }


        transform.position = Camera.main.WorldToScreenPoint(_target.position + _offset);
    }


    public void SetHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        _foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
