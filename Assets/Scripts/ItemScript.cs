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
        Emerald,
        Bucket,
        Photo,
        Letter,
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
                case content.Emerald:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory(3);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Bucket:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory(4);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Photo:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory(5);
                    ui.OpenStory(6);
                    StartCoroutine(DestroyAfterSeconds(0.01f, true));
                    break;
                case content.Letter:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory(7);
                    ui.OpenStory(8);
                    StartCoroutine(DestroyAfterSeconds(0.01f, true));
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
                case content.Emerald:
                    ui.Interact(true, "Pegar Esmeralda (E)");
                    break;
                case content.Bucket:
                    ui.Interact(true, "Pegar Balde (E)");
                    break;
                case content.Photo:
                    ui.Interact(true, "Pegar [ITEM] (E)");
                    break;
                case content.Letter:
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
    IEnumerator DestroyAfterSeconds(float seconds, bool destroyItem = false)
    {
        ui.Interact(false);
        ui.CursorUpdate(true, false);
        ui.scheduleFadeOut = true;
        yield return new WaitForSeconds(seconds);
        ui.scheduleFadeOut = true;
        PlayerInteract.Instance.curItem = null;
        if (destroyItem) { Destroy(gameObject); }
        else { Destroy(this); }
    }
}
