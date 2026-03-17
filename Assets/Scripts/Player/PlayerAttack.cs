using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using Unity.VisualScripting;
using System.Dynamic;

public class PlayerAttack : MonoBehaviour
{

    private PlayerMovement player;

    public Animator anim;

    //Melee attack variables
    [SerializeField] public float attackCooldown = 1f;
    [SerializeField] public float attackOffset = 0.8f;
    [SerializeField] public float attackDuration = 0.3f;
    bool canAttack = true;
    public GameObject Melee;

    //Ranged attack variables
    public Transform Aim;
    public GameObject SpearPrefab;
    public float SpearSpeed = 10f;
    public float SpearCooldown = 2f;
    public bool canSpear = false;
    public Vector2 dir;

    float x;
    float y;

    string direction;

    [SerializeField] private float meleeVolume = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
        Aim = transform.Find("Aim");
        Melee.SetActive(false);

        canSpear = StaticData.canSpear;
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

    public void Spear(string direction)
    {
        if (canSpear)
        {
             dir = GetDirectionVector(direction).normalized;

            canSpear = false;
            Debug.Log("Spear Attack Triggered");
            GameObject intSpear = Instantiate(SpearPrefab, Aim.position, Quaternion.identity);

            //Rotate spear
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            intSpear.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

            //Shoot
            intSpear.GetComponent<Rigidbody2D>().AddForce(dir * SpearSpeed, ForceMode2D.Impulse);
            Destroy(intSpear, 5f);
            StartCoroutine(SpearCoroutine());
        }
    }

    private IEnumerator SpearCoroutine()
    {
        yield return new WaitForSeconds(SpearCooldown);
        canSpear = true;
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        //Play attack animation based on direction
        anim.SetTrigger("Attack");

        SoundManager.instance.PlaySoundFXClip("ReaperMelee", transform, meleeVolume);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private IEnumerator MeleeActivationCoroutine()
    {
        Melee.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        Melee.SetActive(false);
    }

    private Vector2 GetDirectionVector(string direction)
    {
        return direction switch
        {
            "Up" => Vector2.up,
            "Down" => Vector2.down,
            "Left" => Vector2.left,
            "Right" => Vector2.right,
            "UpLeft" => new Vector2(-1, 1),
            "UpRight" => new Vector2(1, 1),
            "DownLeft" => new Vector2(-1, -1),
            "DownRight" => new Vector2(1, -1),
            _ => Vector2.right,
        };
    }

}