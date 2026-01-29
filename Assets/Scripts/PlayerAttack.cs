using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.VisualScripting;
using System.Dynamic;

public class PlayerAttack : MonoBehaviour
{

    private PlayerMovement player;

    public Animator anim;

    [SerializeField] public float attackCooldown = 1f;
    [SerializeField] public float attackOffset = 0.8f;
    [SerializeField] public float attackDuration = 0.3f;
    bool canAttack = true;
    public GameObject Melee;

    float x;
    float y;

    string direction;


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
        Melee.SetActive(false);
    }

    void Update()
    {

    }

    public void Attack(string direction)
    {
        if (canAttack)
        {
            x = direction switch
            {
                "Up" => 0,
                "Down" => 0,
                "Left" => -1,
                "Right" => 1,
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

            //rotate attack hitbox based on direction
            if (y == -1) // down
                Melee.transform.localRotation = Quaternion.Euler(0, 0, 0);
            else if (y == 1) // up
                Melee.transform.localRotation = Quaternion.Euler(0, 0, 180);
            else if (x == -1) // left
                Melee.transform.localRotation = Quaternion.Euler(0, 0, 270);
            else if (x == 1) // right
                Melee.transform.localRotation = Quaternion.Euler(0, 0, 90);

            Vector2 dir = new Vector2(Mathf.Round(x), Mathf.Round(y));
            Melee.transform.localPosition = dir * attackOffset;

            StartCoroutine(AttackCoroutine());
            StartCoroutine(MeleeActivationCoroutine());
        }
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
    private IEnumerator MeleeActivationCoroutine()
    {
        Melee.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        Melee.SetActive(false);
    }
}