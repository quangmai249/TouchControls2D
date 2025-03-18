using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void MultiTapScene()
    {
        SceneManager.LoadScene("MultiTapScene");
        return;
    }

    public void SwipeScene()
    {
        SceneManager.LoadScene("SwipeScene");
        return;
    }

    public void PinchToZoom()
    {
        SceneManager.LoadScene("PinchToZoomScene");
        return;
    }
}
