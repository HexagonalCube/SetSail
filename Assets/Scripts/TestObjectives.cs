using UnityEngine;

public class TestObjectives : MonoBehaviour
{
    [SerializeField] Transform[] Objectives;
    [SerializeField] string objTag;
    [SerializeField] int selected = 0;
    int count;

    private void Start()
    {
        for (int i = 0; i < Objectives.Length; i++)
        {
            if (i == selected)
            {
                Objectives[i].gameObject.SetActive(true);
            }
            else
            {
                Objectives[i].gameObject.SetActive(false);
            }
        }
    }
    public void SelectObjective()
    {
        Objectives[selected].gameObject.SetActive(false);
        selected = UnityEngine.Random.Range(0, Objectives.Length);
        Objectives[selected].gameObject.SetActive(true);
    }
}
