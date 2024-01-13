using objects.interfaces;
using UnityEngine;

namespace objects
{
    public class Fruit : Actor, IEatable, IDamageable, ICarryable
    {
        public Collider collider;
        public Rigidbody rigidBody;
        
        public void StartBeingEaten()
        {
            ActivateRigidBody(false);
        }

        public int OnBitten()
        {
            return 0;
        }

        public void FinishBeingEating()
        {
            ActivateRigidBody(true);
        }

        public void Damage(int amount)
        {
            
        }

        public void OnStartCarry()
        {
            ActivateRigidBody(false);
        }

        public void OnStopCarry()
        {
            ActivateRigidBody(true);
        }

        public override bool IsAlive()
        {
            return true;
        }

        private void ActivateRigidBody(bool yes)
        {
            collider.enabled = yes;
            rigidBody.isKinematic = !yes;
            rigidBody.useGravity = yes;
        }
    }
}