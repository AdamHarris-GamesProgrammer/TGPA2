using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    Animator _anim;

    public LayerMask _layerMask;
    [Range(0f,1f)] public float _distanceToGround = 0.05f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        if (_anim == null) Debug.Log(gameObject.name + " has not got a Animator component attached");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_anim)
        {
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            RaycastHit hit;


            //Left Foot
            Ray ray = new Ray(_anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, _distanceToGround + 1f, _layerMask))
            {
                Vector3 leftFootPosition = hit.point;
                leftFootPosition.y += _distanceToGround;

                _anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition);

                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                _anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, hit.normal));


            }

            //Right foot
            ray = new Ray(_anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, _distanceToGround + 1f, _layerMask))
            {

                Vector3 rightFootPosition = hit.point;
                rightFootPosition.y += _distanceToGround;

                _anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition);

                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                _anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, hit.normal));
            }
        }
    }
}
