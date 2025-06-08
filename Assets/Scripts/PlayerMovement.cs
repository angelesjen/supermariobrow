using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rb;

    private float inputAxis;
    private Vector2 velocity;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 3f;
    public float maxJumpTime = 1.1f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.y > 0f);
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;

    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float dashTimeLeft;
    private float dashCooldownLeft;
    private bool isDashing = false;

    AudioManager AudioManager;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        HandleDashInput();
        HorizontalMovement();

        grounded = rb.Raycast(Vector2.down);

        if (grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();

    }

    private void HandleDashInput()
    {
        if (dashCooldownLeft>0)
        {
            dashCooldownLeft -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownLeft<=0 && Mathf.Abs(inputAxis)>0.1f)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashCooldownLeft = dashCooldown;
            AudioManager.PlaySFX(AudioManager.dash);
            //Debug.Log($"Dash Started! Speed: {dashSpeed}, Duration: {dashDuration}s");
        }
        /*else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(dashCooldownLeft>0)
            {
                Debug.Log($"Dash on Cooldown! {dashCooldownLeft.ToString("F2")}s remaining");
            }
            else if (Mathf.Abs(inputAxis) <= 0.1f)
            {
                Debug.Log("Can't dash - not moving (input axis too small)");
            }
        }*/

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            //Debug.Log($"Dashing! Time left: {dashTimeLeft.ToString("F2")}s");
            if(dashTimeLeft<=0)
            {
                isDashing=false;
                //Debug.Log("Dash ended naturally");
            }
        }
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        //Debug.Log($"Current Input: {inputAxis.ToString("F2")}, Grounded: {grounded}, Dashing: {isDashing}");
        if (Mathf.Abs(inputAxis) > 0.1f)
        {
            transform.eulerAngles = inputAxis > 0f ? Vector3.zero : new Vector3(0f, 180f, 0f);
        }
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * currentSpeed, moveSpeed * Time.deltaTime);

        if(rb.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
            if (isDashing)
            {
                isDashing = false;
                Debug.Log("Dash canceled - hit something");
            }
        }

    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
            AudioManager.PlaySFX(AudioManager.jump);
        }
    }

    private void ApplyGravity()
    {
        if (isDashing) return;
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;
        velocity.y += gravity * Time.deltaTime * multiplier;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        //Debug.Log($"Current velocity: X={velocity.x.ToString("F2")}, Y={velocity.y.ToString("F2")}");
        Vector2 position = rb.position;
        position.x += inputAxis * moveSpeed * Time.fixedDeltaTime;
        position.y += velocity.y * Time.fixedDeltaTime;

        Vector2 leftEdge = cam.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.75f, rightEdge.x - 0.75f);

        rb.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if(transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
        
    }

    public IEnumerator WalkIntoCastle()
    {
        Animator anim = GetComponent<Animator>();
        AudioManager AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (anim != null)
            anim.SetBool("isWalking", true);

        Vector3 startPos = transform.position;
        Vector3 step1Pos = startPos + Vector3.right * 1f;

        float timer = 0f;
        float stepTime = 0.5f;

        while (timer < 1f)
        {
            transform.position = Vector3.Lerp(startPos, step1Pos, timer);
            timer += Time.deltaTime / stepTime;
            yield return null;
        }

        transform.position = step1Pos;

        Vector3 step2Pos = step1Pos + Vector3.down * 1f;

        timer = 0f;
        float dropTime = 0.1f;

        while (timer < 1f)
        {
            transform.position = Vector3.Lerp(step1Pos, step2Pos, timer);
            timer += Time.deltaTime / dropTime;
            yield return null;
        }

        transform.position = step2Pos;

        Vector3 endPos = step2Pos + Vector3.right * 8f;
        timer = 0f;
        float walkTime = 2f;

        while (timer < 1f)
        {
            transform.position = Vector3.Lerp(step2Pos, endPos, timer);
            timer += Time.deltaTime / walkTime;
            yield return null;
        }

        if (anim != null)
            anim.SetBool("isWalking", false);

        AudioManager.PlaySFX(AudioManager.win);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("WinGame");
    }



}
