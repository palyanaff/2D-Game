using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Entity
{
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sr;
    private int health;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;

    public static PlayerMove Instance { get; set; }

    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        isRecharged = true;
        lives = 5;
        health = lives;
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
        
        if (health > lives)
        {
            health = lives;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = aliveHeart;
            }
            else
            {
                hearts[i].sprite = deadHeart;
            }

            if (i < lives)
            {
                hearts[i].enabled = true; 
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        laugeBar.SetMaxLauge(2);

    }

    void FixedUpdate()
    {
        
    }

    public Vector2 moveVector;
    public float speed = 5f;
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
    public LaugeBar laugeBar;
    public float laugeCD;

    void Lunge()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !lockLunge)
        {
            lockLunge = true;
            laugeCD = 0;
            laugeBar.SetLauge(laugeCD);
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
        laugeCD = 2;
        laugeBar.SetLauge(laugeCD);
        lockLunge = false;
    }

    public override void GetDamage()
    {
        health--;
        

        if (health == 0)
        {
            foreach (var h in hearts)
                h.sprite = deadHeart;
            Die();
        }

        if (health > 0) { 
            anim.StopPlayback();
            anim.Play("Damage");
        }
        
    }

    public override void Die()
    {
        anim.StopPlayback();
        anim.Play("Die");
        //Time.timeScale = 0;
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
            StartCoroutine(EnemyOnAttack(colliders[i]));
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
            CardCollect.cardCount++;
            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("Chest"))
        {
            if (CardCollect.cardCount > 0)
            {
                CardCollect.cardCount--;
                MoneyCollect1.moneyCount += 500;
                Destroy(collision.gameObject);
            }
            
        }

        if (collision.tag.Equals("Finish"))
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0;
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

    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        enemyColor.color = new Color(0.79f, 0f, 0f);
        yield return new WaitForSeconds(0.2f);
        if (enemy != null)
        {
            enemyColor.color = new Color(1, 1, 1); 
        }
    }

}
