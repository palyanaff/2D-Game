using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Entity
{
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sr;
    [SerializeField] private GameObject losePanel;

    public static PlayerMove Instance { get; set; }

    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        isRecharged = true;
        lives = 5;
    }

    void Update()
    {
        
        Reflect();
        walk();
        Jump();
        CheckingGround();
        Lunge();
        if (Input.GetButtonDown("Fire1"))
            Attack();
        
    }

    void FixedUpdate()
    {
        
    }

    public Vector2 moveVector;
    public float speed = 3f;

    public bool faceRight = true;

    void walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        anim.SetFloat("moveX", Mathf.Abs(moveVector.x)); 
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }

    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    public float jumpForce = 7f;
    private bool jumpControl;
    private int jumpIteration = 0;
    public int jumpValueIteration = 60;

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            Invoke("IgnoreLayerOff", 0.5f);
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            if (onGround) { jumpControl = true; }
        }
        else { jumpControl = false; }

        if (jumpControl)
        {
            if (jumpIteration++ < jumpValueIteration)
            {
                rb.AddForce(Vector2.up * jumpForce / jumpIteration);
            }
        }
        else { jumpIteration = 0;}
    }

    void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    public bool onGround;
    public Transform GroundCheck;
    public float checkRadius = 0.25f;
    public LayerMask Ground;

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }

    public int lungeImoulse = 5000;

    void Lunge()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !lockLunge)
        {
            lockLunge = true;
            Invoke("LungeLock", 2f);

            if (lives > 0)
            {
                anim.StopPlayback();
                anim.Play("lunge");
            }

            rb.velocity = new Vector2(0, 0);

            if (!faceRight) { rb.AddForce(Vector2.left * lungeImoulse); }
            else { rb.AddForce(Vector2.right * lungeImoulse); }
        }
    }

    private bool lockLunge = false;

    void LungeLock()
    {
        lockLunge = false;
    }

    public override void GetDamage()
    {
        lives--;
        Debug.Log(lives);

        if (lives < 1)
        {
            Die();
        }

        if (lives > 0) { 
            anim.StopPlayback();
            anim.Play("Damage");
        }
        
    }

    public override void Die()
    {
        anim.StopPlayback();
        anim.Play("Die");
        Invoke("SetLosePanel", 1f);
    }

    private void SetLosePanel()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy; 

    private void Attack()
    {
        if (onGround && isRecharged)
        {
            if (lives > 0) { 
                anim.StopPlayback();
                anim.Play("Attack");
            }
            

            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Money"))
        {
            MoneyCollect1.moneyCount += 100;
            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("Card"))
        {
            CardCollect.cardCount += 1;
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

}
