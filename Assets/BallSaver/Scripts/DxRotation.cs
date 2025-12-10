using UnityEngine;

public class DxRotation : MonoBehaviour
{
    public float rotationSpeed = 60f; // Adjust the rotation speed as needed

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
