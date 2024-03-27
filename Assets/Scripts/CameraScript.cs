using UnityEngine;
public class CameraScript : MonoBehaviour
{
    [Header("Pontos de Referencia")]
    public Transform cameraBoatPoint;
    public Transform cameraLandPoint;
    public Transform cameraTarget;
    public Transform player;
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
    bool active = true;
    public bool cameraActive {  get { return active; } set {  active = value; } }

    [SerializeField] bool inBoat = false;
    [SerializeField] Camera cameraMask;
    private void Start()
    {
        active = true;
        rotation = transform.localEulerAngles;
        cameraMask.gameObject.SetActive(inBoat);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (cameraActive)
        {
            cameraMask.gameObject.SetActive(inBoat);
            if (inBoat)
            {
                //Botando a camera no lugar onde ficaria
                transform.position = cameraBoatPoint.position;

                //Usando os vetores do barco para fazer movimento da camera junto ao barco
                Vector2 targetVec = new Vector2(/*cameraTarget.localEulerAngles.x*/0, -cameraTarget.localEulerAngles.y);
                Vector2 cameraVec = transform.localEulerAngles;

                //Rotacao do mouse
                rotation.y += Input.GetAxis("Mouse X") * sensitivity;
                rotation.x += -Input.GetAxis("Mouse Y") * sensitivity;
                //prendendo a rotacao aos limites
                rotation.y = Mathf.Clamp(rotation.y, minY, maxY);
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
            else
            {
                transform.position = cameraLandPoint.position;

                rotation.y += Input.GetAxis("Mouse X") * sensitivity;
                rotation.x += -Input.GetAxis("Mouse Y") * sensitivity;
                rotation.x = Mathf.Clamp(rotation.x, -90, 90);

                transform.localEulerAngles = rotation;
                player.localEulerAngles = new Vector2(0, rotation.y);
            }
        }
    }
    public void SwitchCamera(bool toBoat)
    {
        inBoat = toBoat;
    }
}
