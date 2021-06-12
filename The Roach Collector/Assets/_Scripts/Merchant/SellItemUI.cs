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
        //Set the items icon
        _itemSprite.sprite = sprite;

        //Set the name value and quantity text.
        _itemName.text = name;
        _valueText.text = "Value: " + item.ItemValue.ToString("#0.00");
        _quantityText.text = "Quantity: " + quantity;
        _item = item;
    }

    public void SelectItem() {
        GetComponentInParent<SellItems>().SelectItem(_item);
    }
}
