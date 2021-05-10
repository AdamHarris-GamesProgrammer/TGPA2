using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Harris.Inventories;


namespace Harris.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] Text _itemNumber = null;


        public void SetItem(InventoryItem item)
        {
            SetItem(item, 0);
        }

        public void SetItem(InventoryItem item, int number)
        {
            //used for enabling or disabling the icon image
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                //Disable the icon image if we have no item
                iconImage.enabled = false;
            }
            else
            {
                //Enables the icon image if we have a new item
                iconImage.enabled = true;
                iconImage.sprite = item.Icon;
            }

            //if we have a item number
            if (_itemNumber)
            {
                //if there is only 1 or less items then disable the item number text
                if (number <= 1)
                {
                    _itemNumber.gameObject.SetActive(false);
                }
                else
                {
                    //enable the item number text
                    _itemNumber.gameObject.SetActive(true);
                    _itemNumber.text = number.ToString();
                }
            }
        }
    }
}