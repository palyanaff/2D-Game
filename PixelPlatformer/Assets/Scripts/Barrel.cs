using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Entity
{
    private void Start()
    {
        lives = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerMove.Instance.gameObject)
        {
            lives--;
            Debug.Log("У бочки: " + lives);
        }

        if (lives < 1)
        {
            Die();
        }
    }
}
