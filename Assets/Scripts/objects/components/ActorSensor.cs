using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace objects.components
{
    
    [RequireComponent(typeof(SphereCollider))]
    public class ActorSensor: MonoBehaviour
    {
        private SphereCollider collider;

        private List<Actor> actors = new ();

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter " + other);
            if (other.TryGetComponent(out Actor actor))
            {
                actors.Add(actor);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("OnTriggerExit " + other);
            if (other.TryGetComponent(out Actor actor))
            {
                actors.RemoveAll(x => x == actor);
            }
        }

        public List<T> GetInRange<T>() where T : class
        {
            return actors.OfType<T>().ToList();
        }

    }
}