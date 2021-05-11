using UnityEngine;
using UnityEngine.UI;
using Harris.Inventories;

namespace Harris.UI.Inventories
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] Text _titleText = null;
        [SerializeField] Text _bodyText = null;

        //Sets the information for the tooltip
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
