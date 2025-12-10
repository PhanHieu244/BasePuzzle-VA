using UnityEngine;
using System.Collections;

public class PositionScript : MonoBehaviour
{
    public Vector3 initialLocalPosition;
    public Vector3 targetLocalPosition;
    private bool isMoved;
    public Collider tube;
    private bool isColliding;
    public CollisionChecker checker;
    public float duration;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        isMoved = false;
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
                        // Toggle the position
                        if (isMoved)
                            MoveToInitial();
                        else
                            MoveToTarget();
                    }
                }
            }
            else
            {
                // Handle collision case
                Debug.Log("Cannot move due to collision");
            }
        }
    }

    void MoveToTarget()
    {
        StopAllCoroutines(); // Stop any ongoing movements
        StartCoroutine(MoveTo(targetLocalPosition));
        isMoved = true;
    }

    public void MoveToInitial()
    {
        StopAllCoroutines(); // Stop any ongoing movements
        StartCoroutine(MoveTo(initialLocalPosition));
        isMoved = false;
    }

    IEnumerator MoveTo(Vector3 targetPos)
    {
        float t = 0f; // Interpolation parameter
        Vector3 startPos = transform.localPosition;
        isColliding = false;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 newPosition = Vector3.Lerp(startPos, targetPos, t);

            transform.localPosition = newPosition;
            yield return null;
        }
    }
}
