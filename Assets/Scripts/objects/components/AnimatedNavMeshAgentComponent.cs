using System;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AI;

namespace objects.components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AnimatedNavMeshAgentComponent : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        private Quaternion initialRotation;
        public SkeletonAnimation skeletonAnimation;
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string idleAnimation;
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string walkAnimation;
        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string runAnimation;
        private string currentAnimation;
        
        private bool isMoving;

        private TrackEntry walkTrack;
        private TrackEntry runTrack;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            initialRotation = transform.rotation;
            Activate(true);
        }

        void Update()
        {
            transform.rotation = initialRotation;

            if (!isMoving)
            {
                if (agent.velocity != Vector3.zero)
                {
                    isMoving = true;
                }
            }
            else
            {
                if (agent.velocity == Vector3.zero)
                {
                    isMoving = false;
                    walkTrack.Alpha = 0.0f;
                    runTrack.Alpha = 0.0f;
                }
                else
                {
                    var actualSpeed = agent.velocity.magnitude;
                    if (actualSpeed < 3.0f)
                        walkTrack.Alpha = (Mathf.Clamp(agent.velocity.magnitude, 0.0f, 3.0f))/ 3.0f;
                    else
                        runTrack.Alpha = (Mathf.Clamp(agent.velocity.magnitude, 3.0f, 6.0f) - 3.0f)/ 3.0f;    
                }
            }
            
            SetFlip(agent.velocity.x);
        }

        public void SetDestination(Vector3 dest)
        {
            agent.SetDestination(dest);
        }

        private void SetFlip (float horizontal)
        {
            if (horizontal == 0) return;
            TurnFace(horizontal > 0);
        }

        public void TurnFace(bool right)
        {
            skeletonAnimation.Skeleton.ScaleX = right ? -1f : 1f;
        }

        public void TurnFace(Vector3 to)
        {
            var hor = to.x - transform.position.x;
            SetFlip(hor);
        }

        public bool HasPath()
        {
            return agent.hasPath;
        }

        public void Stop()
        {
            agent.SetDestination(transform.position);
        }

        public void Activate(bool enabled)
        {
            agent.enabled = enabled;
            this.enabled = enabled;

            if (enabled)
            {
                skeletonAnimation.state.SetAnimation(0, idleAnimation, true);
                walkTrack = skeletonAnimation.state.SetAnimation(1, walkAnimation, true);
                walkTrack.Alpha = 0.0f;    
                runTrack = skeletonAnimation.state.SetAnimation(2, runAnimation, true);
                runTrack.Alpha = 0.0f;
            }
        }
        
    }
}