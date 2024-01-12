using boot;
using DG.Tweening;
using objects.interfaces;
using UnityEngine;

namespace objects.actions
{
    public class EatAction : Action
    {
        private enum StateOfState
        {
            start, continuing, finish, finished
        }

        private enum State
        {
            takeFood, bite, blurp, drop
        }
        
        private State state = State.takeFood;
        private StateOfState stateOfState = StateOfState.start;

        public Transform takeFoodHolder;

        private bool foodIsJumping;
        private bool foodIsBiten;

        private float blurpingTime;

        private Vector3 lastPosition;

        protected override void ResetAction()
        {
            base.ResetAction();
            
            state = State.takeFood;
            stateOfState = StateOfState.start;
            foodIsJumping = false;
            foodIsBiten = false;
            blurpingTime = 0.0f;
        }

        private void Update()
        {
            if (!IsExecuting) return;

            switch (stateOfState)
            {
                case StateOfState.start:
                    StartState();
                    break;
                case StateOfState.continuing:
                    UpdateState();
                    break;
                case StateOfState.finish:
                    FinishState();
                    break;
                case StateOfState.finished:
                    ResetAction();
                    break;
            }
        }

        private void StartState()
        {
            switch (state)
            {
                case State.takeFood:
                {
                    ((IEatable)victim).StartEating();
                    ((IEater)performer).StartEating();

                    foodIsJumping = true;

                    lastPosition = victim.transform.localPosition; 
                    victim.transform.SetParent(takeFoodHolder.transform);
                    victim.transform.DOLocalJump(Vector3.zero, 1.0f, 1, 0.5f).OnComplete(() =>
                        {
                            foodIsJumping = false;
                        }
                    );
                    break;
                }
                case State.bite:
                    foodIsBiten = true;
                    victim.transform.DOShakePosition(0.3f).OnComplete(() =>
                    {
                        foodIsBiten = false;
                    });
                    break;
                case State.blurp:

                    blurpingTime = 1.0f;
                    
                    break;
                
                case State.drop:
                    foodIsJumping = true;
                    victim.transform.DOLocalJump(lastPosition, 1.0f, 1, 0.5f).OnComplete(() =>
                        {
                            foodIsJumping = false;
                        }
                    );
                    break;
            }

            stateOfState = StateOfState.continuing;
        }

        private void UpdateState()
        {
            var nextStateOfState = false;
            switch (state)
            {
                case State.takeFood:
                    if (foodIsJumping) return;
                    nextStateOfState = true;
                    break;
                case State.bite:
                    if (foodIsBiten) return;
                    nextStateOfState = true;
                    break;
                case State.blurp:
                    blurpingTime -= Time.deltaTime;
                    if (blurpingTime <= 0)
                        nextStateOfState = true;
                    break;
                
                case State.drop:
                    if (foodIsJumping) return;
                    nextStateOfState = true;
                    break;
            }
            
            if (nextStateOfState)
                stateOfState = StateOfState.finish;
        }

        private void FinishState()
        {
            switch (state)
            {
                case State.takeFood:
                    state = State.bite;
                    break;
                case State.bite:
                {
                    ((IEatable)victim).Eat();
                    ((IEater)performer).Eat();

                    state = State.blurp;
                    break;
                }
                case State.blurp:
                    state = State.drop;
                    break;
                
                case State.drop:
                    victim.transform.SetParent(ActorsManager.instance.defaultActorsHolder);
                    ((IEatable)victim).FinishEating();
                    ((IEater)performer).FinishEating();
                    stateOfState = StateOfState.finished;
                    break;
            }

            stateOfState = StateOfState.start;
        }
    }
}