using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Control;
using Harris.Inventories;
using UnityEngine.UI;

public class SellItems : MonoBehaviour
{
    PlayerController _player;
    Inventory _playerInventoy;

    [SerializeField] Text _itemDescription;
    [SerializeField] Text _itemName;

    [SerializeField] GameObject _itemObject;

    [SerializeField] GameObject _itemList;

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

        //remove objects
        foreach (Transform child in _itemList.transform)
        {
                Destroy(child.gameObject);
        }

        foreach(SellableItem item in _sellables) {
            var slot = Instantiate(_itemObject, _itemList.transform);



        }
        
    }

    public void SelectItem(){

    }

    void SellItem(SellableItem item, int amount) {

    }
}
