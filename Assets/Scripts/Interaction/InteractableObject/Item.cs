using System;
using System.Collections;
using Interaction;
using UnityEngine;

namespace Scenes.Interaction.InteractableObject
{
    public class Item : MonoBehaviour, IInteractable
    {
        public struct ItemProperties
        {
            public enum EItemType
            {
                Heal,
                Poison,
                Other,
                KeyItem
            }
            
            public enum EItemName //Preserve Item Name For easily manage inventory
            {
                //Heal
                NanoMedic,
                //Poison
                RushSteel,
                //Other
                Gear,
                RedGear,
                BlueGear,
                //KeyItem
                RedGem,
                BlueGem,
                GreenGem
            }

            public EItemName Name;
            public EItemType Type;
            public int Value;
        }
        
        [SerializeField] private global::Interaction.Interaction.EInteractionType objInteractType = global::Interaction.Interaction.EInteractionType.Item;
        [Header("Item Info")]
        [SerializeField] private ItemProperties.EItemName itemName;
        [SerializeField] private ItemProperties.EItemType itemType;
        [SerializeField] private int itemEffectValue;
        
        [Header("Setup")]
        [SerializeField] private GameObject interactPrompt;
        [SerializeField] private float interactableTime = 2;
        [SerializeField] private ItemProperties _itemProperties;
        
        [Header("Optional animate when trigger")]
        [SerializeField] private GameObject transformRoot;
        [SerializeField] private Vector3 actiavateVector3 = new Vector3(0,0,0);
        [SerializeField] private float activateDuration = 1;

        private float currentTime = 0;
        private bool isTrigger = false;
        private bool isActivated;
        Vector3 closePosition;

        public void InitItem()
        {
            _itemProperties.Name = itemName;
            _itemProperties.Type = itemType;
            _itemProperties.Value = itemEffectValue;
        }


        void Start()
        {
            // Sets the first position of the door as it's closed position.
            closePosition = transform.position;
            interactPrompt.SetActive(false);
            InitItem();
        }
        
        private void Update()
        {
            if (isTrigger == true)
            {
                if (currentTime <= interactableTime)
                {
                    currentTime += Time.deltaTime;
                }
                else if (currentTime >= interactableTime)
                {
                    SetToInteractable(true);
                }
            }
        }

        public ItemProperties GetItemProperties()
        {
            return _itemProperties;
        }
        
        public ItemProperties.EItemName GetItemName()
        {
            return _itemProperties.Name;
        }
        
        public ItemProperties.EItemType GetItemType()
        {
            return _itemProperties.Type;
        }
        
        public int GetItemEffectValue()
        {
            return _itemProperties.Value;
        }
        
        public global::Interaction.Interaction.EInteractionType GetObjType()
        {
            return objInteractType;
        }

        //----------------------------------------------------------

        public void OnTrigger(GameObject other)
        {
            if (other.CompareTag("PlayerInteractor"))
            {
                if (isTrigger == false)
                {
                    Debug.Log($"Trigger By {other.gameObject.name}");
                    other.GetComponent<IInteractable>().TakeInteractEffect(objInteractType,this.gameObject);
                    isTrigger = true;
                }
                else
                {
                    SetToInteractable(false);
                }
            }
        }

        //----------------------------------------------------------
        private void ActivateAnimate()
        {
            var localTransform = transformRoot.transform;
            var translation = new Vector3(0, 0, 0);
            localTransform.Translate(translation, localTransform);
            StopAllCoroutines();
            if (!isActivated)
            {
                Vector3 openPosition = closePosition + actiavateVector3; //Vector3.up * actiavateHeight;
                StartCoroutine(MoveObj(openPosition));
            }
            else
            {
                StartCoroutine(MoveObj(closePosition));
            }
            
            isActivated = !isActivated;
            Debug.Log("Trap Activation : " + isActivated);
        }

        IEnumerator MoveObj(Vector3 targetPosition)
        {
            float timeElapsed = 0;
            Vector3 startPosition = transformRoot.transform.position;
            while (timeElapsed < activateDuration)
            {
                transformRoot.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / activateDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transformRoot.transform.position = targetPosition;
        }

        private void SetToInteractable(bool state)
        {
            if (state == true)
            {
                //ActivateAnimate();
                interactPrompt.SetActive(true);
            }
            else
            {
                //ActivateAnimate();
                interactPrompt.SetActive(false);
                currentTime = 0;
                isTrigger = false;
            }
            
        }

        // ----------------------------------------------------------
        
        public void OnInteracted()
        {
            //-- Implement this for Interact Object only
            Destroy(this.gameObject);
        }
        public void TakeInteractEffect(global::Interaction.Interaction.EInteractionType type, GameObject target)
        {
            //-- Implement this for Player Object only
        }
    }
}
