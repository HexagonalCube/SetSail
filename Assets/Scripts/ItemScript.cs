using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Will eventually trigger story content
/// </summary>
[RequireComponent (typeof(Outline))]
[RequireComponent (typeof(SphereCollider))]
public class ItemScript : MonoBehaviour
{
    [SerializeField] content type;
    [SerializeField] Outline outline;
    [SerializeField] GameUI_Controller ui;
    [SerializeField] GameProgression gameProg;
    bool canOutline = true;
    private void Start()
    {
        transform.tag = "Item";
        gameProg = FindFirstObjectByType<GameProgression>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
        GetComponent<SphereCollider>().isTrigger = true;
    }
    enum content
    {
        Emerald,
        Bucket,
        Photo,
        Letter,
        End,
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
                    ui.OpenStory(0,2);
                    StartCoroutine(DestroyAfterSeconds(0.01f));
                    break;
                case content.Bucket:
                    if (gameProg.Items >= 1)
                    {
                        canOutline = false;
                        ui.Interact(false);
                        gameProg.Items++;
                        ui.OpenStory(1, 3);
                        StartCoroutine(DestroyAfterSeconds(0.01f));
                    }
                    else
                    {
                        GameUI_Controller.Instance.Comment("Hmmm...");
                    }
                    break;
                case content.Photo:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    ui.OpenStory(2,4);
                    StartCoroutine(DestroyAfterSeconds(0.01f, true));
                    break;
                case content.Letter:
                    if (gameProg.Items >= 3)
                    {
                        canOutline = false;
                        ui.Interact(false);
                        gameProg.Items++;
                        ui.OpenStory(3, 5);
                        StartCoroutine(DestroyAfterSeconds(0.01f, true));
                    }
                    else
                    {
                        GameUI_Controller.Instance.Comment("Hmmm...");
                    }
                    break;
                case content.End:
                    canOutline = false;
                    ui.Interact(false);
                    gameProg.Items++;
                    CameraScript.Instance.enabled = false;
                    InputController.Instance.enabled = false;
                    Footsteps.Instance.gameObject.SetActive(false);
                    MusicComposer1000.Instance.IMPORTANTENDMUSIC();
                    EndScript.Instance.StartEndSequence();
                    break;
                case content.Type_test:
                    GetComponentInParent<TestObjectives>().SelectObjective();
                    break;
            }
        }
    }
    public void CursorActivate(bool active, bool open = true)
    {
        if (CheckIfVisible())
        {
            ui.CursorUpdate(open, active);
        }
    }
    public void HiglightObjectNear(bool highlight)
    {
        if (highlight && CheckIfVisible() && canOutline)
        {
            //ui.CursorUpdate(true, true);
            //ui.Interact(true);
            switch (type)
            {
                case content.Emerald:
                    ui.Interact(true, "Inspecionar Esmeralda (E)");
                    break;
                case content.Bucket:
                    if (gameProg.Items >= 1)
                    {
                        ui.Interact(true, "Inspecionar Balde (E)");
                    }
                    break;
                case content.Photo:
                    ui.Interact(true, "Pegar Foto (E)");
                    break;
                case content.Letter:
                    if (gameProg.Items >= 3)
                    {
                        ui.Interact(true, "Pegar Carta (E)");
                    }
                    break;
                case content.End:
                    ui.Interact(true, "Pegar Boneca (E)");
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
            switch (type)
            {
                case content.Emerald:
                    outline.enabled = true;
                    break;
                case content.Bucket:
                    if (gameProg.Items >= 1)
                    {
                        outline.enabled = true;
                    }
                    else
                    {
                        outline.enabled = false; ui.Interact(false); ui.CursorUpdate(false, false);
                    }
                    break;
                case content.Photo:
                    outline.enabled = true;
                    break;
                case content.Letter:
                    if (gameProg.Items >= 3)
                    {
                        outline.enabled = true;
                    }
                    else
                    {
                        outline.enabled = false; ui.Interact(false); ui.CursorUpdate(false, false);
                    }
                    break;
                case content.End:
                    outline.enabled = true;
                    break;
                default: break;
            }
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
