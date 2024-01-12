using System;
using UnityEngine;

namespace objects.actions
{
    public abstract class Action : MonoBehaviour
    {
        [HideInInspector] public bool IsExecuting = false;

        protected Actor performer;
        protected Actor victim;
        
        public void ExecuteAction(Actor from, Actor target = null)
        {
            performer = from;
            victim = target;
            
            IsExecuting = true;
        }

        protected virtual void ResetAction()
        {
            performer = null;
            victim = null;
            IsExecuting = false;
        }
    }
}