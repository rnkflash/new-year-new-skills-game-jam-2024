using System;
using UnityEngine;
using UnityEngine.Events;

namespace boot
{
    public class DayNightManager : MonoBehaviour
    {
        public static DayNightManager instance;

        public Animator animator;
        
        [Range(0.0f, 1.0f)] public float progress = 0.5f;
        public float animationSpeed = 0.1f;
        public bool isAnimated;

        public UnityEvent onProgressChange;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("DayNightProgress"))
                progress = PlayerPrefs.GetFloat("DayNightProgress");
            
            animator.SetFloat("progress", progress);
            animator.SetFloat("animationSpeed", animationSpeed);
            SetAnimated(isAnimated);
            
            onProgressChange?.Invoke();
        }

        private void Update()
        {
            
            var newProgress = animator.GetFloat("progress");
            if (Math.Abs(progress - newProgress) > 0.1)
            {
                progress = newProgress;
                onProgressChange?.Invoke();
            }

        }

        public void SetProgress(float value)
        {
            progress = value;
            animator.SetFloat("progress", progress);
        }

        public void SetAnimated(bool toggleIsOn)
        {
            isAnimated = toggleIsOn;
            if (isAnimated)
                animator.Play("DayNightCycle", 0, progress);
            else
                animator.PlayInFixedTime("DayNightCycle", 0, progress);
        }
    }
}