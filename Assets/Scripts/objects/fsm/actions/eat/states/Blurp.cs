using objects.interfaces;

namespace objects.fsm.actions.eat.states
{
    public class Blurp: EatState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            ((IEater)Parent.performer).Blurp();
        }
    }
}