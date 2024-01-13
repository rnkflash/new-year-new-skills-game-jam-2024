using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace character
{
    public class FootStepSounds : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
        public string eventName;

        [Space]
        public AudioSource audioSource;
        public AudioClip[] audioClips;
        public float basePitch = 1f;
        public float randomPitchOffset = 0.1f;

        Spine.EventData eventData;

        private Camera cam;
        private Renderer renderer;

        void OnValidate () {
            if (skeletonAnimation == null) GetComponent<SkeletonAnimation>();
            if (audioSource == null) GetComponent<AudioSource>();
        }

        void Start () {
            if (audioSource == null) return;
            if (skeletonAnimation == null) return;
            skeletonAnimation.Initialize(false);
            if (!skeletonAnimation.valid) return;

            eventData = skeletonAnimation.Skeleton.Data.FindEvent(eventName);
            skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;

            cam = Camera.main;
            renderer = GetComponent<MeshRenderer>();
            if (renderer == null)
                renderer = GetComponent<SpriteRenderer>();
        }

        private void HandleAnimationStateEvent (TrackEntry trackEntry, Event e)
        {
            if (trackEntry.Alpha == 0)
                return;
            
            //bool eventMatch = string.Equals(e.Data.Name, eventName, System.StringComparison.Ordinal); // Testing recommendation: String compare.
            bool eventMatch = (eventData == e.Data); // Performance recommendation: Match cached reference instead of string.
            if (eventMatch)
            {
                var walkTrack = skeletonAnimation.AnimationState.Tracks.Items[1];
                var runTrack = skeletonAnimation.AnimationState.Tracks.Items[2];

                var currentMaxAlpha = walkTrack.Alpha > runTrack.Alpha ? walkTrack.Alpha : runTrack.Alpha;
                
                if (trackEntry.Alpha >= currentMaxAlpha)
                    Play(trackEntry.Alpha);
            }
        }

        public void Play (float volume) {
            audioSource.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset);
            if (IsObjectVisible(cam, renderer))
                audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], volume);
        }
        
        private bool IsObjectVisible(Camera cam, Renderer renderer)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), renderer.bounds);
        }
    }
}