using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Events;

public class TriggerTile : MonoBehaviour
{

    public GameObject nextCoffin;
    public float spawnDelay = 0.2f;

    //private bool isActive = true;


    public GameObject tilemapObject;

    public UnityEvent spawnEvent;

    public void TriggerSpawn()
    {
        tilemapObject.SetActive(true);
        Debug.Log("Dörr??????????");
    }

    public void TriggerDespawn()
    {
        tilemapObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D PlayerAttack)
    {
        spawnEvent.Invoke();

    }
}