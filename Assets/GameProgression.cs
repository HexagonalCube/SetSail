using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : GameStage
{
    [SerializeField] Terrain[] playableTerrains;
    public static GameProgression Instance;
    [SerializeField] private WorldStage curStage = WorldStage.Intro;
    public WorldStage Stage { get { return curStage; } set { curStage = value; StageChange(); } }
    [SerializeField] private int itemsCollected;
    public int Items { get {  return itemsCollected; } set {  itemsCollected = value; } }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    void StageChange()
    {
        switch (Stage)
        {
            case WorldStage.Island1:
                Footsteps.Instance.Terrain = playableTerrains[0];
                break;
            case WorldStage.Island2:
                Footsteps.Instance.Terrain = playableTerrains[1];
                break;
            case WorldStage.Island3:
                Footsteps.Instance.Terrain = playableTerrains[2];
                break;
        }
    }
    public bool CheckBarrier(int password)
    {
        if (itemsCollected == password) { return true; }
        else { return false; }
    }
}
