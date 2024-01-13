using DG.Tweening;
using objects.interfaces;

namespace objects.fsm.actions.eat.states
{
    public class BiteFood: EatState
    {
        private bool bitingFood;
        
        public override void OnEnter()
        {
            base.OnEnter();

            bitingFood = true;
            
            Parent.target.transform.DOShakePosition(1.0f, 0.5f).OnComplete(() =>
            {
                bitingFood = false;
            });
            
            ((IEatable)Parent.target).OnBitten();
            ((IEater)Parent.performer).Eat();
        }

        private void Update()
        {
            if (bitingFood) return;

            Fsm.ChangeState<DropFood>();
        }

        public override void OnExit()
        {
            base.OnExit();
            Parent.target.transform.DOKill();
        }
    }
}