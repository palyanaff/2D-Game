using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    private float speed = 3f;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private bool faceRight = true;
    private Animator anim;


    private void Start()    
    {
        anim = GetComponent<Animator>();
        dir = transform.right;
        lives = 2;
    }

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);

        if (colliders.Length > 0)
        {
            dir *= -1f;
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lives > 0 && collision.gameObject == PlayerMove.Instance.gameObject)
        {
            PlayerMove.Instance.GetDamage();
        }
        if (lives < 1)
            Die();
    }

    public override void Die()
    {
        speed = 0f;
        anim.SetTrigger("Death");
        Destroy(this.gameObject);
    }
}
