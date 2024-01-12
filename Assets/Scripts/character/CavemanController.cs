using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AI;

namespace character
{
    public class CavemanController : MonoBehaviour
    {
        public NavMeshAgent agent;
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

        private TrackEntry track;
        
        private void Start()
        {
            initialRotation = transform.rotation;
            SetMovingAnimation(false);
        }

        void Update()
        {
            transform.rotation = initialRotation;

            if (!isMoving)
            {
                if (agent.velocity != Vector3.zero)
                {
                    isMoving = true;
                    SetMovingAnimation(true);                    
                }
            }
            else
            {
                if (agent.velocity == Vector3.zero)
                {
                    isMoving = false;
                    SetMovingAnimation(false);                    
                }
            }

            if (track != null)
            {
                track.Alpha = agent.velocity.magnitude/agent.speed;
            }
            
            SetFlip(agent.velocity.x);
        }

        public void SetDestination(Vector3 dest)
        {
            agent.SetDestination(dest);
        }

        private void SetAnimation(string animation)
        {
            if (currentAnimation == animation) return;
            currentAnimation = animation;
            skeletonAnimation.AnimationState.SetAnimation(0, currentAnimation, true);
        }
        
        private void SetMovingAnimation(bool moving)
        {
            var state = skeletonAnimation.state;
            if (moving)
            {
                state.SetAnimation(0, walkAnimation, true);
                track = state.SetAnimation(1, runAnimation, true);
                track.Alpha = 0.0f;    
            }
            else
            {
                track = null;
                state.ClearTrack(1);
                state.SetAnimation(0, idleAnimation, true);
            }
        }
        
        private void SetFlip (float horizontal)
        {
            if (horizontal == 0) return;
            skeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
        }

        public bool HasPath()
        {
            return agent.hasPath;
        }

        public void Stop()
        {
            agent.SetDestination(transform.position);
        }
    }
}
