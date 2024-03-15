using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Will eventually trigger story content
/// </summary>
public class ItemScript : MonoBehaviour
{
    [SerializeField] content type;
    [SerializeField] Outline outline;
    [SerializeField] GameUI_Controller ui;
    bool canOutline = true;
    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    enum content
    {
        Type_1,
        Type_2,
        Type_3,
        Type_4,
        Type_test
    }
    bool CheckIfVisible()
    {
        if (GetComponentInChildren<Renderer>().isVisible) return true;
        else return false;
    }
    public void Interact()
    {
        if (CheckIfVisible())
        {
            switch (type)
            {
                case content.Type_1:
                    canOutline = false;
                    ui.Interact(false);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_2:
                    canOutline = false;
                    ui.Interact(false);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_3:
                    canOutline = false;
                    ui.Interact(false);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_4:
                    canOutline = false;
                    ui.Interact(false);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_test:
                    GetComponentInParent<TestObjectives>().SelectObjective();
                    break;
            }
        }
    }
    public void HiglightObjectNear(bool highlight)
    {
        if (highlight && CheckIfVisible() && canOutline)
        {
            ui.Interact(true);
        }
        else 
        {
            ui.Interact(false); 
        }
    }
    public void HighlightObjectSimple(bool highlight)
    {
        if (highlight && canOutline)
        {
            outline.enabled = true;
        }
        else { outline.enabled = false; ui.Interact(false); }
    }
    IEnumerator DestroyAfterSeconds(float seconds)
    {
        ui.Interact(false);
        ui.scheduleFade = true;
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
