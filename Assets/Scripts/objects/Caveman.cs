using objects.interfaces;
using Spine.Unity;
using UnityEngine;

namespace objects
{
    public class Caveman : Actor, IDamageable, IEater
    {
        public SkeletonAnimation skeletonAnimation;
        
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string idleAnimation;
        
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string idleCarryAnimation;
        
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string carryAnimation;
        
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string eatAnimation;
        
        public void Damage(int amount)
        {
            
        }

        public override bool IsAlive()
        {
            return true;
        }

        public void StartEating()
        {
            skeletonAnimation.state.SetAnimation(0, carryAnimation, false);
            skeletonAnimation.state.AddAnimation(0, idleCarryAnimation, true, 0);
        }

        public int Eat()
        {
            skeletonAnimation.state.SetAnimation(0, eatAnimation, false);
            skeletonAnimation.state.AddAnimation(0, idleAnimation, true, 0);
            return 0;
        }

        public void FinishEating()
        {
            
        }
    }
}