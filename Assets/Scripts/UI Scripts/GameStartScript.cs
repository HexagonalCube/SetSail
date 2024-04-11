using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    [SerializeField] bool enableInputStart;
    void Start()
    {
        InputController.Instance.EnableInput = enableInputStart;
    }
}
