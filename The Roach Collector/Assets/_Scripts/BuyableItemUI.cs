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

    

    int _index;
    public int Index { get { return _index; } }

    public void Setup(Sprite icon, string name, float price, int quantity, int index)
    {
        //Sets the item image to the icon
        _itemImage.sprite = icon;
        //Sets the item name, price and quantity text.
        _itemName.text = name;
        _itemPrice.text = "Price: " + price.ToString("#0.00");
        _itemQuantity.text = "x" + quantity;
        //Set the index
        _index = index;
    }

    public void SelectItem()
    {
        GetComponentInParent<BuyItems>().SelectItem(this);
    }

}
