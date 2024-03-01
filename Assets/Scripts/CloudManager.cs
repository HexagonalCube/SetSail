using UnityEngine;
/// <summary>
/// Controls Cloud visuals
/// </summary>
public class CloudManager : MonoBehaviour
{
    public Color cloudColor;
    public float colorIntensity;
    public float cloudScale;
    public Vector2 cloudSpeed;
    public float cloudHeight;
    public float cloudPower;

    public Transform clouds;

    [SerializeField] Material cloudMat;

    void Start()
    {
        SetVariables();
    }
    void SetVariables() //Sempre ter as mesmas variaveis
    {
        cloudMat = clouds.GetComponent<Renderer>().sharedMaterial;
    }
    private void OnValidate() //Sempre tenha os valores certos, e atualize eles no editor
    {
        if (!cloudMat)
        {
            SetVariables();
        }
        UpdateMaterial();
    }
    public void UpdateMaterial() //Atualizando material no editor
    {
        cloudMat.SetColor("_CloudColor", cloudColor * colorIntensity);
        cloudMat.SetFloat("_CloudScale", cloudScale);
        cloudMat.SetVector("_CloudSpeed", cloudSpeed / 1000);
        cloudMat.SetVector("_DistortSpeed", -cloudSpeed / 1000);
        cloudMat.SetFloat("_VertexOffset", cloudHeight);
        cloudMat.SetFloat("_CloudPower", cloudPower / 100);
    }
}
