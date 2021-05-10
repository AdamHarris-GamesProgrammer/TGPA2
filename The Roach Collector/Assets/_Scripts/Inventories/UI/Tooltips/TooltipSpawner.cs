using UnityEngine;
using UnityEngine.EventSystems;

namespace Harris.Core.UI.Tooltips
{
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Tooltip("The prefab of the tooltip to spawn.")]
        [SerializeField] GameObject _tooltipPrefab = null;

        GameObject _tooltip = null;

        //Updates the Tooltip gameobject (sets up the tooltips title and description)
        public abstract void UpdateTooltip(GameObject tooltip);
        
        //Can we create a tooltip? (do we have an item in this slot)
        public abstract bool CanCreateTooltip();

        //Clears the tooltip on destroy
        private void OnDestroy()
        {
            ClearTooltip();
        }

        //Clears the tooltip on disable
        private void OnDisable()
        {
            ClearTooltip();
        }

        //Has a pointer entered this gameobject (the inventory slot)
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            //Gets the canvas
            var parentCanvas = GetComponentInParent<Canvas>();

            //Have we got a tooltip and if we cannot create a tooltip
            if (_tooltip && !CanCreateTooltip())
            {
                //Clear the tooltip
                ClearTooltip();
            }

            //if we have not got a tooltip and we can create a tooltip
            if (!_tooltip && CanCreateTooltip())
            {
                //Instantiate a tooltip
                _tooltip = Instantiate(_tooltipPrefab, parentCanvas.transform);
            }

            //If we have a tooltip
            if (_tooltip)
            {
                //Update the tooltip and then position it
                UpdateTooltip(_tooltip);
                PositionTooltip();
            }
        }

        private void PositionTooltip()
        {
            //Forces the canvas to update
            Canvas.ForceUpdateCanvases();

            //Stores the positions of the corners of the tooltip
            var tooltipCorners = new Vector3[4];

            //Gets the location of the item slot
            _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);

            //Get the corners of the slot and store them
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            //are we on the bottom half of the screen
            bool below = transform.position.y > Screen.height / 2;

            //are we on the right hand side of the screen
            bool right = transform.position.x < Screen.width / 2;

            //Decide which slot corner to spawn from
            int slotCorner = GetCornerIndex(below, right);

            //Decide which corner to spawn on the tooltip. 
            int tooltipCorner = GetCornerIndex(!below, !right);

            _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            //bottom left
            if (below && !right) return 0;

            //top left
            else if (!below && !right) return 1;

            //top right
            else if (!below && right) return 2;

            //top right
            else return 3;

        }

        //Clear the tooltip when our mouse moves out of the icons Gameobject 
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        //Destroys the tooltip
        private void ClearTooltip()
        {
            //if we have a tooltip
            if (_tooltip)
            {
                //then destroy it
                Destroy(_tooltip.gameObject);
            }
        }
    }
}