using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadTimer : MonoBehaviour
{
    [SerializeField] Image _outline;

    float _duration;
    public float Duration { get { return _duration; } set { _duration = value; } }

    float _timer = 0.0f;

    bool _active = false;
    public bool Active { get { return _active; } set { _active = value; } }

    float _percentageDone = 0.0f;

    private void Update()
    {
        _timer += Time.deltaTime;

        _percentageDone = _timer / _duration;
        _outline.fillAmount = _percentageDone;

        if (_timer > _duration)
        {
            _timer = 0.0f;

            _outline.fillAmount = 0.0f;
            gameObject.SetActive(false);
        }

    }
}
