using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : GameStage
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject main;
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioSource source;
    [SerializeField] Transform[] playerPoints;
    [SerializeField] TMP_Dropdown musicDropdown;
    [SerializeField] TMP_Dropdown islandDropdown;
    [SerializeField] Terrain[] terrains;
    [SerializeField] Footsteps footsteps;
    List<string> musicList;
    List<string> islandList;
    bool lo;
    private void Start()
    {
        musicList = new List<string>(music.Length);
        islandList = new List<string>(playerPoints.Length);
        foreach(AudioClip music in music)
        {
            musicList.Add(music.name);
        }
        foreach(Transform point in playerPoints)
        {
            islandList.Add(point.name);
        }
        musicDropdown.ClearOptions();
        musicDropdown.AddOptions(musicList);
        islandDropdown.ClearOptions();
        islandDropdown.AddOptions(islandList);
    }
    public void SetItemCount(int i)
    {
        GameProgression.Instance.Items = i;
    }
    public void SetPlayerIsland(int i)
    {
        Debug.Log(playerPoints[i].name);
        player.transform.position = playerPoints[i].position;
        footsteps.Terrain = terrains[i];
        switch (i)
        {
            case 0:
                GameProgression.Instance.Stage = WorldStage.Island1;
                break;
            case 1:
                GameProgression.Instance.Stage = WorldStage.Island2;
                break;
            case 2:
                GameProgression.Instance.Stage = WorldStage.Island3;
                break;
        }
    }
    public void SetPlayerBoat(bool t)
    {
        CameraScript.Instance.InBoat = t;
        player.SetActive(!t);
    }
    public void SetMusicLoop(bool t)
    {
        source.loop = t;
        lo = t;
    }
    public void SetMusicSelect(int i)
    {
        source.Stop();
        source.clip = music[i];
        source.loop = lo;
        source.Play();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            switch (main.activeSelf)
            {
                case true:
                    main.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case false:
                    main.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
            }
        }
    }
}
