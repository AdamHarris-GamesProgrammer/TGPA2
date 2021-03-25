using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
}

public class WeaponIK : MonoBehaviour
{
    public Transform _target;
    public Transform _aimTransform;

    Transform _weaponTransform;

    public Vector3 _offset;

    AIAgent _agent;

    public HumanBone[] _humanBones;
    Transform[] _boneTransforms;

    public float _angleLimit = 90.0f;
    public float _distanceLimit = 1.5f;

    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        _boneTransforms = new Transform[_humanBones.Length];
        for(int i = 0; i < _boneTransforms.Length; i++)
        {
            _boneTransforms[i] = anim.GetBoneTransform(_humanBones[i].bone);
        }

        _agent = GetComponent<AIAgent>();

        if (_agent._aiWeapon.HasWeapon())
        {
            _weaponTransform = GetComponentInChildren<RaycastWeapon>().transform;
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (_target.position + _offset) - _aimTransform.position;
        Vector3 aimDirection = _aimTransform.forward;
        float blendOut = 0.0f;
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngle > _angleLimit)
        {
            blendOut += (targetAngle - _angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < _distanceLimit)
        {
            blendOut += _distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return _aimTransform.position + direction;

    }

    private void LateUpdate()
    {
        if (_aimTransform == null) return;
        if (_target == null) return;
        if (_weaponTransform == null) return;

        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < _boneTransforms.Length; i++)
        {
            Transform bone = _boneTransforms[i];
            AimAtTarget(bone, targetPosition);
        }

        _weaponTransform.LookAt(GetTargetPosition(), Vector3.up);
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = _aimTransform.forward;
        Vector3 targetDirection = targetPosition - _aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        bone.rotation = aimTowards * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        _target = target;
    }

    public void SetAimTransform(Transform target)
    {
        _aimTransform = target;
    }

    public void SetWeaponTransform(Transform target)
    {
        _weaponTransform = target;
    }
}
