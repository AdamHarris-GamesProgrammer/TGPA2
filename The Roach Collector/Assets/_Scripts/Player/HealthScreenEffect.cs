using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HealthScreenEffect : MonoBehaviour
{
    public Shader _currentShader;

    [Range(0.0f, 1.0f)]
    public float _greyScaleAmount = 1.0f;

    private Material _currentMaterial;

    Material material
    {
        get
        {
            if(_currentMaterial == null)
            {
                _currentMaterial = new Material(_currentShader);
                _currentMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _currentMaterial;
        }
    }

    private void Start()
    {
        if(_currentShader && !_currentShader.isSupported)
        {
            enabled = false;
        }
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(_currentShader != null)
        {
            material.SetFloat("_LuminosityAmount", _greyScaleAmount);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        _greyScaleAmount = Mathf.Clamp(_greyScaleAmount, 0.0f, 1.0f);
    }

    private void OnDisable()
    {
        if(_currentMaterial)
        {
            DestroyImmediate(_currentMaterial);
        }
    }
}

