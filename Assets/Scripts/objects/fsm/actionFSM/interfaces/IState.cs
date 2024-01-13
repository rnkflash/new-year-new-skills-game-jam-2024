namespace objects.fsm.actionFSM.interfaces
{
    public interface IState
    {
        string StateName { get; }

        IFSM Fsm { get; }
        IFSM SuperFsm { get; }

        string GetStateName();

        void Initialize();
        void OnEnter();
        void OnExit();
    }
}