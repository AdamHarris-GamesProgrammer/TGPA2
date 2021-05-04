using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor.Animations;
#endif
public class ActiveWeapon : MonoBehaviour
{

    public Transform _crosshairTarget;
    [Header("Animation Settings")]
    [SerializeField] private UnityEngine.Animations.Rigging.Rig _handIK;

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Transform _weaponLeftGrip;
    [SerializeField] private Transform _weaponRightGrip;
    [SerializeField] private RaycastWeapon _startingWeapon;

    public Cinemachine.CinemachineFreeLook _camera;

    Animator _anim;
    AnimatorOverrideController _overrides;
    RaycastWeapon _weapon;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _overrides = _anim.runtimeAnimatorController as AnimatorOverrideController;

        //RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        //if(existingWeapon)
        //{
        //    Equip(existingWeapon);
        //}

        if (_startingWeapon)
        {
            RaycastWeapon weapon = Instantiate(_startingWeapon);

            Equip(weapon);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_weapon)
        {
            if (Input.GetButtonDown("Fire1") && _weapon._weaponConfig.ClipAmmo > 0 && _weapon._isReloading == false)
            {
                _weapon.StartFiring();
            }

            if (_weapon.IsFiring())
            {
                _weapon.UpdateWeapon(Time.deltaTime, _crosshairTarget.position);
            }

            _weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetButtonUp("Fire1"))
            {
                _weapon.StopFiring();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Equip(null);
                _overrides["Weapon_Anim_Empty"] = null;
            }
            if (Input.GetKeyDown(KeyCode.R) && _weapon._TotalAmno > 0)
            {
                
                _weapon._isReloading = true;
                _weapon._TotalAmno += _weapon._weaponConfig.ClipAmmo;

                if (_weapon._TotalAmno < _weapon._weaponConfig.ClipAmmo)
                {
                    _weapon._weaponConfig.ClipAmmo = _weapon._TotalAmno;
                    _weapon._TotalAmno = 0;
                }
                else
                {
                    _weapon._weaponConfig.ClipAmmo = _weapon._weaponConfig.ClipAmmo;
                    _weapon._TotalAmno -= _weapon._weaponConfig.ClipSize;
                }

                _anim.SetBool("isReloading", _weapon._isReloading);
            }
            if (_weapon._isReloading == false)
            {
                _anim.SetBool("isReloading", false);
            }
            if (_weapon._isReloading == false)
            {
                _anim.SetBool("isReloading", false);
            }
        }
        else
        {
            _handIK.weight = 0.0f;
            _anim.SetLayerWeight(1, 0.0f);
        }
    }

    public void DropWeapon()
    {
        if (_weapon)
        {
            _weapon.transform.SetParent(null);
            _weapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            _weapon.gameObject.AddComponent<Rigidbody>();
            _weapon = null;
        }
    }
    public void Equip(RaycastWeapon newWeapon)
    {
        if(_weapon)
        {
            Destroy(_weapon.gameObject);
        }

        _weapon = newWeapon;

        if (newWeapon)
        {
            _weapon.transform.parent = _weaponParent;
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.transform.localRotation = Quaternion.identity;
            _handIK.weight = 1.0f;
            _anim.SetLayerWeight(1, 1.0f);

            _weapon._weaponRecoil._camera = _camera;

            //Crash in current version of the Rigging package, this line fixes it
            Invoke(nameof(SetAnimationDelayed), 0.0001f);
        }
    }

    void SetAnimationDelayed()
    {
        _overrides["Weapon_Anim_Empty"] = _weapon.GetAnimationClip();
    }


#if UNITY_EDITOR
    [ContextMenu("Save Weapon Pose")]
    public void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(_weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(_weapon.GetAnimationClip());
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
