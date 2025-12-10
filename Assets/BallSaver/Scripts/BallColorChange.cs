using UnityEngine;

public class BallColorChange : MonoBehaviour
{
   // public Material newMaterial; // The new material or color to apply
   
    private bool colorChanged = false; // To track if the color has been changed already
    BallInfo ballinfo;


    public void Start()
    {
        ballinfo = gameObject.GetComponent<BallInfo>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!colorChanged && collision.gameObject.CompareTag("Ball"))
        {
            if (!collision.gameObject.GetComponent<BallInfo>().GetTag().Equals("WhiteBall"))
            {
                Renderer renderer = GetComponent<Renderer>();

                ballinfo.SetTag(collision.gameObject.GetComponent<BallInfo>().GetTag());
                Renderer collidingRenderer = collision.gameObject.GetComponent<Renderer>();
                Material newMaterial = collidingRenderer.material;
                // Change the material or color of the sphere
                if (renderer != null)
                {
                    renderer.sharedMaterial = newMaterial; // Change material
                }


                colorChanged = true; // Set the flag to prevent further color changes
                                     //GetComponent<BallColorChange>().enabled = false;
            }
        }
    }
}
