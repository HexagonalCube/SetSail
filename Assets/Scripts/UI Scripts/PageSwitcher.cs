using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    public static PageSwitcher Instance;
    int curPage;
    [SerializeField] GameObject[] pages;
    public int SelectPage {  get { return curPage; } set { SwitchPage(value); } }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else { Instance = this; }
    }
    public void SwitchPage(int page)
    {
        BookNoises.Instance.PlayNoise(BookNoises.Noises.FlipPage);
        curPage = page;
        DisableEnablePages(page);
    }
    void DisableEnablePages(int page)
    {
        foreach (var p in pages)
        {
            p.SetActive(false);
        }
        pages[page].SetActive(true);
    }
}
