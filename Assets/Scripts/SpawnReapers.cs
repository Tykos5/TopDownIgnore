using UnityEngine;

public class SpawnReapers : MonoBehaviour
{
    public GameObject meeleReaper; // Reference to the meele reaper 
    public GameObject rangedReaper; // Reference to the ranged reaper 

    void Start()
    {
        meeleReaper.SetActive(false); // Disable the meele reaper at the start
        rangedReaper.SetActive(false); // Disable the ranged reaper at the start
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //spawn the reapers on trigger with the player
        if (collision.CompareTag("Player"))
        {
            meeleReaper.SetActive(true); // Enable the meele reaper
            rangedReaper.SetActive(true); // Enable the ranged reaper
        }
    }
}
