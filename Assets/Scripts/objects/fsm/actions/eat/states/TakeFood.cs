using DG.Tweening;
using objects.interfaces;
using UnityEngine;

namespace objects.fsm.actions.eat.states
{
    public class TakeFood: EatState
    {
        private bool foodIsJumping;
        
        public override void OnEnter()
        {
            base.OnEnter();

            var target = Parent.target;
            var performer = Parent.performer;
            
            ((IEatable)target).StartBeingEaten();
            ((IEater)performer).StartEating((IEatable)target);

            foodIsJumping = true;

            Parent.lastTargetPosition = target.transform.position;
            target.transform.SetParent(((IEater)performer).GetFoodHolder());
            target.transform.DOLocalJump(Vector3.zero, 2.0f, 1, 0.5f).OnComplete(() =>
                {
                    foodIsJumping = false;
                }
            );
        }

        private void Update()
        {
            if (foodIsJumping) return;
            
            Fsm.ChangeState<BiteFood>();
        }

        public override void OnExit()
        {
            base.OnExit();

            Parent.target.transform.DOKill();
        }
    }
}