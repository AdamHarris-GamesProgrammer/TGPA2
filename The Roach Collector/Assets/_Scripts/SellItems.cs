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

    //The quantity slider
    [SerializeField] Slider _quantitySlider;

    SellableItem _selectedItem = null;

    void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerController>();
        _playerInventoy = _player.GetComponent<Inventory>();
        _sellables = new List<SellableItem>();

        //Load the items the player can sell
        LoadItems();
    }

    public void LoadItems()
    {
        //Clear the sellables
        _sellables.Clear();
        //Set the item to null
        SelectItem(null);

        //Cycle through all filled slots
        foreach (Inventory.InventorySlot item in _playerInventoy.GetFilledSlots())
        {
            //Try casting to a sellable item
            SellableItem sellable = item.item as SellableItem;

            //if it is a sellable item then add it to the sellables list
            if (sellable) _sellables.Add(sellable);
        }

        //remove objects
        foreach (Transform child in _itemList.transform) Destroy(child.gameObject);

        //Cycle through all sellables 
        foreach (SellableItem item in _sellables)
        {
            //Instantiate the sellable iobject in the form of the sellitem prefab
            var slot = Instantiate(_itemObject, _itemList.transform);

            int index = _playerInventoy.Find(item);

            slot.Setup(item.Icon, item.Name, item, _playerInventoy.GetInventorySlot(index).number);
        }

    }

    public void SelectItem(SellableItem item)
    {
        _selectedItem = item;

        //Set the item name and description accordingly
        if (_selectedItem)
        {
            _itemDescription.text = item.Description;
            _itemName.text = item.Name;
        }
        else
        {
            _itemDescription.text = "";
            _itemName.text = "";
        }
    }

    public void SellItem()
    {
        if (_selectedItem != null)
        {
            int quantity = (int)_quantitySlider.value;
           

            //Find the index of the item
            int index = _playerInventoy.Find(_selectedItem);

            int quantityOfItems = _playerInventoy.GetInventorySlot(index).number;

            if (quantityOfItems >= quantity)
            {
                //Give the player money
                _player.GainMoney(_selectedItem.ItemValue * quantity);
                //Remove one of that item from the player
                _playerInventoy.RemoveFromSlot(index, quantity);

                //Remove the item from sellables
                if (_playerInventoy.GetInventorySlot(index).number == 0) _sellables.Remove(_selectedItem);
            }
            else
            {
                int reAdjustSlider = _playerInventoy.GetInventorySlot(index).number;
                _quantitySlider.maxValue = reAdjustSlider;
            }

            

            

           

            //Load the items
            LoadItems();
        }
    }
}
