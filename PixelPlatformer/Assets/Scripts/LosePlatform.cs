using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            collision.gameObject.GetComponent<Entity>().Die();
        }
    }
}
