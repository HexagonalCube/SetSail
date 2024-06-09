using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSkybox : MonoBehaviour
{
    [SerializeField] Material skybox;
    // Start is called before the first frame update
    void Start()
    {
        skybox.SetFloat("_Exposure", 1.3f);
        StartCoroutine(KillAfterSeconds(2f));
    }
    private void Awake()
    {
        skybox.SetFloat("_Exposure", 1.3f);
    }
    private void OnValidate()
    {
        skybox.SetFloat("_Exposure", 1.3f);
    }
    IEnumerator KillAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
    void Update()
    {
        skybox.SetFloat("_Exposure", 1.3f);
    }
}
