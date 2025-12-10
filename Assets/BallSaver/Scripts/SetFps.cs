using UnityEngine;

public class SetFps : MonoBehaviour
{
    public int targetFrameRate = 60;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
