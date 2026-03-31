using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class AsteroidManager : MonoBehaviour
{
    [SerializeField]
    private int m_startingAsteroids;
    [SerializeField]
    private int m_maximumAsteroids;
    [SerializeField]
    private List<GameObject> m_asteroidsPrefabs;

    [SerializeField]
    private GameObject m_collectiblePrefabs;
    [SerializeField]
    private int[] m_numToSpawnOnDeath;
    [SerializeField]
    private Rect m_spawnArea;
    [SerializeField]
    private float m_asteroidSpawnDelay;
    
    private int m_currentAsteroidCount;

    private void OnEnable()
    {
        GameEvents.Instance.onGameOver += OnGameOver;
        GameEvents.Instance.onRetry += OnRetry;
    }
    
    private void OnDisable()
    {
        
    }
    
    private void Start()
    {

        OnRetry();
    }
    private IEnumerator SpawnInitialAsteroids()
     {
        for (int i = 0; i< m_startingAsteroids; i++)
        {
            yield return new WaitForSeconds(0.1f);
           
            SpawnRandomAsteroid(3, GetSpawnPointRandom());
        }
        StartCoroutine(AsteroidSpawner());
    }
    private IEnumerator AsteroidSpawner()
    {
        while(m_currentAsteroidCount >= m_maximumAsteroids)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(m_asteroidSpawnDelay);
        SpawnRandomAsteroid(3, GetSpawnPointRandom());
        StartCoroutine(AsteroidSpawner());
    }
    
    private Vector2 GetSpawnPointRandom()
    {
        int side = Random.Range(0, 4);

        float x = 0;
        float y = 0;
        switch(side)
        {
            //arriba
            case 0:
                x = Random.Range(m_spawnArea.xMin, m_spawnArea.xMax);
                y = m_spawnArea.yMax;
                break;
            //derecha
            case 1:
                y = m_spawnArea.xMax;
                x = Random.Range(m_spawnArea.yMin, m_spawnArea.yMax);
                break;
            //abajo
            case 2:
                x = Random.Range(m_spawnArea.xMin, m_spawnArea.xMax);
                y = m_spawnArea.yMin;
                break;
            //izquierda
            case 3:
                y = m_spawnArea.xMin;
                x = Random.Range(m_spawnArea.yMin, m_spawnArea.yMax);
                break;
        }

    
        return new Vector2(x, y);
    }
    private void SpawnRandomAsteroid(int size, Vector2 spawnPoint)
    {
        IEnumerable<GameObject> sizePrefabs = m_asteroidsPrefabs.Where((x)=> x.GetComponent<AsteroidController>().Size == size);
        if (sizePrefabs == null || sizePrefabs.Count() <= 0) { return; }
        int index = Random.Range(0, sizePrefabs.Count());
        GameObject asteroidToSpawn = Instantiate(sizePrefabs.ElementAt(index),transform);
        
        asteroidToSpawn.transform.position = spawnPoint;
        AsteroidController controller = asteroidToSpawn.GetComponent<AsteroidController>();
        controller.onAsteroidDie += OnAsteroidDie;
        if (size == 3)
        {
            m_currentAsteroidCount++;
        }
    }

    private void OnAsteroidDie(AsteroidController asteroid)
    {
        int size = asteroid.Size;
        Vector2 asteroidPoint = asteroid.transform.position;
        Destroy(asteroid.gameObject);
        GameEvents.Instance.AddToScore(1);
        if (size == 3)
        {
            m_currentAsteroidCount--;
        }
        size--;
        int numToSpawn = m_numToSpawnOnDeath[size];
        if (size > 0)
        {
            SpawnCollectible(asteroidPoint);
            for (int i = 0; i < numToSpawn; i++)
            {
                SpawnRandomAsteroid(size,( Random.insideUnitCircle * 5f) + asteroidPoint);
            }
        }
    }
    private void SpawnCollectible(Vector3 position)
    {

        if(Random.Range(0f, 1f) > 0.5f)
        {
            GameObject collectible = Instantiate(m_collectiblePrefabs, transform);
            collectible.transform.position = position;
        }


    }
    private void OnGameOver()
    {
        StopAllCoroutines();
        m_currentAsteroidCount = 0;
    }
    private void OnRetry()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        StartCoroutine(SpawnInitialAsteroids());
    }
}
