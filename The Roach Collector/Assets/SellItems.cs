using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Control;
using Harris.Inventories;


public class SellItems : MonoBehaviour
{
    PlayerController _player;
    Inventory _playerInventoy;

    List<SellableItem> _sellables;

    void Awake() {
        _player = GameObject.FindObjectOfType<PlayerController>();
        _playerInventoy = _player.GetComponent<Inventory>();
        _sellables = new List<SellableItem>();
    }

    public void LoadItems() {
        foreach(Inventory.InventorySlot item in _playerInventoy.GetFilledSlots()){
            SellableItem sellable = item.item as SellableItem;

            if(sellable) {
                _sellables.Add(sellable);
                Debug.Log(sellable);
            }
        }

        
    }

    void SellItem(SellableItem item, int amount) {

    }
}
