using UnityEngine;
using System.Collections;

public class TriggerTile : MonoBehaviour
{
    public GameObject tilemapObject;

    //public void SpawnTilemap()
    //{
    //    if (spawnedTilemap == null)
    //    {
    //        spawnedTilemap = Instantiate(tilemapPrefab, transform.position, Quaternion.identity);
    //    }
    //}

    public void TriggerSpawn()
    {
        tilemapObject.SetActive(true);
        Debug.Log("Dörr??????????");
    }

    public void TriggerDespawn()
    {
        tilemapObject.SetActive(false);
    }
}
