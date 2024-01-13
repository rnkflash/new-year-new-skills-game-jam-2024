namespace objects.fsm.actionFSM.interfaces
{
    public interface IFSM
    {
        string MachineName { get; set; }


        bool ContainsState<T>() where T : ActionState;

        bool IsCurrentState<T>() where T : ActionState;

        bool IsPreviousState<T>() where T : ActionState;

        T GetCurrentState<T>() where T : ActionState;

        T GetPreviousState<T>() where T : ActionState;

        T GetState<T>() where T : ActionState;

        void AddState<T>() where T : ActionState, new();

        void ChangeState<T>() where T : ActionState;

        void RemoveState<T>() where T : ActionState;

        void SetInitialState<T>() where T : ActionState;

        void GoToPreviousState();

        void RemoveAllStates();

        void Finish();
    }
}