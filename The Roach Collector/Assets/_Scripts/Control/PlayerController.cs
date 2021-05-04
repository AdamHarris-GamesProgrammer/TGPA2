using Harris.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Harris.Inventories.Inventory;

namespace TGP.Control
{
    public class PlayerController : MonoBehaviour
    {
        bool _canDisableAlarm = false;

        AIAgent _agentInRange = null;

        bool _inKillAnimation = false;

        public bool InKillAnimation { get { return _inKillAnimation; } }

        public AIAgent AgentInRange { get { return _agentInRange; } set { _agentInRange = value; } }

        public bool CanDisableAlarm { get { return _canDisableAlarm; } set { _canDisableAlarm = value; } }
        AlarmController _alarm;


        [SerializeField] GameObject _applyingHealthText;
        [SerializeField] GameObject _applyingDamageText;
        [SerializeField] GameObject _applyingResistanceText;
        [SerializeField] GameObject _applyingSpeedText;


        LockedDoor _doorInRange;
        public LockedDoor DoorInRange
        {
            get { return _doorInRange; }
            set
            {
                _doorInRange = value;
                if (value == null)
                {
                    _unlockDoorPrompt.SetActive(false);
                }
                else
                {
                    _unlockDoorPrompt.SetActive(true);
                }
            }
        }



        bool _detected = false;
        public bool IsDetected { get { return _detected; } set { _detected = value; } }

        public AlarmController Alarm { set { _alarm = value; } }

        ActionStore _actionSlots;

        Animator _animator;

        [SerializeField] Vector3 _assassinOffset = Vector3.back;

        [SerializeField] GameObject _unlockDoorPrompt;

        Inventory _playerInventory;

        List<UsableItem> _usables;
        List<UsableItem> _itemsToRemoveThisFrame;


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
                }
            }
        }

        public void UnqeuipStat(StatValues stat)
        {
            //Cycle through each stat
            for (int i = 0; i < _stats.Length; i++)
            {
                //Check if the stat id is equal to the passed in stat id
                if (_stats[i]._id == stat._id)
                {
                    //Add the value to this value
                    _stats[i]._value -= stat._value;
                }
            }
        }


        public StatValues GetStat(StatID id)
        {
            foreach(StatValues stat in _stats)
            {
                if (stat._id == id) return stat;
            }

            return new StatValues(StatID.NONE, 1.0f);
        }

        //TODO: Equipping armor needs to modify the stats
        //TODO: Unequipping armor needs to modify the stats
        //TODO: to do this armor needs stats
        //TODO: Implement these into damage calculations, movement calculations etc. 
        //TODO: Get stats from the player

        public void AddUsable(UsableItem item)
        {
            _usables.Add(item);

            switch (item.GetID())
            {
                case UsableID.MEDKIT:
                    _applyingHealthText.SetActive(true);
                    break;
                case UsableID.DAMAGE:
                    _applyingDamageText.SetActive(true);
                    break;
                case UsableID.RESISTANCE:
                    _applyingResistanceText.SetActive(true);
                    break;
                case UsableID.SPEED:
                    _applyingSpeedText.SetActive(true);
                    break;
            }
        }

        public void RemoveUsable(UsableItem item)
        {
            _itemsToRemoveThisFrame.Add(item);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _actionSlots = GetComponent<ActionStore>();
            _playerInventory = GetComponent<Inventory>();
            _usables = new List<UsableItem>();
            _itemsToRemoveThisFrame = new List<UsableItem>();
        }



        private void InteractWithAssassination()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_agentInRange != null && !_detected)
                {
                    if (_agentInRange.GetHealth().IsDead()) return;
                    //TODO: Somehow make the animation look better 


                    Vector3 offSetPos = _agentInRange.transform.position - _assassinOffset;

                    transform.position = offSetPos;

                    _animator.SetTrigger("stealthAssassinate");

                    _agentInRange.BeingKilled = true;

                    _inKillAnimation = true;


                    _agentInRange.GetComponent<Animator>().SetTrigger("stealthAssassinate");

                    _agentInRange = null;

                }
            }
        }

        private void InteractWithUsables()
        {
            foreach (UsableItem item in _usables)
            {
                item.Update(Time.deltaTime);

                TimerText _timer = null;
                switch (item.GetID())
                {
                    case UsableID.MEDKIT:
                        _timer = _applyingHealthText.GetComponentInChildren<TimerText>();
                        break;
                    case UsableID.DAMAGE:
                        _timer = _applyingDamageText.GetComponentInChildren<TimerText>();
                        break;
                    case UsableID.RESISTANCE:
                        _timer = _applyingResistanceText.GetComponentInChildren<TimerText>();
                        break;
                    case UsableID.SPEED:
                        _timer = _applyingSpeedText.GetComponentInChildren<TimerText>();
                        break;
                }

                float remainingTime = item.GetApplyTimeRemaining();

                _timer.SetTimer(remainingTime);

                if (remainingTime <= 0.0f)
                {
                    _timer.gameObject.transform.parent.gameObject.SetActive(false);
                }

            }

            foreach (UsableItem item in _itemsToRemoveThisFrame)
            {
                _usables.Remove(item);
            }

            _itemsToRemoveThisFrame.Clear();
        }

        private void Update()
        {
            if (_canDisableAlarm && _alarm)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _alarm.DisableAlarm();
                }
            }
            InteractWithAssassination();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                InteractWithEquipment();

            }
            else
            {
                InteractWithActionBar();
            }

            InteractWithLockedDoor();


            InteractWithUsables();

        }

        private void InteractWithLockedDoor()
        {
            if (_doorInRange != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LockedDoorID doorID = _doorInRange.GetLockID();

                    InventorySlot[] slots = _playerInventory.GetFilledSlots();

                    foreach (InventorySlot slot in slots)
                    {
                        KeycardItem keycard = slot.item as KeycardItem;

                        if (keycard)
                        {
                            Debug.Log("Keycard");

                            if (keycard.GetUnlockables() == doorID)
                            {
                                _doorInRange.Unlock();
                            }
                        }

                    }

                }
            }

        }

        private void InteractWithEquipment()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Debug.Log("Shift + 1");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Debug.Log("Shift + 2");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //Debug.Log("Shift + 3");
            }
        }

        private void InteractWithActionBar()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _actionSlots.Use(0, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _actionSlots.Use(1, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _actionSlots.Use(2, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _actionSlots.Use(3, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _actionSlots.Use(4, this.gameObject);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _actionSlots.Use(5, this.gameObject);
            }
        }

        //Animation event from the StealthAttack animation 
        void OutOfKillAnim()
        {
            _inKillAnimation = false;
        }
    }


}

