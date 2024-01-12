using objects.interfaces;
using UnityEngine;

namespace objects
{
    public class Fruit : Actor, IEatable, IDamageable, ICarryable
    {
        public Collider collider;
        
        public void StartEating()
        {
            collider.enabled = false;
        }

        public int Eat()
        {
            return 0;
        }

        public void FinishEating()
        {
            collider.enabled = true;
        }

        public void Damage(int amount)
        {
            
        }

        public void OnStartCarry()
        {
            collider.enabled = false;
        }

        public void OnStopCarry()
        {
            collider.enabled = true;
        }

        public override bool IsAlive()
        {
            return true;
        }
    }
}