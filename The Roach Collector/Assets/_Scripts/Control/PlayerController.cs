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
        public GameObject AimCam { get { return _aimCam; } }
        [SerializeField] GameObject _followCam;
        public GameObject FollowCam { get { return _followCam; } }

        [Header("Text Settings")]
        [SerializeField] Text _roachText;
        [SerializeField] Text _cashText;

        [Header("Stat Settings")]
        [SerializeField] StatValues[] _stats;

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


        bool _isShooting = false;
        public bool IsShooting { get { return _isShooting; } set { _isShooting = value; } }

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }
        

        bool _isStationary = false;
        public bool IsStationary { get { return _isStationary;} set { _isStationary = value; } }

        private float _currency = 0.0f;
        public float Cash { get { return _currency; } }
        private int _roaches = 0;

        Animator _animator;


        //Stores the door we are near
        LockedDoor _doorInRange = null;
        public LockedDoor DoorInRange
        {
            get { return _doorInRange; }
            set
            {
                _doorInRange = value;
                //Displays the UI prompt
                gameObject.SendMessage("DisplayDoorPrompt", value != null);
            }
        }


        //Stores if we have been detected by the AI
        bool _detected = false;
        public bool IsDetected { get { return _detected; } set { _detected = value; } }

        ActionStore _actionSlots;

        //Store a reference to the inventory for checking keys
        Inventory _playerInventory;

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

        private void Update()
        {
            //Makes the player dance
            if (Input.GetKeyDown(KeyCode.Y)) _animator.SetTrigger("Dance");

            //Goes through all interact methods, separated for neatness
            InteractWithAssassination();
            InteractWithActionBar();
            InteractWithLockedDoor();
        }


        public void ResetStats()
        {
            //Reset all stats to 0
            for (int i = 0; i < _stats.Length; i++)
            {
                _stats[i]._value = 0.0f;
            }
        }

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

        public void UnequipStat(StatValues id)
        {
            //Cycles through the stats array and removes the value of the stat from the stat saved here
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
            //Cycle through each stat and see if it matches the passed inID
            foreach (StatValues stat in _stats)
            {
                if (stat._id == id) return stat;
            }
            //Return a none stat if no stat is found
            return new StatValues(StatID.NONE, 0.0f);
        }

        public void SpendRoach(int amount)
        {
            //Spends a roach
            _roaches -= amount;
            UpdateRoach();
        }

        public void GainRoach(int amount)
        {
            //Gives a roach
            _roaches += amount;
            UpdateRoach();
        }

        void UpdateCash()
        {
            //Updates the text for the cash
            _cashText.text = _currency.ToString("#0.00");
        }

        void UpdateRoach()
        {
            //Updates the text for the roaches
            _roachText.text = _roaches.ToString();
        }

        public bool HasEnoughRoach(int amount)
        {
            //Returns true if we have enough roaches
            return (_roaches - amount) >= 0;
        }

        public void SpendMoney(float amount)
        {
            //Spends money
            _currency -= amount;
            UpdateCash();
        }

        public void GainMoney(float amount)
        {
            //Adds money
            _currency += amount;
            UpdateCash();
        }

        public bool HasEnoughMoney(float amount)
        {
            //Returns true if we have enough money
            return (_currency - amount >= 0.0f);
        }

        private void InteractWithAssassination()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //If we are standing then return
                if (_isStanding) return;

                //if there is an agent in range and we have not been detected
                if (_agentInRange != null && !_detected)
                {
                    //if the agent is dead then return
                    if (_agentInRange.Health.IsDead) return;

                    //Calculate the offset
                    Vector3 offSetPos = _agentInRange.transform.position - _agentInRange.transform.forward * 1.0f;
                    //Set our position to that offset
                    transform.position = offSetPos;
                    //Call the assassinate animation
                    _animator.SetTrigger("stealthAssassinate");
                    //Set the agent to being killed
                    _agentInRange.BeingKilled = true;
                    //Set us in the kill animation, stops us from moving 
                    _inKillAnimation = true;
                    //Set the animation for the AI
                    _agentInRange.GetComponent<Animator>().SetTrigger("stealthAssassinate");
                    //Clears the agent
                    _agentInRange = null;
                    //Stops the UI from displaying the prompt
                    gameObject.SendMessage("DisplayAssassinationPrompt", false);

                }
            }
        }

        private void InteractWithLockedDoor()
        {
            //if we have a door in range and the door is not unlocked
            if (_doorInRange != null && !_doorInRange.IsUnlocked)
            {
                //Check if we try to open it
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Get the door id
                    LockedDoorID doorID = _doorInRange.GetLockID();

                    //Search through the inventory for a matching key
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
            //Handles input for keys 1 through 6 and uses the item in that slot
            if (Input.GetKeyDown(KeyCode.Alpha1))      _actionSlots.Use(0, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) _actionSlots.Use(1, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) _actionSlots.Use(2, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) _actionSlots.Use(3, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) _actionSlots.Use(4, this.gameObject);
            else if (Input.GetKeyDown(KeyCode.Alpha6)) _actionSlots.Use(5, this.gameObject);
        }

        void OutOfKillAnim() => _inKillAnimation = false;

        //Structure for saving the players cash and roaches amount
        [System.Serializable]
        struct SaveRecord
        {
            public float cash;
            public int roaches;
        }

        public object Save()
        {
            //Creates a new save record and fills it with data
            SaveRecord saveData;
            saveData.cash = _currency;
            saveData.roaches = _roaches;

            //Returns the data
            return saveData;
        }

        public void Load(object state)
        {
            //Casts the data as a SaveRecord
            SaveRecord record = (SaveRecord)state;

            //Reads the data
            _currency = record.cash;
            _roaches = record.roaches;

            //Updates the UI
            UpdateRoach();
            UpdateCash();
        }
    }


}

