using UnityEngine;

namespace Interaction
{
    public class Interaction
    {
        public enum EInteractionType
        {
            Player,
            Door,
            Puzzle,
            Trap,
            Item
        }
    }
    
    public interface IInteractable
    {
        Interaction.EInteractionType GetObjType();
        
        void OnTrigger(GameObject other);
        void OnInteracted();
        void TakeInteractEffect(Interaction.EInteractionType type, GameObject target);
    }
}
