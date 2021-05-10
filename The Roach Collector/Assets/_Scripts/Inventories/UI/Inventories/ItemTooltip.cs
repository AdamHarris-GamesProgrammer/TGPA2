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
        [SerializeField] Text _titleText = null;
        [SerializeField] Text _bodyText = null;

        public void Setup(InventoryItem item)
        {
            _titleText.text = item.Name;
            _bodyText.text = item.Description;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
