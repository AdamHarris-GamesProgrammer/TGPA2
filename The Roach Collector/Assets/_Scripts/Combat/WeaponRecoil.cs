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

    [SerializeField] Vector2[] _recoilPattern = null;
    float _verticalRecoil;
    float _horizontalRecoil;

    [SerializeField] float _duration = 0.1f;

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
        if (!gameObject.transform.IsChildOf(GameObject.FindGameObjectWithTag("Player").transform)) return;

        _time = _duration;

        _cameraShake.GenerateImpulse(Camera.main.transform.forward);

        _horizontalRecoil = _recoilPattern[index].x;
        _verticalRecoil = _recoilPattern[index].y;

        index = GetNextIndex();
    }


    // Update is called once per frame
    void Update()
    {
        if (!_camera) return;
        if(_time > 0)
        {
            _camera.m_YAxis.Value -= ((_verticalRecoil/1000) * Time.deltaTime) / _duration;
            _camera.m_XAxis.Value -= ((_horizontalRecoil/10) * Time.deltaTime) / _duration;
            _time -= Time.deltaTime;
        }
    }
}
