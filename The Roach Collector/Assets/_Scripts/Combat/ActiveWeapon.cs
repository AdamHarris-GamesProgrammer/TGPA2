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

    [Header("Weapon Settings")]
    [SerializeField] private Transform _weaponParent = null;
    [SerializeField] private Transform _weaponLeftGrip = null;
    [SerializeField] private Transform _weaponRightGrip = null;
    [SerializeField] private RaycastWeapon _startingWeapon = null;

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
            if (Input.GetButtonDown("Fire1") && _weapon._clipAmmo > 0 && _weapon._isReloading == false)
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
            if (Input.GetKeyDown(KeyCode.R) && _weapon._totalAmmo > 0)
            {
                
                _weapon._isReloading = true;
                _weapon._totalAmmo += _weapon._clipAmmo;

                if (_weapon._totalAmmo < _weapon._clipSize)
                {
                    _weapon._clipAmmo = _weapon._totalAmmo;
                    _weapon._totalAmmo = 0;
                }
                else
                {
                    _weapon._clipAmmo = _weapon._clipSize;
                    _weapon._totalAmmo -= _weapon._clipSize;
                }

                _anim.SetBool("isReloading", true);
            }
            if (_weapon._isReloading == false)
            {
                _anim.SetBool("isReloading", false);
            }
        }
        else
        {
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

            _weapon.Recoil._camera = _camera;

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
