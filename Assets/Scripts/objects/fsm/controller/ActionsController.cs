using System;
using System.Linq;
using objects.fsm.actions.eat;
using objects.interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace objects.fsm.controller
{
    public class ActionsController : MonoBehaviour
    {
        private Actor performer;

        private GameObject currentAction;

        private void Awake()
        {
            performer = GetComponentInParent<Actor>();
        }

        public void DoAction()
        {
            if (performer.isActionBeingPerformedOn || performer.isPerformingAction) return;
            
            var eatablesInRange = performer.ReadSensor<IEatable>(0);
            var notBusy = eatablesInRange.FindAll(x => !((Actor)x).isActionBeingPerformedOn && !((Actor)x).isPerformingAction);
            if (notBusy.Count > 0)
            {
                Eat(notBusy[Random.Range(0, notBusy.Count)]);
            }
        }

        private void OnActionComplete()
        {
            var component = currentAction.GetComponent<EatAction>();
            component.onFinish -= OnActionComplete;
            component.performer.SetPerformingAction(false);
            component.target.SetActionBeingPerformedOn(false);
            Destroy(currentAction);
            currentAction = null;

        }

        private void Eat(IEatable target)
        {
            var targetActor = (Actor)target;
            targetActor.SetActionBeingPerformedOn(true);
            
            performer.SetPerformingAction(true);
            
            var newGameObject = new GameObject("EatAction");
            
            var component = newGameObject.AddComponent<EatAction>();
            component.performer = performer;
            component.target = (Actor)target;

            component.onFinish += OnActionComplete;
            newGameObject.transform.SetParent(transform);

            currentAction = newGameObject;
        }
    }
}