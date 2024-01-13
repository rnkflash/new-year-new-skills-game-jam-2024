using System;
using System.Collections;
using objects.fsm.actions.eat;
using UnityEngine;

namespace objects.actions
{
    public class ActionsController : MonoBehaviour
    {

        public EatAction eatAction;
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            eatAction.enabled = true;
        }
    }
}