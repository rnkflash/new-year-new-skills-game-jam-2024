using System;
using System.Collections;
using UnityEngine;

namespace objects.actions
{
    public class ActionsController : MonoBehaviour
    {
        public Action action;
        public Actor performer;
        public Actor target;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            action.ExecuteAction(performer, target);
        }
    }
}