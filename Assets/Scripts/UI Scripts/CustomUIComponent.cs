using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CustomUIComponent : MonoBehaviour
{
    //get all the components before the game starts running
    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        Setup();
        Configure();
    }

    //these are going to be implemented individually on each script
    public abstract void Setup();

    public abstract void Configure();


    //this triggers the Init() method on each file save
    public void OnValidate()
    {
        Init();
    }

}
