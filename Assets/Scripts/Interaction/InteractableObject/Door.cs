using System.Collections;
using Interaction;
using UnityEngine;

namespace Scenes.Interaction.InteractableObject
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private global::Interaction.Interaction.EInteractionType objInteractType = global::Interaction.Interaction.EInteractionType.Door;
        [SerializeField] private GameObject transformRoot;
        [SerializeField] private Vector3 actiavateVector3 = new Vector3(0,0,1.5f);
        [SerializeField] private float activateDuration = 1;
        [SerializeField] private float resetTime = 4;
        
        private float currentTime = 0;
        private bool isTrigger = false;
        private bool isActivated;
        Vector3 closePosition;
        
        void Start()
        {
            // Sets the first position of the door as it's closed position.
            closePosition = transform.position;
        }
        
        private void Update()
        {
            if (isTrigger == true)
            {
                if (currentTime <= resetTime)
                {
                    currentTime += Time.deltaTime;
                }
                else if (currentTime >= resetTime)
                {
                    ResetSelf();
                }
            }
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
                Debug.Log($"Trigger By {other.gameObject.name}");
                ActivateDoor();
                other.GetComponent<IInteractable>().TakeInteractEffect(objInteractType,this.gameObject);
                isTrigger = true;
            }
        }

        public void ExampleMethodInDoorCScript()
        {
            Debug.Log("Door Interacted");
        }
        
        //----------------------------------------------------------
        private void ActivateDoor()
        {
            var localTransform = transformRoot.transform;
            var translation = new Vector3(0, 0, 0);
            localTransform.Translate(translation, localTransform);
            StopAllCoroutines();
            if (!isActivated)
            {
                Vector3 openPosition = closePosition + actiavateVector3; //Vector3.up * actiavateHeight;
                StartCoroutine(MoveDoor(openPosition));
            }
            else
            {
                StartCoroutine(MoveDoor(closePosition));
            }
            
            isActivated = !isActivated;
            Debug.Log("Trap Activation : " + isActivated);
        }

        IEnumerator MoveDoor(Vector3 targetPosition)
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

        private void ResetSelf()
        {
            ActivateDoor();
            currentTime = 0;
            isTrigger = false;
        }
        
        // ----------------------------------------------------------
        
        public void OnInteracted()
        {
            //-- Implement this for Interact Object only
        }
        public void TakeInteractEffect(global::Interaction.Interaction.EInteractionType type, GameObject target)
        {
            //-- Implement this for Player Object only
        }
    }
}
