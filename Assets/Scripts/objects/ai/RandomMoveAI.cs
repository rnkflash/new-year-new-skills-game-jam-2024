using objects.components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace objects.ai
{
    public class RandomAI: MonoBehaviour
    {
        public AnimatedNavMeshAgentComponent navMeshComponent;
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
                var pointReached = Vector3.Distance(navMeshComponent.transform.position, currentDestination) <= stoppingDistance;
                var noPathAndStoppedMoving = !navMeshComponent.HasPath();
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
                navMeshComponent.Stop();
            }
            else
            {
                currentDestination = RandomNavmeshLocation(radius);
                
                navMeshComponent.SetDestination(currentDestination);
                navMeshComponent.agent.speed = Mathf.Clamp(Random.Range(0.0f, 6.0f), 1.0f, 6.0f);
                
                
                currentMaxTime = maxRunningTime;
            }
        }

        private Vector2 PickRandomPoint(Vector2 center, float circleRadius)
        {
            return center + Random.insideUnitCircle * circleRadius;
        }
        
        private Vector3 RandomNavmeshLocation(float radius) {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                finalPosition = hit.position;            
            }
            return finalPosition;
        }
        
    }
}