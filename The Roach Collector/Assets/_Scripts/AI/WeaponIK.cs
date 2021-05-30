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

    AIWeapons _aiWeapon;

    public HumanBone[] _humanBones;
    Transform[] _boneTransforms;

    public float _angleLimit = 90.0f;
    public float _distanceLimit = 1.5f;

    private void Start()
    {
        //Gets the animator
        Animator anim = GetComponent<Animator>();

        //Initalizes the bone transforms array
        _boneTransforms = new Transform[_humanBones.Length];

        //Cycles through the length
        for(int i = 0; i < _boneTransforms.Length; i++)
        {
            //Gets the transforms of the desired bones
            _boneTransforms[i] = anim.GetBoneTransform(_humanBones[i].bone);
        }

        //Gets the weapon
        _aiWeapon = GetComponent<AIWeapons>();

        //if we have a weapon
        if (_aiWeapon.HasWeapon())
        {
            //Stores the weapon transform
            _weaponTransform = GetComponentInChildren<RaycastWeapon>().transform;
        }
    }

    Vector3 GetTargetPosition()
    {
        //Stores the direction to the target
        Vector3 targetDirection = (_target.position + _offset) - _aimTransform.position;

        //Gets the direction
        Vector3 aimDirection = _aimTransform.forward;


        float blendOut = 0.0f;
        //Blends the transforms back incase they are too extreme
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngle > _angleLimit) blendOut += (targetAngle - _angleLimit) / 50.0f;

        //Blends the transforms back incase the the target is too close
        float targetDistance = targetDirection.magnitude;
        if(targetDistance < _distanceLimit) blendOut += _distanceLimit - targetDistance;

        //Gets the direction
        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);

        //Aims towards the target
        return _aimTransform.position + direction;

    }

    private void LateUpdate()
    {
        if (_aimTransform == null) return;
        if (_target == null) return;
        if (_weaponTransform == null) return;

        //Gets the target positon
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < _boneTransforms.Length; i++)
        {
            //Aims each bone at the target
            Transform bone = _boneTransforms[i];
            AimAtTarget(bone, targetPosition);
        }

        //Look at the target
        _weaponTransform.LookAt(GetTargetPosition(), Vector3.up);
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        //Gets the direction
        Vector3 aimDirection = _aimTransform.forward;
        Vector3 targetDirection = targetPosition - _aimTransform.position;
        //Calculates the rotation needed to rotate to the target
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        //Sets the rotation
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
