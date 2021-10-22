using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerMove.Instance.gameObject)
        {
            PlayerMove.Instance.GetDamage();
        }
    }

}
