using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : MonoBehaviour
{
    public enum GameStage { Intro, Island1, MidSea, Island2, OpenSea, Island3}
    [SerializeField] private GameStage curStage = GameStage.Intro;
    public GameStage Stage { get { return curStage; } set { curStage = value;} }
    [SerializeField] private int itemsCollected;
    public int Items { get {  return itemsCollected; } set {  itemsCollected = value; } }

    public bool CheckBarrier(int password)
    {
        if (itemsCollected == password) { return true; }
        else { return false; }
    }
}
