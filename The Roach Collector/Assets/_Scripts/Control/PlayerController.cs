using Harris.Inventories;
using Harris.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Harris.Inventories.Inventory;

namespace TGP.Control
{
    public class PlayerController : MonoBehaviour, ISaveable
    {
        [Header("Camera Settings")]
        [SerializeField] GameObject _aimCam;
        [SerializeField] GameObject _followCam;

        [Header("Text Settings")]
        [SerializeField] Text _roachText;
        [SerializeField] Text _cashText;

        bool _isDancing = false;
        public bool IsDancing { get { return _isDancing; } set { _isDancing = value; } }

        AIAgent _agentInRange = null;

        bool _inKillAnimation = false;

        public bool InKillAnimation { get { return _inKillAnimation; } }

        public AIAgent AgentInRange { get { return _agentInRange; }
            set {
                _agentInRange = value;
                if(!_isStanding) gameObject.SendMessage("DisplayAssassinationPrompt", _agentInRange != null);
            } }

        private GameObject _chestInventory;
        public GameObject ChestInventory { get { return _chestInventory; } }



        public GameObject AimCam { get { return _aimCam; } }
        public GameObject FollowCam { get { return _followCam; } }

        LockedDoor _doorInRange = null;

        bool _isShooting = false;
        bool _isStanding = true;

        public bool IsShooting { get { return _isShooting; } set { _isShooting = value; } }
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }

        private float _currency = 0.0f;
        private int _roaches = 0;

        public float Cash { get { return _currency; } }

        bool _isStationary = false;
        public bool IsStationary { get { return _isStationary;} set { _isStationary = value; } }

        Animator _animator;

        public void SpendRoach(int amount) {
            _roaches -= amount;
            UpdateRoach();
        }

        public void GainRoach(int amount) {
            _roaches += amount;
            UpdateRoach();
        }

        void UpdateCash(){
            _cashText.text = _currency.ToString("#0.00");
        }

        void UpdateRoach() {
            _roachText.text = _roaches.ToString();
        }

        public bool HasEnoughRoach(int amount) {
            return (_roaches - amount) >= 0;
        }

        public void SpendMoney(float amount) {
            _currency -= amount;
            UpdateCash();
        }

        public void GainMoney(float amount) {
            _currency += amount;
            UpdateCash();
        }

        public bool HasEnoughMoney(float amount) {
            return (_currency - amount >= 0.0f);
        }

        public LockedDoor DoorInRange
        {
            get { return _doorInRange; }
            set
            {
                _doorInRange = value;
                gameObject.SendMessage("DisplayDoorPrompt", value != null);
            }
        }

        public void ResetStats()
        {
            for (int i = 0; i < _stats.Length; i++)
            {
                _stats[i]._value = 0.0f;
            }
        }

        bool _detected = false;
        public bool IsDetected { get { return _detected; } set { _detected = value; } }

        ActionStore _actionSlots;

        Inventory _playerInventory;




        [SerializeField] StatValues[] _stats;

        public void EquipStat(StatValues stat)
        {
            //Cycle through each stat
            for (int i = 0; i < _stats.Length; i++)
            {
                //Check if the stat id is equal to the passed in stat id
                if (_stats[i]._id == stat._id)
                {
                    //Add the value to this value
                    _stats[i]._value += stat._value;

                    //Debug.Log(_stats[i]._id + " is now " + _stats[i]._value);
                }
            }
        }

        public void UnequipStat(StatValues id)
        {
            for(int i = 0; i < _stats.Length; i++)
            {
                if(_stats[i]._id == id._id)
                {
                    _stats[i]._value -= id._value;
                }
            }
        }

        public StatValues GetStat(StatID id)
        {
            foreach (StatValues stat in _stats)
            {
                if (stat._id == id) return stat;
            }

            return new StatValues(StatID.NONE, 1.0f);
        }

        //TODO: Implement these into damage calculations, movement calculations etc. 





        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionSlots = GetComponent<ActionStore>();
            _playerInventory = GetComponent<Inventory>();

            _chestInventory = GameObject.FindGameObjectWithTag("ChestCanvas");
            _chestInventory.SetActive(false);

            UpdateCash();
            UpdateRoach();
        }


        private void InteractWithAssassination()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isStanding) return;

                if (_agentInRange != null && !_detected)
                {
                    if (_agentInRange.Health.IsDead) return;

                    Vector3 offSetPos = _agentInRange.transform.position - _agentInRange.transform.forward * 1.0f;

                    transform.position = offSetPos;

                    _animator.SetTrigger("stealthAssassinate");

                    _agentInRange.BeingKilled = true;

                    _inKillAnimation = true;

                    _agentInRange.GetComponent<Animator>().SetTrigger("stealthAssassinate");

                    _agentInRange = null;

                    gameObject.SendMessage("DisplayAssassinationPrompt", false);

                }
            }
        }



        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Y)) _animator.SetTrigger("Dance");

            InteractWithAssassination();

            InteractWithActionBar();

            InteractWithLockedDoor();
        }

        private void InteractWithLockedDoor()
        {
            if (_doorInRange != null && !_doorInRange.IsUnlocked)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LockedDoorID doorID = _doorInRange.GetLockID();

                    InventorySlot[] slots = _playerInventory.GetFilledSlots();

                    foreach (InventorySlot slot in slots)
                    {
                        KeycardItem keycard = slot.item as KeycardItem;

                        if (keycard) if (keycard.GetUnlockables() == doorID) _doorInRange.Unlock();
                    }
                }
            }
        }

        private void InteractWithActionBar()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))      _actionSlots.Use(0, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) _actionSlots.Use(1, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) _actionSlots.Use(2, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) _actionSlots.Use(3, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) _actionSlots.Use(4, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha6)) _actionSlots.Use(5, this.gameObject);
        }

        //Animation event from the StealthAttack animation 
#pragma warning disable IDE0051 // Remove unused private members //This is just disabling a warning as OutOfKillAnim is not technically used in code but instead is called in a animation
        void OutOfKillAnim() => _inKillAnimation = false;
#pragma warning restore IDE0051 // Remove unused private members

        [System.Serializable]
        struct SaveRecord
        {
            public float cash;
            public int roaches;
        }

        public object Save()
        {
            SaveRecord saveData;
            saveData.cash = _currency;
            saveData.roaches = _roaches;

            return saveData;
        }

        public void Load(object state)
        {
            SaveRecord record = (SaveRecord)state;

            _currency = record.cash;
            _roaches = record.roaches;

            UpdateRoach();
            UpdateCash();
        }
    }


}

