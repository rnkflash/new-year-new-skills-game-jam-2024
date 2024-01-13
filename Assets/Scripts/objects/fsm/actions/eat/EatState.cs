using objects.fsm.actionFSM;

namespace objects.fsm.actions.eat
{
    public class EatState : ActionState
    {
        public EatAction Parent
        {
            get
            {
                return (EatAction)Fsm;
            }
        }
    }
}