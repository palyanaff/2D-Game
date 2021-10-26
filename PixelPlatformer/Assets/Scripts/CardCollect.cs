using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollect : MonoBehaviour
{
    [SerializeField] GameObject cardImg;
    public static int cardCount;

    void Start()
    {
        cardCount = 0;
        cardImg.SetActive(false);
    }

    void Update()
    {
        if (cardCount > 0)
        {
            cardImg.SetActive(true);
        }
        else
        {
            cardImg.SetActive(false);
        }
    }
}
