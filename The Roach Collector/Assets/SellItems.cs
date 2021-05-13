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

    [SerializeField] SellItemUI _itemObject;

    [SerializeField] GameObject _itemList;

    List<SellableItem> _sellables;

    SellableItem _selectedItem = null;

    void Awake() {
        _player = GameObject.FindObjectOfType<PlayerController>();
        _playerInventoy = _player.GetComponent<Inventory>();
        _sellables = new List<SellableItem>();

        LoadItems();
    }

    public void LoadItems() {
        _sellables.Clear();
        SelectItem(null);

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

             int index = _playerInventoy.Find(item);

            slot.Setup(item.Icon, item.Name, item, _playerInventoy.GetInventorySlot(index).number);
        }
        
    }

    public void SelectItem(SellableItem item) {
        _selectedItem = item;

        if(_selectedItem) {
            _itemDescription.text = item.Description;
            _itemName.text = item.Name;
        }else
        {
            _itemDescription.text = "";
            _itemName.text = "";    
        }
    }

    public void SellItem() {
        if(_selectedItem != null) {
            Debug.Log("Selling item");

            _player.GainMoney(_selectedItem.ItemValue);

            int index = _playerInventoy.Find(_selectedItem);
            
            Debug.Log(index);

            _playerInventoy.RemoveFromSlot(index, 1);

            if(_playerInventoy.GetInventorySlot(index).number == 0) {
                Debug.Log("Removing from sellables");
                _sellables.Remove(_selectedItem);
            }            

            LoadItems();
        }
    }
}
