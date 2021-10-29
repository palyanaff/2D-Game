using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore1 : MonoBehaviour
{
    private Text finalScore;
    public static int finalCount;

    void Start()
    {
        finalScore = GetComponent<Text>();
        finalCount = MoneyCollect1.moneyCount;
    }

    void Update()
    {
        finalScore.text = "Score: " + finalCount;
    }
}
