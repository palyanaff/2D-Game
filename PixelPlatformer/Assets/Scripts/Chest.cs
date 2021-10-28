using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    private bool isOpened = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerMove.Instance.gameObject && CardCollect.cardCount > 0 && isOpened == false)
        {
            isOpened = true;
            ChestAnim();
        }
    }

    public void ChestAnim()
    {
        anim.SetTrigger("isOpened");
    }
}
