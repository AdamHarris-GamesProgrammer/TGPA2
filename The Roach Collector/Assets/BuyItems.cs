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

    [SerializeField] private BuyableItemUI _buyablePrefab;
    [SerializeField] private List<BuyableItem> _items;
    [SerializeField] Transform _itemTransform;

    [SerializeField] Text _itemNameText;
    [SerializeField] Text _itemDescriptionText;
    [SerializeField] Text _playerCash;

    public List<BuyableItem> Items { get { return _items; } }

    Inventory _playerInventory;
    PlayerController _player;

    static BuyableItemUI _selectedItem = null;
    static List<BuyableItem> _selectedList = null;

    public void SelectItem(BuyableItemUI ui)
    {
        _selectedList = ui.transform.parent.GetComponent<BuyItems>().Items;
        _selectedItem = ui;

        if (_selectedItem != null)
        {
            BuyableItem item = _items[_selectedItem.Index];

            _itemNameText.text = item._item.Name;
            _itemDescriptionText.text = item._item.Description;
        }
        else
        {
            _itemNameText.text = "";
            _itemDescriptionText.text = "";
        }
    }

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _playerInventory =_player.GetComponent<Inventory>();
        _selectedList = new List<BuyableItem>();
        for(int i = 0; i < _items.Count; i++)
        {
            var item = Instantiate(_buyablePrefab, _itemTransform);

            item.Setup(_items[i]._item.Icon, _items[i]._item.name, _items[i]._price, _items[i]._quantity, i);
        }

        
    }

    private void OnEnable()
    {
        _playerCash.text = "Your cash: " + _player.Cash.ToString("#0.00");
    }

    public void BuyItem()
    {
        if(_selectedItem != null)
        {
            BuyableItem item = _selectedList[_selectedItem.Index];

            if (_player.HasEnoughMoney(item._price))
            {
                _player.SpendMoney(item._price);

                _playerInventory.AddToFirstEmptySlot(item._item, item._quantity);
            }

            _playerCash.text = "Your cash: " + _player.Cash.ToString("#0.00");
        }
    }

}
