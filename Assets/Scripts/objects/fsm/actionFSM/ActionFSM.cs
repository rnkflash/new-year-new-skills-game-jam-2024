using System;
using System.Collections.Generic;
using objects.fsm.actionFSM.interfaces;
using UnityEditor;
using UnityEngine;

namespace objects.fsm.actionFSM
{
    public abstract class ActionFSM : MonoBehaviour, IFSM
    {
        /// <summary>
        /// A reference to machine name of this instance
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// A reference to the current state of this FSM
        /// </summary>
        protected ActionState CurrentState { get; set; }

        /// <summary>
        /// A reference to the initial state of this FSM
        /// </summary>
        protected ActionState InitialState { get; set; }

        /// <summary>
        /// A reference to the next state of this FSM
        /// </summary>
        protected ActionState NextState { get; set; }

        /// <summary>
        /// A reference to the previous state of this FSM
        /// </summary>
        protected ActionState PreviousState { get; set; }

        /// <summary>
        /// A reference to the states dictionary of this instance
        /// </summary>
        private Dictionary<Type, ActionState> _states = new ();
        public Dictionary<Type, ActionState> States { get => _states; set => _states = value; }


        #region FSM Initialization Methods

        /// <summary>
        /// REQUIRES IMPL
        /// Adds states to the machine with calls to AddState<>()
        ///     
        /// When all states have been added set the initial state with 
        /// a call toSetInitialState<>()
        /// </summary>
        public abstract void AddStates();

        /// <summary>
        /// Add the state to the FSM
        /// </summary>
        /// <typeparam name="T">state type</typeparam>
        public void AddState<T>() where T: ActionState, new()
        {
            if (!ContainsState<T>())
            {
                ActionState monoState = gameObject.AddComponent<T>();
                monoState.enabled = false;

                monoState.Fsm = this;
                monoState.SuperFsm = this;
                monoState.Initialize();

                States.Add(typeof(T), monoState);
            }
        }

        /// <summary>
        /// Initializes the FSM
        /// </summary>
        public virtual void Initialize()
        {
            //if no name hase been specified set the name of this instance to the the
            if (String.IsNullOrEmpty(MachineName))
                MachineName = GetType().Name;

            //add the states
            AddStates();

            //set the current state to be the initial state
            CurrentState = InitialState;

            //throw an error if we do not have an initial state
            if (CurrentState == null)
                throw new Exception("\n" + name + "Initial state not specified.\tSpecify the initial state inside the AddStates()!!!\n");

            //initialize every state
            foreach (KeyValuePair<Type, ActionState> pair in States)
            {
                //set the super machine and initialize the state
                //pair.Value.Machine = this;
                //pair.Value.SuperMachine = this;
                //pair.Value.Initialize();
            }

            //change to the current state
            CurrentState.enabled = true;
            CurrentState.OnEnter();
        }

        /// <summary>
        /// Sets the initial state for this FSM
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SetInitialState<T>() where T: ActionState
        {
            InitialState = States[typeof(T)];
        }

        #endregion

        #region MonoBehaviour Methods

        /// <summary>
        /// Raises the start event
        /// </summary>
        public virtual void Start()
        {
            // initialize stuff
            Initialize();
        }

        #endregion

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (CurrentState != null)
            {
                //set the print text
                string printText = MachineName + "\n" + CurrentState.GetStateName();

                //render the label

                Handles.Label(transform.position, printText);

            }
        }
#endif
        
        #region FSM Methods

        /// <summary>
        /// Triggers a state transition of the FSM to the specified state
        /// </summary>
        /// <typeparam name="T">the next state</typeparam>
        public void ChangeState<T>() where T: ActionState
        {
            ChangeState(typeof(T));
        }

        /// <summary>
        /// Triggers a state transition of the FSM to the specified state
        /// </summary>
        /// <param name="t"></param>
        private void ChangeState(Type t)
        {
            try
            {
                //cache the correct states
                PreviousState = CurrentState;
                NextState = States[t];

                //exit the current state
                CurrentState.OnExit();
                CurrentState.enabled = false;
                CurrentState = NextState;
                NextState = null;

                //enter the current state
                CurrentState.enabled = true;
                CurrentState.OnEnter();
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("\n" + name + " could not be found in the state machine" + e.Message);
            }
        }

        /// <summary>
        /// Checks whether this FSM contains the passed state
        /// </summary>
        /// <typeparam name="T">the state type</typeparam>
        /// <returns><c>true</c>, if state is such type is available else <c>false</c></returns>
        public bool ContainsState<T>() where T: ActionState
        {
            return States.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Checks whether the current state in the FSM matches the passed state
        /// </summary>
        /// <typeparam name="T">the state type</typeparam>
        /// <returns><c>true</c>, if the passed state matches the current state<c>false</c></returns>
        public bool IsCurrentState<T>() where T: ActionState
        {
            return (CurrentState.GetType() == typeof(T)) ? true : false;
        }

        /// <summary>
        /// Checks whether the previous state in the FSM matches the passed state
        /// </summary>
        /// <typeparam name="T">the state type</typeparam>
        /// <returns><c>true</c>, if the passed state matches the previous state<c>false</c></returns>
        public bool IsPreviousState<T>() where T: ActionState
        {
            return (PreviousState.GetType() == typeof(T)) ? true : false;
        }

        /// <summary>
        /// Returns the current state
        /// </summary>
        /// <typeparam name="T">the state type</typeparam>
        /// <returns>the current state</returns>
        public T GetCurrentState<T>() where T: ActionState
        {
            return (T)CurrentState;
        }

        /// <summary>
        /// Returns the previous state
        /// </summary>
        /// <typeparam name="T">the state type</typeparam>
        /// <returns>the previous state</returns>
        public T GetPreviousState<T>() where T: ActionState
        {
            return (T)PreviousState;
        }

        /// <summary>
        /// Retrieves the specified state from the FSM
        /// </summary>
        /// <typeparam name="T">the state type</typeparam>
        /// <returns>the previous state</returns>
        public T GetState<T>() where T: ActionState
        {
            return (T)States[typeof(T)];
        }

        /// <summary>
        /// Triggers the FSM to go to the previous state
        /// </summary>
        public void GoToPreviousState()
        {
            ChangeState(PreviousState.GetType());
        }

        /// <summary>
        /// Removes the passed state from the state machine
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveState<T>() where T: ActionState
        {
            Type t = typeof(T);
            if(States.ContainsKey(t))
                States.Remove(t);
        }

        /// <summary>
        /// Removes all states in the FSM
        /// </summary>
        public void RemoveAllStates()
        {
            States.Clear();
        }
        
        #endregion

        public Action onFinish;
        
        public void Finish()
        {
            onFinish?.Invoke();
        }

    }
}