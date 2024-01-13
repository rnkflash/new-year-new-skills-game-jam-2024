using System;
using System.Collections.Generic;
using objects.components;
using objects.interfaces;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AI;

namespace objects
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(AnimatedNavMeshAgentComponent))]
    public class Caveman : Actor, IDamageable, IEater, IEatable
    {
        public Transform carryHolder;
        public SkeletonAnimation skeletonAnimation;

        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string idleAnimation;

        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string idleCarryAnimation;

        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string dropAnimation;

        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string eatAnimation;

        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string pickupAnimation;

        [SpineAnimation(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string damageAnimation;


        private AnimatedNavMeshAgentComponent animatedNavMeshAgentComponent;
        private Collider collider;
        private Rigidbody rigidBody;

        public ActorSensor interactSensor;
        public ActorSensor visibilitySensor;
        
        private void Awake()
        {
            collider = GetComponent<Collider>();
            rigidBody = GetComponent<Rigidbody>();
            animatedNavMeshAgentComponent = GetComponent<AnimatedNavMeshAgentComponent>();
        }

        public void Damage(int amount)
        {
            
        }

        public override bool IsAlive()
        {
            return true;
        }

        public void StartEating(IEatable food)
        {
            var foodActor = (Actor)food;
            animatedNavMeshAgentComponent.TurnFace(foodActor.transform.position);
            animatedNavMeshAgentComponent.Activate(false);
            
            skeletonAnimation.state.ClearTrack(0);
            skeletonAnimation.state.SetAnimation(0, pickupAnimation, false);
            skeletonAnimation.state.AddAnimation(0, idleCarryAnimation, true, 0);
        }

        public int Eat()
        {
            skeletonAnimation.state.SetAnimation(1, eatAnimation, false);
            return 0;
        }

        public void DropFood()
        {
            skeletonAnimation.state.ClearTrack(0);
            skeletonAnimation.state.SetAnimation(0, dropAnimation, false);
            skeletonAnimation.state.AddAnimation(0, idleAnimation, true, 0);
        }

        public void FinishEating()
        {
            animatedNavMeshAgentComponent.Activate(true);
        }

        public void Blurp()
        {
            Debug.Log("Blurp");
        }

        public Transform GetFoodHolder()
        {
            return carryHolder;
        }

        public void StartBeingEaten()
        {
            animatedNavMeshAgentComponent.Activate(false);
            ActivateRigidBody(false);
        }

        public int OnBitten()
        {
            skeletonAnimation.state.SetAnimation(0, damageAnimation, false);
            return 0;
        }

        public void FinishBeingEating()
        {
            ActivateRigidBody(true);
            animatedNavMeshAgentComponent.Activate(true);
        }
        
        private void ActivateRigidBody(bool yes)
        {
            collider.enabled = yes;
            rigidBody.isKinematic = !yes;
            rigidBody.useGravity = yes;
        }

        public override List<T> ReadSensor<T>(int sensorType) where T: class
        {
            return sensorType switch
            {
                0 => interactSensor.GetInRange<T>(),
                1 => visibilitySensor.GetInRange<T>(),
                _ => base.ReadSensor<T>(sensorType)
            };
        }
    }
}