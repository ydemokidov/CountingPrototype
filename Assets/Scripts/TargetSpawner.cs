using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private float xRange;
    private float spawnInterval = 3.0f;
    private float markerInterval = 2.0f;

    private List<GameObject> pooledTargets;
    public GameObject[] targetsToPool;

    public GameObject markerArrow;
    private List<GameObject> pooledMarkers;
    public int amountToPool;

    private bool spawning = false;


    // Start is called before the first frame update
    public void Init(int difficulty)
    {
        this.spawnInterval = spawnInterval / (difficulty + 1); ;
        this.markerInterval = markerInterval / (difficulty + 1); ;

        pooledTargets = new List<GameObject>();
        pooledMarkers = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            int index = Random.Range(0, targetsToPool.Length);
            tmp = Instantiate(targetsToPool[index], transform.position, targetsToPool[index].transform.rotation);
            tmp.SetActive(false);
            pooledTargets.Add(tmp);

            tmp = Instantiate(markerArrow, transform.position, markerArrow.transform.rotation);
            tmp.SetActive(false);
            pooledMarkers.Add(tmp);
        }
    }

    public Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-xRange, xRange), transform.position.y, 0);
    }

    public GameObject GetPooledObject(List<GameObject> pooledObjects)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    IEnumerator SpawnTarget()
    {
        yield return new WaitForSeconds(spawnInterval);
        if (spawning)
        {
            GameObject marker = GetPooledObject(pooledMarkers);

            Vector3 spawnPosition = GetSpawnPosition();

            marker.transform.position = spawnPosition + new Vector3(0, -2, 0);
            marker.SetActive(true);
            yield return new WaitForSeconds(markerInterval);
            marker.SetActive(false);

            GameObject target = GetPooledObject(pooledTargets);
            target.transform.position = spawnPosition;
            target.SetActive(true);
            StartCoroutine(SpawnTarget());
        }
    }

    public void BeginSpawn()
    {
        spawning = true;
        StartCoroutine(SpawnTarget());
    }

    public void StopSpawn()
    {
        spawning = false;
    }

}
