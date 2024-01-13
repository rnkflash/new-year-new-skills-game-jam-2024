using RobustFSM.Interfaces;

namespace objects.fsm.actionFSM.interfaces
{
    public interface IHState : IState
    {
        public IFSM Fsm { get; set; }
    }
}
