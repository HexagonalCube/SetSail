using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Saves in playerprefs set game variables
/// </summary>
public class SaveGame
{
    const string SAVE_VOLUME = "VOLUME"; //Volume addr
    const string SAVE_FSCREEN = "FSCREEN"; //Fullscreen addr
    const string SAVE_RESOLUTION = "RESOLUTION"; //Resolution addr
    const string SAVE_COMPLETE = "COMPLETE"; //Completion addr
    const string SAVE_LAST = "LAST"; //Last Level addr
    const string SAVE_SENSITIVITY = "SENSITIVITY"; //UNUSED addr
    const string SAVE_CON_SENSITIVITY = "CONSENSITIVITY"; //UNUSED addr
    const string T = "true"; //True value
    const string F = "false"; //False value
    public static void SaveVolume(float volume) //Gets volume value & saves
    {
        Debug.Log("OpenAccess SaveVolume " + volume);
        PlayerPrefs.SetFloat(SAVE_VOLUME, volume);
    }
    public static float LoadVolume() //Returns saved volume value or default
    {
        return PlayerPrefs.GetFloat(SAVE_VOLUME, 0.8f);
    }
    public static void SaveFullscreen(bool fullscreen) //Gets fullscreen value & saves using string values for true&false
    {
        if (fullscreen)
        {
            PlayerPrefs.SetString(SAVE_FSCREEN, T);
        }
        else
        {
            PlayerPrefs.SetString(SAVE_FSCREEN, F);

        }
    }
    public static bool LoadFullscreen() //Returns bool from saved string value
    {
        return PlayerPrefs.GetString(SAVE_FSCREEN,T) == "true" ? true : false;
    }
    public static void SaveResolutionIndex(int resolution) //Saves selected resolution list index
    {
        PlayerPrefs.SetInt(SAVE_RESOLUTION, resolution);
    } 
    public static int LoadResolutionIndex() //Returns saved resolution list index
    {
        return PlayerPrefs.GetInt(SAVE_RESOLUTION,0);
    }
    public static bool LoadLevel(int level) //Returns if level was completed
    {
        return PlayerPrefs.GetString(SAVE_COMPLETE + level, F) == "true" ? true : false;
    }
    public static void SaveSens(float sensitivity) //UNUSED
    {
        PlayerPrefs.SetFloat(SAVE_SENSITIVITY, sensitivity);
    }
    public static float LoadSens() //UNUSED
    {
        return PlayerPrefs.GetFloat(SAVE_SENSITIVITY, 1);
    }
    public static void SaveConSens(float sensitivity) //UNUSED
    {
        PlayerPrefs.SetFloat(SAVE_CON_SENSITIVITY, sensitivity);
    }
    public static float LoadConSens() //UNUSED
    {
        return PlayerPrefs.GetFloat(SAVE_CON_SENSITIVITY, 1);
    }
}
