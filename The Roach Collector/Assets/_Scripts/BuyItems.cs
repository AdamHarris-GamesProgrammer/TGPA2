using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.UI;

public class BuyItems : MonoBehaviour
{
    [System.Serializable]
    public struct BuyableItem
    {
        public InventoryItem _item;
        public int _quantity;
        public float _price;
    }

    //The prefab for the buyable item ui
    [SerializeField] private BuyableItemUI _buyablePrefab;

    //List of all the buyable items
    [SerializeField] private List<BuyableItem> _items;
    [SerializeField] Transform _itemTransform;

    //The description panel
    [SerializeField] Text _itemNameText;
    [SerializeField] Text _itemDescriptionText;
    [SerializeField] Text _playerCash;

    public List<BuyableItem> Items { get { return _items; } }

    Inventory _playerInventory;
    PlayerController _player;

    //Only one selected item and selected list instance between all BuyItem instances
    static BuyableItemUI _selectedItem = null;
    static List<BuyableItem> _selectedList = null;

    public void SelectItem(BuyableItemUI ui)
    {
        //Set the selected list and item
        _selectedList = ui.transform.parent.GetComponent<BuyItems>().Items;
        _selectedItem = ui;

        //if we have a selected item
        if (_selectedItem != null)
        {
            //Get the item based on the index
            BuyableItem item = _items[_selectedItem.Index];

            //Set the name and description text
            _itemNameText.text = item._item.Name;
            _itemDescriptionText.text = item._item.Description;
        }
        else
        {
            //Set the texts to blank
            _itemNameText.text = "";
            _itemDescriptionText.text = "";
        }
    }

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerInventory =_player.GetComponent<Inventory>();
        _selectedList = new List<BuyableItem>();
        //Cycle through all buyable items
        for(int i = 0; i < _items.Count; i++)
        {
            //Instantiate and setup the items
            var item = Instantiate(_buyablePrefab, _itemTransform);
            item.Setup(_items[i]._item.Icon, _items[i]._item.name, _items[i]._price, _items[i]._quantity, i);
        }

        
    }

    private void OnEnable()
    {
        //Updates the Cash UI
        _playerCash.text = "Your cash: " + _player.Cash.ToString("#0.00");
    }

    public void BuyItem()
    {
        if(_selectedItem != null)
        {
            //Get the item
            BuyableItem item = _selectedList[_selectedItem.Index];

            //Check the player has enough money
            if (_player.HasEnoughMoney(item._price))
            {
                //Spend the money
                _player.SpendMoney(item._price);

                //Add the item to the first empty slot
                _playerInventory.AddToFirstEmptySlot(item._item, item._quantity);
            }

            _playerCash.text = "Your cash: " + _player.Cash.ToString("#0.00");
        }
    }

}
