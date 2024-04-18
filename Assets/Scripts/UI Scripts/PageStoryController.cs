using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageStoryController : MonoBehaviour
{
    public static PageStoryController Instance;
    [SerializeField] GameObject[] page3;
    [SerializeField] GameObject[] page4;
    [SerializeField] GameObject[] page5;
    [SerializeField] GameObject[] page6;
    [SerializeField] GameObject[] page7;
    [SerializeField] GameObject[] page8;
    bool[] discovered = new bool[10];

    delegate void DisableAllPages(bool enable = false);
    DisableAllPages DisableAll;
    private void OnEnable()
    {
        DisableAll += DisableEnalbePage3;
        DisableAll += DisableEnalbePage4;
        DisableAll += DisableEnalbePage5;
        DisableAll += DisableEnalbePage6;
        DisableAll += DisableEnalbePage7;
        DisableAll += DisableEnalbePage8;
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
        DisableAll.Invoke();
    }
    public void DiscorverPages(int page)
    {
        if (!discovered[page]) { BookNoises.Instance.PlayNoise(BookNoises.Noises.ScribblePage); }
        discovered[page] = true;
        switch (page)
        {
            case 3:
                DisableEnalbePage3(true);
                break;
            case 4:
                DisableEnalbePage4(true);
                break;
            case 5:
                DisableEnalbePage5(true);
                break;
            case 6:
                DisableEnalbePage6(true);
                break;
            case 7:
                DisableEnalbePage7(true);
                break;
            case 8:
                DisableEnalbePage8(true);
                break;
        }
    }
    void DisableEnalbePage3(bool enable = false)
    {
        foreach (var obj in page3)
        {
            obj.gameObject.SetActive(enable);
        }
    }
    void DisableEnalbePage4(bool enable = false)
    {
        foreach (var obj in page4)
        {
            obj.gameObject.SetActive(enable);
        }
    }
    void DisableEnalbePage5(bool enable = false)
    {
        foreach (var obj in page5)
        {
            obj.gameObject.SetActive(enable);
        }
    }
    void DisableEnalbePage6(bool enable = false)
    {
        foreach (var obj in page6)
        {
            obj.gameObject.SetActive(enable);
        }
    }
    void DisableEnalbePage7(bool enable = false)
    {
        foreach (var obj in page7)
        {
            obj.gameObject.SetActive(enable);
        }
    }
    void DisableEnalbePage8(bool enable = false)
    {
        foreach (var obj in page8)
        {
            obj.gameObject.SetActive(enable);
        }
    }
}
