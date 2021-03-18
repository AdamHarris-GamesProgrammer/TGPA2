using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Cinemachine.CinemachineFreeLook _camera;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource _cameraShake;

    void Awake()
    {
        _cameraShake = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    public Vector2[] _recoilPattern;
    float _verticalRecoil;
    float _horizontalRecoil;

    public float _duration;

    float _time;

    int index;


    int GetNextIndex()
    {
        return (index + 1) % _recoilPattern.Length;
    }

    public void Reset()
    {
        index = 0;
    }

    public void GenerateRecoil()
    {
        _time = _duration;

        _cameraShake.GenerateImpulse(Camera.main.transform.forward);

        _horizontalRecoil = _recoilPattern[index].x;
        _verticalRecoil = _recoilPattern[index].y;

        index = GetNextIndex();
    }


    // Update is called once per frame
    void Update()
    {
        if(_time > 0)
        {
            _camera.m_YAxis.Value -= ((_verticalRecoil/1000) * Time.deltaTime) / _duration;
            _camera.m_XAxis.Value -= ((_horizontalRecoil/10) * Time.deltaTime) / _duration;
            _time -= Time.deltaTime;
        }
    }
}
