using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PageStoryController : MonoBehaviour
{
    public static PageStoryController Instance;
    [SerializeField] GameObject[] pages;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] tabsAurea;
    [SerializeField] GameObject[] tabsParadisi;
    bool[] discovered = new bool[20];
    public bool[] PagesDiscovered {  get { return discovered; } }
    delegate void DisableAllPages(int page = 0, bool enabled = false);
    DisableAllPages DisableAll;
    private void OnEnable()
    {
        DisableAll += DisableEnalbePages;
        DisableAll += DisableEnableButtons;
        DisableAll += DisableEnableTabs;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    private void Start()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            DisableAll.Invoke(i);
        }
    }
    public void DiscorverPages(int page)
    {
        if (page !=10 && !discovered[page]) { BookNoises.Instance.PlayNoise(BookNoises.Noises.ScribblePage); }
        discovered[page] = true;
        DisableEnalbePages(page, true);
        DisableEnableButtons(page, true);
        if (page <2)
        {
            DisableEnableTabs(1, true);
        }
        else if (page < 3) { DisableEnableTabs(2, true); }
    }
    void DisableEnalbePages(int page = 0, bool enable = false)
    {
        if(page!=10)
        pages[page].SetActive(enable);
    }
    void DisableEnableButtons(int button = 0, bool enable = false)
    {
        if (button != 10)
        buttons[button].SetActive(enable);
    }
    void DisableEnableTabs(int placeholder = 0, bool enable = false)
    {
        switch (placeholder)
        {
            case 0:
                Debug.Log("AllTabs "+enable);
                foreach (var tab in tabsAurea)
                {
                    tab.SetActive(enable);
                }
                foreach (var tab in tabsParadisi)
                {
                    tab.SetActive(enable);
                }
                break;
            case 1:
                Debug.Log("AureaTabs " + enable);
                foreach (var tab in tabsAurea)
                {
                    tab.SetActive(enable);
                }
                break;
            case 2:
                Debug.Log("ParadisiTabs " + enable);
                foreach (var tab in tabsParadisi)
                {
                    tab.SetActive(enable);
                }
                break;
        }
    }
}
