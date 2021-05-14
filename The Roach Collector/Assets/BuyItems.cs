using Harris.Inventories;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.UI;

public class BuyItems : MonoBehaviour
{
    [System.Serializable]
    struct BuyableItem
    {
        public InventoryItem _item;
        public int _quantity;
        public float _price;
    }

    [SerializeField] private BuyableItemUI _buyablePrefab;
    [SerializeField] private BuyableItem[] _items;
    [SerializeField] Transform _itemTransform;

    [SerializeField] Text _itemNameText;
    [SerializeField] Text _itemDescriptionText;
    

    Inventory _playerInventory;

    BuyableItemUI _selectedItem = null;

    public void SelectItem(BuyableItemUI ui)
    {
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
        _playerInventory = FindObjectOfType<PlayerController>().GetComponent<Inventory>();

        for(int i = 0; i < _items.Length; i++)
        {
            var item = Instantiate(_buyablePrefab, _itemTransform);

            item.Setup(_items[i]._item.Icon, _items[i]._item.name, _items[i]._price, _items[i]._quantity, i);
        }
    }


}
