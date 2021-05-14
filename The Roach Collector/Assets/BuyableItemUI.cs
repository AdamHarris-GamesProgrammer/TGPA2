using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyableItemUI : MonoBehaviour
{
    [SerializeField] Image _itemImage;
    [SerializeField] Text _itemName;
    [SerializeField] Text _itemPrice;
    [SerializeField] Text _itemQuantity;

    public void Setup(Sprite icon, string name, float price, int quantity)
    {
        _itemImage.sprite = icon;
        _itemName.text = name;
        _itemPrice.text = "Price: " + price.ToString("#0.00");
        _itemQuantity.text = "x" + quantity;
    }

}
