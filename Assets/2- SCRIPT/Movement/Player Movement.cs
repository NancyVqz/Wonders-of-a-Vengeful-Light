using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    public Joystick joy;
    private Vector2 movement;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 7f;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimeLeft;
    private float dashCooldownTimeLeft;
    private Vector2 dashDirection;
    private Animator animator;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement = new Vector2(joy.Horizontal, joy.Vertical);

        UpdateMovementAnimation();

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && canDash && movement.magnitude > 0.1f)
        {
            StartDash();
        }

        if (!canDash)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
            if (dashCooldownTimeLeft <= 0)
            {
                canDash = true;
            }
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Vector2 newPosition = rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
        else
        {
            Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    private void UpdateMovementAnimation()
    {
        if (animator == null) return;

        // Si no se está moviendo, idle
        if (movement.magnitude < 0.1f)
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", false);
        }
        // Si se mueve a la derecha
        else if (movement.x > 0.1f)
        {
            animator.SetBool("isMovingRight", true);
            animator.SetBool("isMovingLeft", false);
        }
        // Si se mueve a la izquierda
        else if (movement.x < -0.1f)
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", true);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        dashCooldownTimeLeft = DashDuration();
        dashDirection = movement.normalized;
    }

    private void EndDash()
    {
        isDashing = false;
    }

    public bool CanDash()
    {
        return canDash;
    }

    public void ButtonDash()
    {
        if (canDash && movement.magnitude > 0.1f)
        {
            StartDash();
        }
    }

    public float GetDashCooldownPercent()
    {
        if (canDash) return 1f;
        return 1f - (dashCooldownTimeLeft / DashDuration());
    }

    public float DashDuration()
    {
        switch (GameManager.instance.dashLvl)
        {
            case 1:
                dashCooldown = 7;
                break;
            case 2:
                dashCooldown = 5;
                break;
            case 3:
                dashCooldown = 3;
                break;
            case 4:
                dashCooldown = 1;
                break;
        }
        return dashCooldown;
    }

}
