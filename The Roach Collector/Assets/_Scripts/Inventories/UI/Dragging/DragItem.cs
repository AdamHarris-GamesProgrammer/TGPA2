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
            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                return;
            }

            AttemptSwap(destinationContainer, sourceContainer);
        }

        private void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
        {
            // Provisionally remove item from both sides. 
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            var removedDestinationNumber = destination.GetNumber();
            var removedDestinationItem = destination.GetItem();

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            // Do take backs (if needed)
            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            // Abort if we can't do a successful swap
            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber ||
                removedSourceNumber == 0)
            {
                if (removedDestinationNumber > 0)
                {
                    destination.AddItems(removedDestinationItem, removedDestinationNumber);
                }
                if (removedSourceNumber > 0)
                {
                    source.AddItems(removedSourceItem, removedSourceNumber);
                }
                return;
            }

            // Do swaps
            if (removedDestinationNumber > 0)
            {
                source.AddItems(removedDestinationItem, removedDestinationNumber);
            }
            if (removedSourceNumber > 0)
            {
                destination.AddItems(removedSourceItem, removedSourceNumber);
            }
        }

        private bool AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = _itemSource.GetItem();
            var draggingNumber = _itemSource.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                _itemSource.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }

            return true;
        }

        private int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = removeSource.MaxAcceptable(removedItem);

                // Abort and reset
                if (sourceTakeBackAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }
            return takeBackNumber;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            Equipment playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();

            InventorySlotUI inventorySlot = GetComponentInParent<InventorySlotUI>();
            ActionSlotUI actionSlot = GetComponentInParent<ActionSlotUI>();

            if (inventorySlot)
            {
                int indexOfItem = inventorySlot._index;
                playerInventory.SelectItem(indexOfItem);
            }
            else if (actionSlot)
            {
                int indexOfItem = actionSlot._index;
                playerInventory.SelectItem(indexOfItem);
            }
            else
            {
                EquipmentSlotUI equipmentSlot = GetComponentInParent<EquipmentSlotUI>();
                EquipLocation location = equipmentSlot.EquipLocation;
                playerEquipment.Select(location);
            }
        }
    }
}