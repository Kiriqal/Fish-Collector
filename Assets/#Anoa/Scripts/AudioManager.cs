using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("SFX Settings")]
    public AudioSource sfxAudioSource;
    public AudioClip buttonClickSound;

    void Start()
    {
        // Ambil volume tersimpan dari PlayerPrefs (default = 1)
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Set posisi awal slider
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Terapkan volume ke mixer
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // Listener untuk perubahan slider
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        // 1 = keras (0dB), 0 = pelan (-80dB)
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void PlayButtonClick()
    {
        if (sfxAudioSource != null && buttonClickSound != null)
        {
            sfxAudioSource.PlayOneShot(buttonClickSound);
        }
    }

    void OnDisable()
    {
        // Simpan perubahan saat keluar panel
        PlayerPrefs.Save();
    }
}
