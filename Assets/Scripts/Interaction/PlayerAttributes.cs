using UnityEngine;

namespace Interaction
{
    public class PlayerAttributes : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private int maxHealth = 100;

        private bool isDead;
        
        // Start is called before the first frame update
        void Start()
        {
            isDead = false;
            health = maxHealth;
        }

        public void DoDamage(int damage)
        {
            if (health <= 0)
            {
                isDead = true;
            }
            health -= damage;
        }

        public void Healing(int value)
        {
            if (isDead == false)
            {
                health += value;
            }
        }
    }
}
