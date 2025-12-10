using UnityEngine;
using System.Collections;
using DxCoder;
public class RotationScript : MonoBehaviour
{
    public float initialZRotation;
    public float targetZRotation;
    private bool isRotated;
    public Collider tube;
    private bool isColliding;
    public CollisionChecker checker;
    public float duration;

   
    void Start()
    {
        initialZRotation = transform.eulerAngles.z;
        isRotated = false;
        isColliding = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (checker.CheckCollision())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == tube)
                    {
                        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

                        // Toggle the rotation
                        if (isRotated)
                            RotateToInitial();
                        else
                            RotateToTarget();
                    }
                }
            }
            else {
                //StartCoroutine(PingPongSpriteColor());
                Debug.Log("Notdd");
            }
        }
    }

    void RotateToTarget()
    {
      
        StopAllCoroutines(); // Stop any ongoing rotations
        StartCoroutine(RotateTo(targetZRotation));
        isRotated = true;
    }

    public void RotateToInitial()
    {
      
        StopAllCoroutines(); // Stop any ongoing rotations
        StartCoroutine(RotateTo(initialZRotation));
        isRotated = false;
    }

    IEnumerator RotateTo(float targetRotation)
    {
        float t = 0f; // Interpolation parameter
        float startRotation = transform.eulerAngles.z;
        isColliding = false;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float newZRotation = Mathf.SmoothStep(startRotation, targetRotation, t);

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
            yield return null;
        }
    }


    
}

   

   

