using InventorySystem;
using Scenes.Interaction;
using Scenes.Interaction.InteractableObject;
using UnityEngine;

namespace Interaction
{
    public class PlayerInteractor : MonoBehaviour,IInteractable
    {
        [SerializeField] private PlayerAttributes playerAttributes;
        [SerializeField] private SimpleInventory simpleInventory;
        [SerializeField] private Item currentInteractObj;

        private void Start()
        {
            currentInteractObj = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            var triggerObj = other.GetComponent<IInteractable>();

            if (triggerObj != null)
            {
                triggerObj.OnTrigger(this.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (currentInteractObj != null)
            {
                currentInteractObj = null;
            }
        }

        public Interaction.EInteractionType GetObjType()
        {
            return Interaction.EInteractionType.Player;
        }

        // Call OnInteracted when Player Press some Input Key
        public void PickupCurrentItem()
        {
            // Then call OnInteracted
            if (simpleInventory != null)
            {
                if (currentInteractObj != null)
                {
                    OnPickupCheckItemEffect(currentInteractObj);
                    simpleInventory.PickUpItem(currentInteractObj);
                    currentInteractObj.OnInteracted();
                }
                else
                {
                    Debug.Log("No item to pickup");
                }
            }
            else
            {
                Debug.Log("Can not Fine Simple Inventory.");
            }
        }

        private void OnPickupCheckItemEffect(Item item)
        {
            var itemType = item.GetItemType();
            switch (itemType)
            {
                case Item.ItemProperties.EItemType.Heal:
                {
                    playerAttributes.Healing(item.GetItemEffectValue());
                    break;
                }

                case Item.ItemProperties.EItemType.Poison:
                {
                    playerAttributes.DoDamage(item.GetItemEffectValue());
                    break;
                }
            }
        }
        
        public void OnTrigger(GameObject other)
        {
            // This for other obj type Not Implement
            // throw new System.NotImplementedException();
        }
        
        
        public void OnInteracted()
        {
            // Not Implement for Interact Object only
        }

        public void TakeInteractEffect(Interaction.EInteractionType type, GameObject target)
        {
            // Other object will call this by OnTrigger of thier objects and send back Self Type and Self Game Object
            if(target != null)
            {
                switch (type) // This for Player Only
                {
                    case Interaction.EInteractionType.Trap:
                    {
                        var trapTarget = target.GetComponent<Trap>();
                        playerAttributes.DoDamage(trapTarget.GetDamage());
                        break;
                    }
                
                    case Interaction.EInteractionType.Door:
                    {
                        var doorTarget = target.GetComponent<Door>();
                        doorTarget.ExampleMethodInDoorCScript();
                        // Some effect after open door ?
                        break;
                    }
                
                    case Interaction.EInteractionType.Puzzle:
                    {
                        //target.SolvePuzzle();
                        break;
                    }
                
                    case Interaction.EInteractionType.Item:
                    {
                        currentInteractObj = target.GetComponent<Item>();
                        break;
                    }
                }
            }
        }
    }
}
