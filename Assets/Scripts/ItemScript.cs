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
    [SerializeField] GameProgression gameProg;
    bool canOutline = true;
    private void Start()
    {
        gameProg = FindFirstObjectByType<GameProgression>();
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
                    gameProg.Items++;
                    ui.OpenStory(3);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_2:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory(4);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_3:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory();
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_4:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory();
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Type_test:
                    GetComponentInParent<TestObjectives>().SelectObjective();
                    break;
            }
        }
    }
    public void CursorActivate(bool active)
    {
        if (CheckIfVisible())
        {
            ui.CursorUpdate(false, active);
        }
    }
    public void HiglightObjectNear(bool highlight)
    {
        if (highlight && CheckIfVisible() && canOutline)
        {
            ui.CursorUpdate(true, true);
            //ui.Interact(true);
            switch (type)
            {
                case content.Type_1:
                    ui.Interact(true, "Pegar Esmeralda (E)");
                    break;
                case content.Type_2:
                    ui.Interact(true, "Pegar Balde (E)");
                    break;
                case content.Type_3:
                    ui.Interact(true, "Pegar [ITEM] (E)");
                    break;
                case content.Type_4:
                    ui.Interact(true, "Pegar [ITEM] (E)");
                    break;
                default: break;
            }
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
        else { outline.enabled = false; ui.Interact(false); ui.CursorUpdate(false, false); }
    }
    IEnumerator DestroyAfterSeconds(float seconds)
    {
        ui.Interact(false);
        ui.CursorUpdate(true, false);
        ui.scheduleFadeOut = true;
        yield return new WaitForSeconds(seconds);
        ui.scheduleFadeOut = true;
        Destroy(gameObject);
    }
}
