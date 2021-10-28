using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [SerializeField] GameObject finishPanel;

    private void Awake()
    {
        finishPanel.SetActive(false);
    }

    public void SetFinish()
    {
        finishPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
