using System.Collections.Generic;
using UnityEngine;

namespace objects
{
    public abstract class Actor : MonoBehaviour
    {
        public abstract bool IsAlive();
        
        public bool isPerformingAction;

        public void SetPerformingAction(bool value)
        {
            isPerformingAction = value;
        }

        public bool isActionBeingPerformedOn;

        public void SetActionBeingPerformedOn(bool value)
        {
            isActionBeingPerformedOn = value;
        }

        
        public virtual List<T> ReadSensor<T>(int sensorType) where T: class
        {
            return new();
        }
    }
}