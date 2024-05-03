using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public static CursorScript Instance;
    [SerializeField] Animator anim;
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
        anim = GetComponent<Animator>();
    }
    public void Cursor(bool visible, bool open)
    {
        anim.SetBool ("Visible", visible);
        anim.SetBool ("Open", open);
    }
}
