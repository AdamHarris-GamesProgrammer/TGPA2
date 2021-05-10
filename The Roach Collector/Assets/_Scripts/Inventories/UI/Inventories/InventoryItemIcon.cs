using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Harris.Inventories;


namespace Harris.UI.Inventories
{
    /// <summary>
    /// To be put on the icon representing an inventory item. Allows the slot to
    /// update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] Text itemNumber = null;

        // PUBLIC

        public void SetItem(InventoryItem item)
        {
            SetItem(item, 0);
        }

        public void SetItem(InventoryItem item, int number)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.Icon;
            }

            if (itemNumber)
            {
                if (number <= 1)
                {
                    itemNumber.gameObject.SetActive(false);
                }
                else
                {
                    itemNumber.gameObject.SetActive(true);
                    itemNumber.text = number.ToString();
                }
            }
        }
    }
}