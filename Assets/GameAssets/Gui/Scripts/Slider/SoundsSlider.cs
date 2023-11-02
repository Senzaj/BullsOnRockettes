using System;
using UnityEngine;

namespace GameAssets.Gui.Scripts.Slider
{
    public class SoundsSlider : MonoBehaviour
    {
        private UnityEngine.UI.Slider _slider;
        private const string Volume = nameof(Volume);

        private void OnEnable()
        {
            _slider = GetComponent<UnityEngine.UI.Slider>();

            if (PlayerPrefs.HasKey(Volume))
                SetSavedVolume();
            else
                SetUnsavedVolume();
        }

        private void SetSavedVolume()
        {
            AudioListener.volume = PlayerPrefs.GetFloat(Volume);
            _slider.value = PlayerPrefs.GetFloat(Volume);
        }

        private void SetUnsavedVolume()
        {
            AudioListener.volume = 1;
            _slider.value = 1;
        }
        
        public void ChangeVolume(Single newValue)
        {
            AudioListener.volume = newValue;
            PlayerPrefs.SetFloat(Volume, newValue);
        }
    }
}
