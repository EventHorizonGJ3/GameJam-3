using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyType currentSpawned;
    [SerializeField][Tooltip("Ritardo di Spawn in secondi reali")] float spawnFrequancy;
    [Header("Eventi di Spawn")]
    [SerializeField] List<Wave> waves;

    int currentWaveIndex = 0;

    private void Start()
    {
        if (waves.Count > 0)
        {
            SetCurrentWave(0);
            StartSpawn();
        }
    }

    void StartSpawn()
    {
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(spawnFrequancy);
            SelectEnemyToSpawn();
        }
    }

    void SelectEnemyToSpawn()
    {
        WaveProbabilities currentWaveProbabilities = waves[currentWaveIndex].probabilities;
        float totalChance = currentWaveProbabilities.Manager_Chance + currentWaveProbabilities.Police_Chance + currentWaveProbabilities.Army_Chance;

        if (totalChance <= 0f)
        {

            return;
        }

        float sceltaCasuale = Random.Range(0f, totalChance);
        float cumulativeProbability = 0f;

        switch (true)
        {
            case bool _ when (sceltaCasuale <= (cumulativeProbability += currentWaveProbabilities.Manager_Chance)):
                currentSpawned = EnemyType.MANAGER;
                break;
            case bool _ when (sceltaCasuale <= (cumulativeProbability += currentWaveProbabilities.Police_Chance)):
                currentSpawned = EnemyType.POLICE;
                break;
            case bool _ when (sceltaCasuale <= (cumulativeProbability += currentWaveProbabilities.Army_Chance)):
                currentSpawned = EnemyType.ARMY;
                break;
            default:
                Debug.LogError("Errore nella selezione del nemico da spawnare.");
                return;
        }

        SpawnEnemy();

    }

    void SpawnEnemy()
    {
        List<IEnemy> selectedList = EnemyPooler.Instance.GetEnemy(currentSpawned);
        foreach (IEnemy enemy in selectedList)
        {
            GameObject tmp;
            if (!enemy.Transform.gameObject.activeInHierarchy)
            {
                tmp = enemy.Transform.gameObject;
                enemy.Transform.position = transform.position;

                tmp.SetActive(true);

                break;
            }
        }


    }

    void SetCurrentWave(int index)
    {
        if (index >= 0 && index < waves.Count)
        {
            currentWaveIndex = index;
        }
        else
        {
            Debug.LogError("Indice dell'onda non valido");
        }
    }

    [System.Serializable]
    public class Wave
    {
        [Tooltip("Nome dell'onda")]
        public string waveName;
        public WaveProbabilities probabilities;
    }

    [System.Serializable]
    public struct WaveProbabilities
    {
        [Range(0f, 1f)] public float Manager_Chance;
        [Range(0f, 1f)] public float Police_Chance;
        [Range(0f, 1f)] public float Army_Chance;
    }
}



