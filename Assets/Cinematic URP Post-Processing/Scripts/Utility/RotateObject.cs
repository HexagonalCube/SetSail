using UnityEngine;

namespace PRISM.Utils
{
    public class RotateObject : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 14.0f, Space.World);
        }
    }
}
