using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Text ammoTxt;

    [SerializeField] GameObject _unlockDoorPrompt = null;
    [SerializeField] GameObject _assassinationPrompt = null;

    [Header("Applying Text Settings")]
    [SerializeField] GameObject _applyingHealthText = null;
    [SerializeField] GameObject _applyingDamageText = null;
    [SerializeField] GameObject _applyingResistanceText = null;
    [SerializeField] GameObject _applyingSpeedText = null;

    [SerializeField] GameObject _pauseUI;
    private bool isPaused = false;

    List<UsableItem> _usables;
    List<UsableItem> _itemsToRemoveThisFrame;

    //Adds a usable to be removed at the end of this frame
    public void RemoveUsable(UsableItem item)
    {
        _itemsToRemoveThisFrame.Add(item);
    }

    private void Awake()
    {
        UpdateAmmoUI(0, 0, 0);
        _usables = new List<UsableItem>();
        _itemsToRemoveThisFrame = new List<UsableItem>();
    }

    //update ammo whenever player shoots, reloads or gains ammo. clip is ammo in clip. clipsize is for ammo in each reload and anmoLeft is total ammo
    public void UpdateAmmoUI(int clip, int clipSize, int ammoLeft)
    {
        ammoTxt.text = clip + " / " + (ammoLeft);
        if (ammoLeft > 0) ammoTxt.color = Color.white;
        else ammoTxt.color = Color.red;
    }

    public void DisplayDoorPrompt(bool val)
    {
        _unlockDoorPrompt.SetActive(val);
    }

    public void DisplayAssassinationPrompt(bool val)
    {
        _assassinationPrompt.SetActive(val);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                _pauseUI.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                _pauseUI.SetActive(false);
                
            }
        }

        InteractWithUsables();
    }

    public void AddUsable(UsableItem item)
    {
        //Sets a applying text to be active
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

    private void InteractWithUsables()
    {
        foreach (UsableItem item in _usables)
        {
            item.Update(Time.deltaTime);

            //Used to update the timer
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

            //Get the remaining time
            float remainingTime = item.GetApplyTimeRemaining();

            //Updates the timer
            _timer.SetTimer(remainingTime);

            //Disables the object when the time has ran out
            if (remainingTime <= 0.0f) _timer.gameObject.transform.parent.gameObject.SetActive(false);

        }

        //Removes the finished items from the usables list
        foreach (UsableItem item in _itemsToRemoveThisFrame)
        {
            _usables.Remove(item);
        }

        _itemsToRemoveThisFrame.Clear();
    }
}
