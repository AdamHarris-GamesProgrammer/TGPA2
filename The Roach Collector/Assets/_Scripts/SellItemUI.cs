using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private Text _itemName;

    [SerializeField] private Text _quantityText;
    [SerializeField] private Text _valueText;

    private SellableItem _item;

    void Awake() {
    }

    public void Setup(Sprite sprite, string name, SellableItem item, int quantity) {
        _itemSprite.sprite = sprite;
        _itemName.text = name;
        _valueText.text = "Value: " + item.ItemValue.ToString("#0.00");
        _quantityText.text = "Quantity: " + quantity;
        _item = item;
    }

    public void SelectItem() {
        //TODO: Set this as the selected UI piece in the inventory 
        GetComponentInParent<SellItems>().SelectItem(_item);
    }
}
