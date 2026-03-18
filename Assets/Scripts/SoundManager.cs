using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private AudioClip audioclip;


    // All sound effects
    [Header("Music")]
    [SerializeField] private AudioClip Level1Music;
    [SerializeField] private AudioClip Level2Music;
    [SerializeField] private AudioClip Level3Music;
    [SerializeField] private AudioClip Level4Music;

    [Header("Player")]
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerMelee;
    [SerializeField] private AudioClip playerSpear;
    [SerializeField] private AudioClip victory;

    [Header("Reaper")]
    [SerializeField] private AudioClip reaperDeath;
    [SerializeField] private AudioClip reaperHit;
    [SerializeField] private AudioClip reaperMelee;
    [SerializeField] private AudioClip reaperOrb;

    [Header("Zombie")]
    [SerializeField] private AudioClip zombieDeath;
    [SerializeField] private AudioClip zombieHit;
    [SerializeField] private AudioClip zombieMelee;

    [Header("Boss")]
    [SerializeField] private AudioClip rockDeath;
    [SerializeField] private AudioClip rockHit;
    [SerializeField] private AudioClip rockMelee;
    [SerializeField] private AudioClip rockOrb;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(string audioclipString, Transform spawnTransform, float volume)
    {
        audioclip = audioclipString switch
        {
            "Level1Music" => Level1Music,
            "Level2Music" => Level2Music,
            "Level3Music" => Level3Music,
            "Level4Music" => Level4Music,
            "Dash" => dash,
            "PlayerDeath" => playerDeath,
            "PlayerHit" => playerHit,
            "PlayerMelee" => playerMelee,
            "PlayerSpear" => playerSpear,
            "Victory" => victory,
            "ReaperDeath" => reaperDeath,
            "ReaperHit" => reaperHit,
            "ReaperMelee" => reaperMelee,
            "ReaperOrb" => reaperOrb,
            "ZombieDeath" => zombieDeath,
            "ZombieHit" => zombieHit,
            "ZombieMelee" => zombieMelee,
            "RockDeath" => rockDeath,
            "RockHit" => rockHit,
            "RockMelee" => rockMelee,
            "RockOrb" => rockOrb,
            _ => null
        };

        if (audioclip == null)
        {
            Debug.LogWarning("SoundManager: No clip found for key: " + audioclipString);
            return;
        }

        //Spawn gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign audioclip
        audioSource.clip = audioclip;

        //Assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get Length of clip
        float clipLength = audioclip.length;

        //destroy gameobject after clip length
        Destroy(audioSource.gameObject, clipLength);
    }
}
