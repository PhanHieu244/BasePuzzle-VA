using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadTextureCoroutine : MonoBehaviour
{
    public Coroutine currentCoroutine = null;
    public UnityWebRequest uwr;

    private void AbortDownload()
    {
        if (uwr != null && !uwr.isDone)
        {
            uwr.Abort();
        }
    }
    private void StopCurrentCoroutine()
    {
        if (currentCoroutine != null)
        {
            AGameController.Instance.StopCoroutine(currentCoroutine);
        }
    }
    public void ResetAndRunCoroutine(IEnumerator couroutine, UnityWebRequest uwr)
    {
        StopPreLoading();
        this.uwr = uwr;
        currentCoroutine = AGameController.Instance.StartCoroutine(couroutine);
    }

    public void StopPreLoading()
    {
        AbortDownload();
        StopCurrentCoroutine();
    }

}
