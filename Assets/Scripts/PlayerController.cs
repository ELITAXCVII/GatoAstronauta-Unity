using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    // Tamaño del gato (el original era 1). Constante para que la escena no guarde otro valor.
    const float SIZE = 1.5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    CapsuleCollider2D capsule;
    float moveInput;
    bool jumpRequested;

    void Awake()
    {
        // El collider y el GroundCheck (hijo) crecen junto con el sprite
        transform.localScale = new Vector3(SIZE, SIZE, 1f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0f) sr.flipX = false;
        else if (moveInput < 0f) sr.flipX = true;

        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        bool jumpKey = Input.GetButtonDown("Jump")
                    || Input.GetKeyDown(KeyCode.UpArrow)
                    || Input.GetKeyDown(KeyCode.W);
        if (jumpKey && IsGrounded())
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    bool IsGrounded()
    {
        Vector2 origin = groundCheck
            ? (Vector2)groundCheck.position
            : capsule
                ? (Vector2)transform.position + capsule.offset - new Vector2(0f, capsule.size.y * 0.5f + 0.05f)
                : (Vector2)transform.position + Vector2.down * 0.4f;
        return Physics2D.OverlapCircle(origin, 0.12f, groundLayer);
    }
}
