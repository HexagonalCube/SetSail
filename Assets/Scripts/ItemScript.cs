using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Will eventually trigger story content
/// </summary>
public class ItemScript : MonoBehaviour
{
    [SerializeField] content type;
    enum content
    {
        Type_1,
        Type_2,
        Type_3,
        Type_4
    }
    bool CheckIfVisible()
    {
        if (GetComponent<Renderer>().isVisible) return true;
        else return false;
    }
    public void Interact()
    {
        if (CheckIfVisible())
        {
            switch (type)
            {
                case content.Type_1:
                    Destroy(gameObject);
                    break;
                case content.Type_2:
                    Destroy(gameObject);
                    break;
                case content.Type_3:
                    Destroy(gameObject);
                    break;
                case content.Type_4:
                    Destroy(gameObject);
                    break;
            }
        }
    }
    public void HiglightObject(bool highlight)
    {
        if (highlight)
        {
            if (CheckIfVisible())
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else { GetComponent<Renderer>().material.color = Color.white; }
        }
        else { GetComponent<Renderer>().material.color = Color.white; }
    }
}
