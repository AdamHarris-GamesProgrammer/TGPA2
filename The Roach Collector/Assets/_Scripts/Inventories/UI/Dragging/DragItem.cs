using Harris.Inventories;
using Harris.UI.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Harris.Core.UI.Dragging
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler where T : class
    {
        Vector3 _startPosition;
        Transform _originalParent;
        IDragSource<T> _itemSource;

        Canvas _parentCanvas;

        private void Awake()
        {
            //Get the parent canvas and the current slot
            _parentCanvas = GetComponentInParent<Canvas>();
            _itemSource = GetComponentInParent<IDragSource<T>>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            //Set the starting position and the original parent of this item.
            _startPosition = transform.position;
            _originalParent = transform.parent;
            
            //Allows raycasts to the canvas
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            //Sets the transform to the canvas as opposed to the slot, used for dragging
            transform.SetParent(_parentCanvas.transform, true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            //Updates the objects position as we move around
            transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            //Sets the position to original
            transform.position = _startPosition;
            //Canvas needs to block raycasts again
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //sets us to the original parent
            transform.SetParent(_originalParent, true);

            //Creates a destination object
            IDragDestination<T> container = null;
            //if we are not over this current object
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //set the canvas as our destination
                container = _parentCanvas.GetComponent<IDragDestination<T>>();
            }
            else
            {
                //Get the container we are hovering over
                if (eventData.pointerEnter)
                {
                    container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
                }
            }

            //if we have a container object then drop our item into the new container.
            if (container != null)
            {
                DropItemIntoContainer(container);
            }


        }

        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            //if the destination is the same as the source
            if (object.ReferenceEquals(destination, _itemSource)) return;

            //Gets the IDragContainer of the destination
            var destinationContainer = destination as IDragContainer<T>;
            var sourceContainer = _itemSource as IDragContainer<T>;

            // Swap won't be possible
            //if the destination or the source are null
            //or the item is null
            //or the object of the source and destination are the same
                //then attempt a transfer
            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptItemTransfer(destination);
                return;
            }

            //if the conditions above are not met then attempt to swap the items
            AttemptSwap(destinationContainer, sourceContainer);
        }

        private void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
        {
            //Get the items and the number of the items at the destination and source
            int removedSourceNumber = source.GetNumber();
            T removedSourceItem = source.GetItem();
            int removedDestinationNumber = destination.GetNumber();
            T removedDestinationItem = destination.GetItem();

            //Remove the items from the source and destination 
            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            //Calculates the amount of items we need to take back 
            int sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            int destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            //if we need to do takebacks then add the items to the source 
            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            //if we need to do takeabacks then add the items to the destination
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            //Abandon this if we cannot swap
            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber ||
                removedSourceNumber == 0)
            {
                //Add the items back to the slots
                if (removedDestinationNumber > 0) destination.AddItems(removedDestinationItem, removedDestinationNumber);
                if (removedSourceNumber > 0) source.AddItems(removedSourceItem, removedSourceNumber);
                return;
            }

            // Do swaps
            if (removedDestinationNumber > 0) source.AddItems(removedDestinationItem, removedDestinationNumber);
            if (removedSourceNumber > 0) destination.AddItems(removedSourceItem, removedSourceNumber);
        }

        private bool AttemptItemTransfer(IDragDestination<T> destination)
        {
            T draggingItem = _itemSource.GetItem();
            int draggingNumber = _itemSource.GetNumber();

            int acceptable = destination.MaxAcceptable(draggingItem);
            int toTransfer = Mathf.Min(acceptable, draggingNumber);

            //if we have items to transfer
            if (toTransfer > 0)
            {
                //remove items from the source
                _itemSource.RemoveItems(toTransfer);

                //and add them to the destination
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }

            return true;
        }

        private int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
        {
            int takeBackNumber = 0;
            int destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                int sourceTakeBackAcceptable = removeSource.MaxAcceptable(removedItem);

                // Abort and reset
                if (sourceTakeBackAcceptable < takeBackNumber) return 0;
            }
            return takeBackNumber;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Get the players inventory and equipment
            Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            Equipment playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();

            //Get the slot ui 
            InventorySlotUI inventorySlot = GetComponentInParent<InventorySlotUI>();
            ActionSlotUI actionSlot = GetComponentInParent<ActionSlotUI>();

            //if this is an inventory slot
            if (inventorySlot)
            {
                int indexOfItem = inventorySlot._index;
                playerInventory.SelectItem(indexOfItem);
            }
            //if this is an action slot
            else if (actionSlot)
            {
                int indexOfItem = actionSlot._index;
                playerInventory.SelectItem(indexOfItem);
            }
            //if it is neither a inventory or a action slot then we are an equipment slot
            else
            {
                EquipmentSlotUI equipmentSlot = GetComponentInParent<EquipmentSlotUI>();
                EquipLocation location = equipmentSlot.EquipLocation;
                playerEquipment.Select(location);
            }
        }
    }
}