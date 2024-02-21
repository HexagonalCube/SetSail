using UnityEngine;
public class CameraScript : MonoBehaviour
{
    [Header("Pontos de Referencia")]
    public Transform cameraPoint;
    public Transform cameraTarget;
    [Header("Limites da camera")]
    [SerializeField] float minX = -45f;
    [SerializeField] float maxX = 10f;
    [SerializeField] float minY = 0;
    [SerializeField] float maxY = 180;
    [Header("Valores de Debug")]
    [SerializeField] Vector2 rotation;
    [SerializeField] Vector2 realVector;
    [SerializeField] Vector2 realDiffVector;
    [Header("Velocidade da rotacao da camera")]
    public float sensitivity = 3;

    private void Start()
    {
        rotation = transform.localEulerAngles;
    }

    void Update()
    {
        //Botando a camera no lugar onde ficaria
        transform.position = cameraPoint.position;

        //Usando os vetores do barco para fazer movimento da camera junto ao barco
        Vector2 targetVec = new Vector2(/*cameraTarget.localEulerAngles.x*/0, -cameraTarget.localEulerAngles.y);
        Vector2 cameraVec = transform.localEulerAngles;

        //Rotacao do mouse
        rotation.y += Input.GetAxis("Mouse X") * sensitivity;
        rotation.x += -Input.GetAxis("Mouse Y") * sensitivity;
        //prendendo a rotacao aos limites
        rotation.y = Mathf.Clamp(rotation.y, minY,maxY);
        rotation.x = Mathf.Clamp(rotation.x, minX, maxX);
        //Calculando quanto o mouse se moveu
        Vector2 vectorDiff = rotation - cameraVec;
        //A diferenca entre o movimento do mouse e o movimento do barco
        realDiffVector = vectorDiff - targetVec;
        //Adicionando a diferenca ao vetor antigo da camera
        realVector = cameraVec + realDiffVector;
        //Rotacionando a camera ao angulo certo
        transform.localEulerAngles = realVector;
    }
}
