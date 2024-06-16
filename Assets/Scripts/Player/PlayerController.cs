using System;
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static Action<PlayerController> OnPlayerReady;

    [Header("Parameters")]
    [SerializeField] float movingSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCooldown;
    [SerializeField] float dashDuration;
    [SerializeField] AnimationCurve dashCurve;
    [Header("Refs")]
    [SerializeField] Transform meshTransform;
    [SerializeField] private Animator animator; //ByEma


    Rigidbody rb;
    float curveDuration;
    bool dashIsAvailable;
    bool isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        OnPlayerReady?.Invoke(this);
        dashIsAvailable = true;
    }

    private void FixedUpdate()
    {
        if (InputManager.IsMoving(out Vector3 direction) && !isDashing)
        {
            rb.velocity = new Vector3(direction.x * movingSpeed, 0, direction.z * movingSpeed);
        }
        else if (!InputManager.IsMoving(out _) && !isDashing)
        {
            rb.velocity = Vector3.zero;
        }
        else if (InputManager.IsMoving(out _) && isDashing)
        {
            rb.velocity = dashCurve.Evaluate(curveDuration / dashDuration) * dashSpeed * rb.velocity.normalized;
        }
        else if(!InputManager.IsMoving(out _) && isDashing)
        {
            rb.velocity = dashCurve.Evaluate(curveDuration / dashDuration) * dashSpeed * meshTransform.forward;
        }
    }

    private void Update()
    {
        GameManager.enemyTargetPosition = transform.position;

        if (InputManager.ActionMap.Player.Dash.WasPerformedThisFrame() && dashIsAvailable)
        {
            ApplyDash();
            animator.SetTrigger("Dash"); //ByEma
        }
    }

    void ApplyDash()
    {
        isDashing = true;
        dashIsAvailable = false;
        StartCoroutine(Curve());
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashIsAvailable = true;
    }

    IEnumerator Curve()
    {
        curveDuration = 0;
        while (curveDuration < dashDuration)
        {
            curveDuration += Time.deltaTime;
            rb.velocity = dashCurve.Evaluate(curveDuration / dashDuration) * dashSpeed * rb.velocity.normalized;
            yield return null;
        }
        isDashing = false;
    }


}
