using System.Collections;
using Interaction;
using UnityEngine;

namespace Scenes.Interaction.InteractableObject
{
    public class Trap : MonoBehaviour, IInteractable
    {
        [SerializeField] private global::Interaction.Interaction.EInteractionType objInteractType = global::Interaction.Interaction.EInteractionType.Trap;
        [SerializeField] private GameObject transformRoot;
        [SerializeField] private int trapDamage = 20;
        [SerializeField] private float actiavateHeight = 1.5f;
        [SerializeField] private float activateDuration = 1;
        [SerializeField] private float resetTime = 8;
        
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
        
        public int GetDamage()
        {
            return trapDamage;
        }
        
        //----------------------------------------------------------

        public void OnTrigger(GameObject other)
        {
            if (other.CompareTag("PlayerInteractor"))
            {
                Debug.Log($"Trigger By {other.gameObject.name}");
                ActivateTrap();
                other.GetComponent<IInteractable>().TakeInteractEffect(objInteractType,this.gameObject);
                isTrigger = true;
            }
        }

        //----------------------------------------------------------
        private void ActivateTrap()
        {
            var localTransform = transformRoot.transform;
            var translation = new Vector3(0, 0, 0);
            localTransform.Translate(translation, localTransform);
            StopAllCoroutines();
            if (!isActivated)
            {
                Vector3 openPosition = closePosition + Vector3.up * actiavateHeight;
                StartCoroutine(MoveTrap(openPosition));
            }
            else
            {
                StartCoroutine(MoveTrap(closePosition));
            }
            
            isActivated = !isActivated;
            Debug.Log("Trap Activation : " + isActivated);
        }

        IEnumerator MoveTrap(Vector3 targetPosition)
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
            ActivateTrap();
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
