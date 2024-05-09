using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindLevelController : MonoBehaviour
{
    [SerializeField] GameObject[] windLevels;
    int level;
    public int setLevel { get { return level; } set { level = value; SelectLevel(value); } }
    public static WindLevelController Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    void SelectLevel(int lvl)
    {
        foreach (GameObject item in windLevels)
        {
            item.SetActive(false);
        }
        windLevels[lvl].SetActive(true);
    }
}
