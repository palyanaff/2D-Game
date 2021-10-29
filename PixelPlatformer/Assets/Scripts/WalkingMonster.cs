using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    private float speed = 3f;
    //private Vector3 dir;
    private SpriteRenderer sprite;
    private bool faceRight = true;
    private Animator anim;
    public Transform[] points;
    public int i;


    private void Start()    
    {
        anim = GetComponent<Animator>();
        //dir = transform.right;
        lives = 2;
    }

    private void Update()
    {
        Move();
    }


    private void Move()
    {

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, points[i].position) < 0.5f)
        {
            if (i > 0)
            {
                i = 0;
                transform.localScale *= new Vector2(-1, 1);
                faceRight = !faceRight;
            }
            else
            {
                i = 1;
                transform.localScale *= new Vector2(-1, 1);
                faceRight = !faceRight;
            }
        }
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
        Invoke("DestroyBiker", 1f);
    }

    private void DestroyBiker()
    {
        Destroy(this.gameObject);
    }
}
