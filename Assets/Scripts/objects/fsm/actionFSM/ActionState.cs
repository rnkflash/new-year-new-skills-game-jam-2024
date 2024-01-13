using System;
using objects.fsm.actionFSM.interfaces;
using UnityEngine;

namespace objects.fsm.actionFSM
{
    [Serializable]
    public abstract class ActionState : MonoBehaviour, IState
    {
        public string StateName { get; internal set; }

        public IFSM Fsm { get; internal set; }
        public IFSM SuperFsm { get; internal set; }

        public virtual void Initialize()
        {
            //if no name hase been specified set the name of this instance to the the
            if (String.IsNullOrEmpty(StateName))
                StateName = GetType().Name;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        public virtual string GetStateName()
        {
            return StateName;
        }
    }
}