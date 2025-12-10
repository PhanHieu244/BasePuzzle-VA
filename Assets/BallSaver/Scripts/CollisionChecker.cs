using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public Collider Ring;

     GameObject[] movableObjects;
    Collider colliderToCheck;
    Collider[] allColliders;
    private List<GameObject> gameObjectsInsideTrigger = new List<GameObject>();

    public void Start()
    {
        colliderToCheck = GetComponent<Collider>();

      //  movableObjects = GameObject.FindGameObjectsWithTag("Movable");
       
    }

    public bool CheckCollision()
    {
            foreach (GameObject otherCollider in gameObjectsInsideTrigger)
              {
                        // Skip if the otherCollider is a ring
                        if (otherCollider.gameObject.Equals(Ring.gameObject))
                            {

                                continue;
                            }

                        if (!otherCollider.gameObject.Equals(Ring.gameObject))
                        {
                             Debug.Log(otherCollider.gameObject.name);

                             return false;
                        }
        }
        

        return true;
    }



    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Movable"))
        {
            GameObject enteredObject = other.gameObject;

            // Check if the entered object is not already in the list
            if (!gameObjectsInsideTrigger.Contains(enteredObject))
            {
                // Add the entered object to the list
                gameObjectsInsideTrigger.Add(enteredObject);

                // Optionally, you can perform further processing or actions here
                int r = gameObjectsInsideTrigger.Count;
             //   Debug.Log(r);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Movable"))
        {
            GameObject exitedObject = other.gameObject;

            // Remove the exited object from the list
            gameObjectsInsideTrigger.Remove(exitedObject);

            // Optionally, you can perform further processing or actions here
        }
    }

}
