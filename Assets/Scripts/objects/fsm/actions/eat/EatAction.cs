using objects.fsm.actionFSM;
using objects.fsm.actions.eat.states;
using UnityEngine;

namespace objects.fsm.actions.eat
{
    public class EatAction: ActionFSM
    {
        public Actor target;
        public Actor performer;
        [HideInInspector] public Vector3 lastTargetPosition;
        
        public override void AddStates()
        {
            AddState<TakeFood>();
            AddState<BiteFood>();
            AddState<DropFood>();
            AddState<Blurp>();
            
            SetInitialState<TakeFood>();
        }
    }
}