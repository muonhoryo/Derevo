

using UnityEngine;

namespace Derevo.GUI
{
    public sealed class CameraMovement : MonoBehaviour
    {
        [SerializeField] private GameObject MovedCameraObj;
        [SerializeField] private float Sensitive;
        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.W))
                MovedCameraObj.transform.position += new Vector3(0, Sensitive, 0);
            else if (Input.GetKey(KeyCode.S))
                MovedCameraObj.transform.position += new Vector3(0, -Sensitive, 0);

            if (Input.GetKey(KeyCode.D))
                MovedCameraObj.transform.position += new Vector3(Sensitive, 0, 0);
            else if (Input.GetKey(KeyCode.A))
                MovedCameraObj.transform.position += new Vector3(-Sensitive, 0, 0);
        }
    }
}