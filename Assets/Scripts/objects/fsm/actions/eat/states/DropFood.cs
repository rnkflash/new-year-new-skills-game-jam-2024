using boot;
using DG.Tweening;
using objects.interfaces;

namespace objects.fsm.actions.eat.states
{
    public class DropFood: EatState
    {
        private bool foodIsJumping;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            Parent.target.transform.SetParent(ActorsManager.instance.defaultActorsHolder);
            
            foodIsJumping = true;
            Parent.target.transform.DOJump(Parent.lastTargetPosition, 3.0f, 1, 0.5f).OnComplete(() =>
                {
                    foodIsJumping = false;
                }
            );
            
            ((IEater)Parent.performer).DropFood();
        }

        private void Update()
        {
            if (foodIsJumping) return;
            
            Fsm.ChangeState<Blurp>();
        }

        public override void OnExit()
        {
            base.OnExit();

            Parent.target.transform.DOKill();
            
            ((IEatable)Parent.target).FinishBeingEating();
            ((IEater)Parent.performer).FinishEating();

            Fsm.Finish();
        }
    }
}