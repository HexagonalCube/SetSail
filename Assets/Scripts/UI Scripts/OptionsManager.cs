using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
/// <summary>
/// Player Options Script
/// </summary>
public class OptionsManager : MonoBehaviour
{
    [SerializeField] Resolution[] fullScreenResolutionsAvailable;
    [SerializeField] List<string> fullScreenResolutionList;
    [SerializeField] TMP_Dropdown resolutionSelector;
    [SerializeField] bool fullScreen = true;
    [SerializeField] Resolution selectedResolution;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioMixer masterVolume;
    [SerializeField] float[] unsuportedResolutions;
    private void Start() //Gets Fisrt-Time and previously set options
    {
        SetupResolution();
        //fullScreenResolutionsAvailable = Screen.resolutions;
        GetResolutions();
        fullScreen = SaveGame.LoadFullscreen();
        selectedResolution = fullScreenResolutionsAvailable[SaveGame.LoadResolutionIndex()];
        //Debug.Log($"Previously set resolution {SaveGame.LoadResolutionIndex()}");
        UpdateResolution();
        LoadVolume();
    }
    void SetupResolution()
    {
        int item = 0;
        int setup = 0;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            float w = Screen.resolutions[i].width;
            float h = Screen.resolutions[i].height;
            bool available = true;
            for (int j = 0; j < unsuportedResolutions.Length; j++)
            {
                if (Mathf.Approximately(w / h, unsuportedResolutions[j]))
                {
                    available = false;
                }
            }
            if (available)
            {
                setup++;
            }
        }
        fullScreenResolutionsAvailable = new Resolution[setup];
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            float w = Screen.resolutions[i].width;
            float h = Screen.resolutions[i].height;
            bool available = true;
            for (int j = 0; j < unsuportedResolutions.Length; j++)
            {
                if (Mathf.Approximately(w / h, unsuportedResolutions[j]))
                {
                    available = false;
                }
            }
            if (available)
            {
                fullScreenResolutionsAvailable[item] = Screen.resolutions[i];
                item++;
            }
        }
    }
    void GetResolutions() //Gets all available resolutions, & sets player default to highest
    {
        for (int i = 0; i < fullScreenResolutionsAvailable.Length; i++)
        {
            //float w = fullScreenResolutionsAvailable[i].width;
            //float h = fullScreenResolutionsAvailable[i].height;
            //if (!Mathf.Approximately(w / h, 4f / 3f) && !Mathf.Approximately(w / h, 5f / 4f))
            //{

            //}
            fullScreenResolutionList.Add($"{fullScreenResolutionsAvailable[i].width} x {fullScreenResolutionsAvailable[i].height} ({Mathf.FloorToInt(Convert.ToSingle(fullScreenResolutionsAvailable[i].refreshRateRatio.value))}Hz)");
        }
        resolutionSelector.AddOptions(fullScreenResolutionList);
        resolutionSelector.SetValueWithoutNotify(SaveGame.LoadResolutionIndex());
        if (PlayerPrefs.GetInt("FRESH",0) == 0)
        {
            resolutionSelector.SetValueWithoutNotify(fullScreenResolutionList.Count-1);
            SetResolution(fullScreenResolutionList.Count-1);
        }
    }
    public void MakeFullscreen(bool fs) //Toggles & saves fullscreen
    {
        fullScreen = fs;
        SaveGame.SaveFullscreen(fs);
        UpdateResolution();
    }
    public void SetResolution(int res) //Sets resolution in list Index
    {
        selectedResolution = fullScreenResolutionsAvailable[res];
        SaveGame.SaveResolutionIndex(res);
        PlayerPrefs.SetInt("FRESH",1);
        //Debug.Log($"Resolution Index Set {res}");
        UpdateResolution();
    }
    void UpdateResolution() //Updates screen resolution
    {
        //Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullScreen, selectedResolution.refreshRate);
        if (fullScreen)
        {
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, FullScreenMode.FullScreenWindow, selectedResolution.refreshRateRatio);
        }
        else
        {
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, FullScreenMode.Windowed, selectedResolution.refreshRateRatio);
        }
    }
    public void SetVolume(float vol) //saves selected volume & sets volume to log10 scale
    {
        SaveGame.SaveVolume(vol);
        float convertedVolume = 20 * Mathf.Log10(vol + 0.01f);
        masterVolume.SetFloat("MasterVolume", convertedVolume);
    }
    void LoadVolume() //Loads saved volume & converts to log10 scale
    {
        volumeSlider.SetValueWithoutNotify(SaveGame.LoadVolume());
        float convertedVolume = 20 * Mathf.Log10(SaveGame.LoadVolume() + 0.01f);
        masterVolume.SetFloat("MasterVolume",convertedVolume);
        //Debug.Log($"VolumeLoaded {SaveGame.LoadVolume()}");
    }
}
