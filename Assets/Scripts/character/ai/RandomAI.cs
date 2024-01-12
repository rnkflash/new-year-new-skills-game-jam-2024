using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace character.ai
{
    public class RandomAI: MonoBehaviour
    {
        public CavemanController caveman;
        public float radius = 10.0f;
        public float minIdleTime = 0.5f;
        public float maxIdleTime = 1.0f;
        public float stoppingDistance = 0.1f;
        public float maxRunningTime = 5.0f;

        private enum State
        {
            idle, running
        }

        private State state = State.idle;
        private float currentStateTime;
        private float currentMaxTime;
        private Vector3 currentDestination;

        private void Start()
        {
            SetState(State.idle);
        }

        private void Update()
        {
            currentStateTime += Time.deltaTime;
            
            if (state == State.idle)
            {
                if (currentStateTime > currentMaxTime)
                {
                    SetState(State.running);
                }
            }
            else
            {
                var pointReached = Vector3.Distance(caveman.transform.position, currentDestination) <= stoppingDistance;
                var noPathAndStoppedMoving = !caveman.HasPath();
                var timeLimitExceeded = currentStateTime > currentMaxTime;
                if (pointReached || noPathAndStoppedMoving || timeLimitExceeded)
                {
                    SetState(State.idle);
                }
            }
        }

        private void SetState(State newState)
        {
            currentStateTime = 0;
            state = newState;
            if (state == State.idle)
            {
                currentMaxTime = Random.Range(minIdleTime, maxIdleTime);
                caveman.Stop();
            }
            else
            {
                var randomPoint = pickRandomPoint(new Vector2(caveman.transform.position.x, caveman.transform.position.z), radius);
                currentDestination = new Vector3(randomPoint.x, caveman.transform.position.y, randomPoint.y);
                caveman.SetDestination(currentDestination);
                
                currentMaxTime = maxRunningTime;
            }
        }

        private Vector2 pickRandomPoint(Vector2 center, float circleRadius)
        {
            return center + Random.insideUnitCircle * circleRadius;
        }
        
    }
}