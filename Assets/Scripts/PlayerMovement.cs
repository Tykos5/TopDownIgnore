using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.VisualScripting;
using System.Dynamic;

public class Player : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashCooldown = 1f;
    bool isDashing = false;
    bool canDash = true;
    TrailRenderer trailRenderer;

    [SerializeField] private float attackCooldown = 1f;
    bool canAttack = true;

    public Animator anim;
    
    private Vector2 _movement;

    private Rigidbody2D _rb;

    private bool moving;

    private Vector2 input;

    string direction;
    string lastDirection = "Down";

    private float x;
    private float y;

    public EnemyAI zombie;

    private bool inZombieTrigger = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        GetInput();
        Animate();
        direction = GetDirection();

        if (isDashing)
            return;

        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        _rb.linearVelocity = _movement * _moveSpeed;   
    }

    public void Dash()
    {
        if (canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            x = direction switch
            {
                "Up" => 0,
                "Down" => 0,
                "Left" => -1,
                "Right"  => 1,
                "UpLeft" => -1,
                "UpRight" => 1,
                "DownLeft" => -1,
                "DownRight" => 1,
                _ => 0,
            };

            y = direction switch
            {
                "Up" => 1,
                "Down" => -1,
                "Left" => 0,
                "Right" => 0,
                "UpLeft" => 0,
                "UpRight" => 0,
                "DownLeft" => 0,
                "DownRight" => 0,
                _ => 1,
            };


            anim.SetFloat("AttackX", x);
            anim.SetFloat("AttackY", y);
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        //trailRenderer.emitting = true;

        //Dash in the direction the player is currently moving or last moved, get direction from GetDirection()
        
        _rb.linearVelocity = direction switch   //dasgh in the direction the player is facing
        {
            "Up" => new Vector2(0, dashSpeed),
            "Down" => new Vector2(0, -dashSpeed),
            "Left" => new Vector2(-dashSpeed, 0),
            "Right" => new Vector2(dashSpeed, 0),
            "UpLeft" => new Vector2(-dashSpeed / Mathf.Sqrt(2), dashSpeed / Mathf.Sqrt(2)),
            "UpRight" => new Vector2(dashSpeed / Mathf.Sqrt(2), dashSpeed / Mathf.Sqrt(2)),
            "DownLeft" => new Vector2(-dashSpeed / Mathf.Sqrt(2), -dashSpeed / Mathf.Sqrt(2)),
            "DownRight" => new Vector2(dashSpeed / Mathf.Sqrt(2), -dashSpeed / Mathf.Sqrt(2)),
            _ => new Vector2(0, 0),
        };
        yield return new WaitForSeconds(dashDuration);

        _rb.linearVelocity = new Vector2(0,0);// Stop movement after dash  

        isDashing = false;
        //trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        //Play attack animation based on direction
        anim.SetTrigger("Attack");

        //Here you can add code to deal damage to enemies in range based on direction
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void GetInput()
    {
        x = InputManager.Movement.x;
        y = InputManager.Movement.y;

        input = new Vector2(x, y);
        input.Normalize();

        if (InputManager.DashPressed)
        {
            Debug.Log("Dash pressed in Player script");
            Dash();
        }

        if (InputManager.AttackPressed)
        {
            Debug.Log ("Attack pressed in Player script");
            Attack();
        }
    }
    private void Animate()
    {
        if (input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
        }
        anim.SetBool("Moving", moving);
    }

    private string GetDirection() // Returns "Up", "Down", "Left", "Right", "UpLeft", "UpRight", "DownLeft" or "DownRight" based on input x and y values
    {
        if (y > 0 && x == 0)
        {
            lastDirection = "Up";
            return "Up";
        }
        else if (y < 0 && x == 0)
        {
            lastDirection = "Down";
            return "Down";
        }
        else if (x < 0 && y == 0)
        {
            lastDirection = "Left";
            return "Left";
        }
        else if (x > 0 && y == 0)
        {
            lastDirection = "Right";
            return "Right";
        }
        else if (x > 0 && y > 0)
        {
            lastDirection = "UpRight";
            return "UpRight";
        }
        else if (x < 0 && y > 0)
        {
            lastDirection = "UpLeft";
            return "UpLeft";
        }
        else if (x < 0 && y < 0)
        {
            lastDirection = "DownLeft";
            return "DownLeft";
        }
        else if (x > 0 && y < 0)
        {
            lastDirection = "DownRight";
            return "DownRight";
        }
        else
            return lastDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ZombieTrigger"))
        {
            Debug.Log("Player entered zombie trigger");

            zombie.StartChasing(transform); // send PLAYER transform
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ZombieTrigger"))
        {
            Debug.Log("Player exited zombie trigger");

            zombie.StopChasing();
        }
    }
}
