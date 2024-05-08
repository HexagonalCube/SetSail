using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : GameStage
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject main;
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] beeps;
    [SerializeField] float beepScale;
    [SerializeField] Transform[] playerPoints;
    [SerializeField] TMP_Dropdown musicDropdown;
    [SerializeField] TMP_Dropdown islandDropdown;
    [SerializeField] Terrain[] terrains;
    [SerializeField] Footsteps footsteps;
    List<string> musicList;
    List<string> islandList;
    bool lo;
    bool debug;
    enum Sequence { Up, Up2, Down, Down2, Left, Right, Left2, Right2, Start, Inside }
    Sequence combo = Sequence.Up;
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
        Debug.Log("boat "+t);
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
    void CheckCombo(Sequence state)
    {
        switch (state)
        {
            case Sequence.Up:
                if (Input.GetKeyDown(KeyCode.UpArrow)) { combo = Sequence.Up2; source.PlayOneShot(beeps[0], beepScale); }
                break;
            case Sequence.Up2:
                if (Input.GetKeyDown(KeyCode.UpArrow)) { combo = Sequence.Down; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Down:
                if (Input.GetKeyDown(KeyCode.DownArrow)) { combo = Sequence.Down2; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Down2:
                if (Input.GetKeyDown(KeyCode.DownArrow)) { combo = Sequence.Left; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Left:
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { combo = Sequence.Right; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Left2:
                if (Input.GetKeyDown(KeyCode.LeftArrow)) { combo = Sequence.Right2; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Right:
                if (Input.GetKeyDown(KeyCode.RightArrow)) { combo = Sequence.Left2; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Right2:
                if (Input.GetKeyDown(KeyCode.RightArrow)) { combo = Sequence.Start; source.PlayOneShot(beeps[0], beepScale); } else { combo = Sequence.Up; source.PlayOneShot(beeps[1], beepScale); }
                break;
            case Sequence.Start:
                if (Input.GetKeyDown(KeyCode.Return)) { debug = true; source.PlayOneShot(beeps[2], beepScale); }
                break;
        }
    }
    private void Update()
    {

        if (Input.anyKeyDown && !debug)
        {
            CheckCombo(combo);
        }
        if (Input.GetKeyDown(KeyCode.G) && debug)
        {
            switch (main.activeSelf)
            {
                case true:
                    main.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    combo = Sequence.Up;
                    break;
                case false:
                    main.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
            }
        }
    }
}
