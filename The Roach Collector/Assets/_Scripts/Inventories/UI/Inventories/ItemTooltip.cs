using UnityEngine;
using UnityEngine.UI;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] Text titleText = null;
        [SerializeField] Text bodyText = null;

        // PUBLIC

        public void Setup(InventoryItem item)
        {
            titleText.text = item.Name;
            bodyText.text = item.Description;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
