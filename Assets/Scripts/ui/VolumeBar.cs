using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ui
{
    public class VolumeBar : MonoBehaviour
    {
        public Slider slider;
        public AudioMixer mixer;
        public string exposedParameter;
        private float value;
        
        private void Start()
        {
            Load();
        }

        private void OnDestroy()
        {
            Save();
        }

        public void ChangeVolume()
        {
            var newValue = slider.value;
            if (Math.Abs(value - newValue) < 0.01) return;
            value = newValue;
            slider.value = value;
            mixer.SetFloat(exposedParameter, value*80.0f - 80.0f);
            Save();
        }

        private void Load()
        {
            if (!PlayerPrefs.HasKey(exposedParameter)) return;
            value = PlayerPrefs.GetFloat(exposedParameter);
            slider.value = value;
            mixer.SetFloat(exposedParameter, value*40.0f - 40.0f);
        }

        private void Save()
        {
            PlayerPrefs.SetFloat(exposedParameter, value);
        }
    }
}
