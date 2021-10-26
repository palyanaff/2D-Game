using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCollect1 : MonoBehaviour
{
    public static int moneyCount;
    private Text moneyCounter;
    
    void Start()
    {
        moneyCounter = GetComponent<Text>();
        moneyCount = 0;
    }

    void Update()
    {
        moneyCounter.text = "Score: " + moneyCount;
    }
}
