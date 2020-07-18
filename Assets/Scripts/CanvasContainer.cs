using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasContainer : MonoBehaviour
{
    public Canvas endGamePanel;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public Canvas getEndGamePanel()
    {
        return endGamePanel;
    }
}
