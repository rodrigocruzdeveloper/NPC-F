using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float speed;

    private Vector2 direction;
    private bool rolling;
    private bool attacking;

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private Collider2D collider2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();   
        collider2D = GetComponent<Collider2D>();
    }


    void Start()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.freezeRotation = true;
    }

    
    void Update()
    {
        Move();
        Roll();
        Attack();

        Animations();
    }

    private void FixedUpdate()
    {
        OnMove();
    }
    
    void Move()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        if(direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0.0f, 0.0f);
        }
        else if(direction.x < 0)
        {
            transform.eulerAngles = new Vector2(0.0f, 180.0f);
        }
    }

    void OnMove()
    {
        rigidbody2D.linearVelocity = direction.normalized * speed;
    }

    void Roll()
    {
        rolling = direction.x  != 0.0f && Input.GetButtonDown("Jump");

        if(rolling == true && collider2D.enabled == true)
        {
            StartCoroutine(Invencible());
        }
    }

    IEnumerator Invencible()
    {
        collider2D.enabled = false;
        yield return new WaitForSeconds(0.9f);
        collider2D.enabled = true;
    }

    void Attack()
    {
        attacking = Input.GetButtonDown("Fire1");
    }


    IEnumerator OnAttack()
    {
        attacking = true;
        yield return new WaitForSeconds(1.5f);
        attacking = false;
    }


    void Animations()
    {
        animator.SetInteger("pMove", (int)(Mathf.Abs(direction.x) + Mathf.Abs(direction.y)));
        animator.SetBool("pRoll", rolling);
        animator.SetBool("pAttack", attacking);
    }

}
