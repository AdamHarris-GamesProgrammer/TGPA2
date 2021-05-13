using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemUI : MonoBehaviour
{
    private Image _itemSprite;
    private Text _itemName;

    void Awake() {
        _itemSprite = GetComponentInChildren<Image>();
        _itemName = GetComponentInChildren<Text>();
    }

    public void Setup(Sprite sprite, string name) {
        _itemSprite.sprite = sprite;
        _itemName.text = name;
    }

    public void SelectItem() {
        //TODO: Set this as the selected UI piece in the inventory 
    }
}
