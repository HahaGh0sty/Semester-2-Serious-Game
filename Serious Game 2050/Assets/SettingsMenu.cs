using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public static class GameSettings
{
    public static int MapWidth = 50;
    public static int MapHeight = 50;
}

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider mainSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;

    const string Mixer_Main = "VolumeMaster";
    const string Mixer_Music = "VolumeMusic";
    const string Mixer_Effects = "VolumeEffects";

    void Awake()
    {
        mainSlider.onValueChanged.AddListener(SetMainVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
        Object.DontDestroyOnLoad(this);
    }

    void Start()
    {
        float mainVolume = PlayerPrefs.GetFloat("VolumeMaster", 0.75f);
        float musicVolume = PlayerPrefs.GetFloat("VolumeMusic", 0.75f);
        float effectsVolume = PlayerPrefs.GetFloat("VolumeEffects", 0.75f);

        mainSlider.value = mainVolume;
        musicSlider.value = musicVolume;
        effectsSlider.value = effectsVolume;

        audioMixer.SetFloat(Mixer_Main, mainVolume);
        audioMixer.SetFloat(Mixer_Music, musicVolume);
        audioMixer.SetFloat(Mixer_Effects, effectsVolume);

        PlayerPrefs.Save();
    }

    void SetMainVolume(float value)
    {
        audioMixer.SetFloat(Mixer_Main, value);
        PlayerPrefs.SetFloat("VolumeMaster", value);
    }

    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(Mixer_Music, value);
        PlayerPrefs.SetFloat("VolumeMusic", value);
    }

    void SetEffectsVolume(float value)
    {
        audioMixer.SetFloat(Mixer_Effects, value);
        PlayerPrefs.SetFloat("VolumeEffects", value);
    }


    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("VolumeMaster", volume);
    }

    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("VolumeMusic", volume);
    }

    public void SetVolumeEffects(float volume)
    {
        audioMixer.SetFloat("VolumeEffects", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }


}
