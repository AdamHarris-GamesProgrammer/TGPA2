using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Cinemachine.CinemachineVirtualCamera _camera;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource _cameraShake;

    void Awake()
    {
        //Get the impulse source component
        _cameraShake = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }


    public void GenerateRecoil()
    {
        if (!gameObject.transform.IsChildOf(GameObject.FindGameObjectWithTag("Player").transform)) return;

        //Generate the impulse for the recoil
        _cameraShake.GenerateImpulse(Camera.main.transform.forward);
    }
}
