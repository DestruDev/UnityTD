using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {
    public AudioMixer audioMixer; // Drag in your Audio Mixer
    public Slider volumeSlider;   // Drag in your UI slider

    private const string volumePrefKey = "SavedMasterVolume";

    private void Start() {
        float savedVolume = PlayerPrefs.GetFloat(volumePrefKey, 100f);
        SetVolume(savedVolume); // Set initial volume and sync everything
    }

    public void SetVolume(float value) {
        value = Mathf.Clamp(value, 0.001f, 100f); // prevent log(0)

        // Update the Audio Mixer (convert 0-100 to 0-1)
        float volumeNormalized = value / 100f;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volumeNormalized) * 20f);

        // Save and update UI
        PlayerPrefs.SetFloat(volumePrefKey, value);
        volumeSlider.value = value;
    }

    public void SetVolumeFromSlider() {
        SetVolume(volumeSlider.value);
    }
}