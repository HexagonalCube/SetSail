using System.Collections;
using UnityEngine;
/// <summary>
/// Gives Wind forces when inside wind zones
/// </summary>
public class WindObject : MonoBehaviour
{
    public bool inWindZone = false; //If in windzone
    public bool inEndZone = false; //If at edge of map
    public bool boatStopped = false;
    public GameObject windZone; //AOE

    public Rigidbody rb; //Boat
    public Transform sail; //Sail
    public Transform windIndicator; //WindFlag
    public Material flagMat; //FlagColor

    [SerializeField] Vector3 windCurrent; //Wind Dir
    Vector3 sailDirection; //Sail Dir
    [SerializeField] Vector3 passiveDir; //Passive Wind Dir
    float passiveChange;
    [Range(0f, 1f)]
    [SerializeField] float passiveScale = 0.1f;
    [SerializeField] bool canRollForWind = true;

    [SerializeField] float angleDiffR;
    [SerializeField] float angleDiffL;
    [SerializeField] float dirDiff;
    [SerializeField] float speedCur;

    [SerializeField] float passiveAngleDiffR;
    [SerializeField] float passiveAngleDiffL;

    public float angleMin; //MinAngle for wind effectiveness
    public float baseSpeed; //Base speed without wind
    public float minSpeed; //Speed against wind
    public float maxSpeed; //Speed with wind

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Update Variables
        sailDirection = sail.right;
        angleDiffR = Vector3.Angle(windCurrent, sailDirection);
        angleDiffL = Vector3.Angle(windCurrent, -sailDirection);
        passiveAngleDiffR = Vector3.Angle(passiveDir, sailDirection);
        passiveAngleDiffL = Vector3.Angle(passiveDir, -sailDirection);
        dirDiff = Vector3.Angle(windCurrent, transform.forward);
        speedCur = rb.velocity.magnitude;
        if (!inWindZone)
        {
            windCurrent = Vector3.zero;
        }
    }
    void FixedUpdate()
    {
        RollForWindChange();
        if (!inEndZone)//When not at edge of map
        {
            if (inWindZone)//When effected by winds
            {
                windIndicator.forward = windCurrent;
                windIndicator.localEulerAngles = windIndicator.localEulerAngles - new Vector3(windIndicator.localEulerAngles.x, 0, windIndicator.localEulerAngles.z);
                if ((angleDiffR < angleMin || angleDiffL < angleMin) && dirDiff < 100)
                {
                    //Maximum Force
                    if (!boatStopped) { rb.AddForce(rb.transform.forward * maxSpeed); }
                    flagMat.color = Color.green;
                }
                else if ((angleDiffR < angleMin || angleDiffL < angleMin) && dirDiff > 100)
                {
                    //Minimum Force
                    if (!boatStopped) { rb.AddForce(rb.transform.forward * (minSpeed)); }
                    flagMat.color = Color.red;
                }
                else
                {
                    //Paralel to Wind force
                    if (!boatStopped) { rb.AddForce(rb.transform.forward * (minSpeed * 3)); }
                    flagMat.color = Color.cyan;
                }
            }
            else
            {
                //Normal Force
                windIndicator.forward = Vector3.RotateTowards(windIndicator.forward, passiveDir, 0.005f, 0.001f);
                windIndicator.localEulerAngles = windIndicator.localEulerAngles - new Vector3(windIndicator.localEulerAngles.x, 0, windIndicator.localEulerAngles.z);
                float diff = Mathf.Min(passiveAngleDiffL, passiveAngleDiffR);
                if (!boatStopped) { rb.AddForce(rb.transform.forward * (Mathf.Clamp(baseSpeed + Mathf.Pow(diff, -1) * 400, 0, 100))); }
                if (diff < 10f) { flagMat.color = Color.magenta; }
                else if (diff < 25f) { flagMat.color = Color.green; }
                else {  flagMat.color = Color.yellow; }
            }
        }
        else //When at edge of map
        {
            windIndicator.forward = windCurrent;
            rb.AddForce(windCurrent * 75);
            flagMat.color = Color.red;
        }
    }
    void RollForWindChange()
    {
        if (canRollForWind)
        {
            canRollForWind = false;
            float roll = Random.value;
            if (roll > 0.5f) { PassiveWindChange(); }
            StartCoroutine(RollCooldown());
        }
    }
    IEnumerator RollCooldown()
    {
        yield return new WaitForSecondsRealtime(3f);
        canRollForWind = true;
    }
    void PassiveWindChange()
    {
        float newChange = Random.value;
        float newRange = Random.Range(0.8f, 1f);
        if (RoundToDecimalPlace(newChange, 2) > 0.5f) { passiveChange = Mathf.MoveTowards(passiveChange, -1 * newRange, passiveScale); }
        if (RoundToDecimalPlace(newChange, 2) < 0.5f) { passiveChange = Mathf.MoveTowards(passiveChange, 1 * newRange, passiveScale); }
        passiveDir = transform.forward + (transform.right * passiveChange);
    }
    float RoundToDecimalPlace(float number, int decimalPlaces)
    {
        float pow = Mathf.Pow(10, decimalPlaces);
        return Mathf.Round((number * pow) / pow);
    }
    private void OnEnable()
    {
        boatStopped = false;
    }
    public void SwitchBoatStopped(bool stopped)
    {
        boatStopped = stopped;
    }
    private void OnTriggerEnter(Collider col)//Enter Wind
    {
        if(col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
        if (col.gameObject.tag == "windBarrier")
        {
            windZone = col.gameObject;
            inWindZone = true;
            inEndZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
    }
    private void OnTriggerStay(Collider col)//In Wind
    {
        if (col.gameObject.tag == "windArea")
        {
            windZone = col.gameObject;
            inWindZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
        if (col.gameObject.tag == "windBarrier")
        {
            windZone = col.gameObject;
            inWindZone = true;
            inEndZone = true;
            windCurrent = windZone.GetComponent<WindArea>().direction;
        }
    }
    private void OnTriggerExit(Collider col)//Exit Wind
    {
        if (col.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
        if (col.gameObject.tag == "windBarrier")
        {
            inWindZone = false;
            inEndZone = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(sail.position, sail.position + windCurrent * 10);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(sail.position, sail.position + sailDirection * 10);
        Gizmos.DrawLine(sail.position, sail.position + -sailDirection * 10);
    }
}
