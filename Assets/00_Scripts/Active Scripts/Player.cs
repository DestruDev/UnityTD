using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player Stats")]
    public float moveSpeed = 1f;          // initial walking speed
    public float interactRange = 8f;

    private float baseMoveSpeed;          // remembers original speed
    private const float sprintMultiplier = 1.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private bool isFacingRight;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        baseMoveSpeed = moveSpeed;        // cache starting speed
    }

    void Update() {
        // ������ Handle sprint toggle ����������������������������������������������������������������
        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        moveSpeed = sprinting ? baseMoveSpeed * sprintMultiplier : baseMoveSpeed;
        if (sprinting && moveInput.sqrMagnitude > 0.01f) {
            animator.speed = sprintMultiplier;
        } else {
            animator.speed = 1f;
        }

        // ������ Movement input & walking animation flag ��������������������������
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isWalking", moveInput.sqrMagnitude > 0.01f);

        // ������ Flip sprite when changing direction ����������������������������������
        if (!isFacingRight && moveInput.x < 0f) Flip();
        else if (isFacingRight && moveInput.x > 0f) Flip();
    }

    void FixedUpdate() {
        Vector2 moveAmount = moveInput.normalized * moveSpeed * Time.fixedUnscaledDeltaTime;
        rb.MovePosition(rb.position + moveAmount);
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}