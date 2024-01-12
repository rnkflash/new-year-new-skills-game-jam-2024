using System;
using boot;
using UnityEngine;
using UnityEngine.UI;

namespace ui
{
    public class DayNightBar : MonoBehaviour
    {
        public Slider slider;
        public Toggle toggle;

        private void Start()
        {
            OnDayNightProgress();

            slider.onValueChanged.AddListener(OnSliderValueChanged);
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveAllListeners();
        }

        private void OnToggleValueChanged(bool arg0)
        {
            DayNightManager.instance.SetAnimated(arg0);
        }

        private void OnSliderValueChanged(float arg0)
        {
            DayNightManager.instance.SetProgress(arg0);
            Save();
        }
        
        public void OnDayNightProgress()
        {
            toggle.isOn = DayNightManager.instance.isAnimated;
            slider.value = DayNightManager.instance.progress;
        }
        
        private void Save()
        {
            PlayerPrefs.SetFloat("DayNightProgress", slider.value);
        }
    }
}
