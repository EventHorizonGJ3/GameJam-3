using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyType currentSpawned;
    [SerializeField][Tooltip("Intervallo tra un nemico e l'altro")] float spawnFrequancy;
    [Header("Eventi di Spawn")]
    [SerializeField] List<Wave> waves;

    int currentWaveIndex = 0;

    private void OnEnable()
    {
        Score.OnScoreChanged += CheckScore;
    }
    private void OnDisable()
    {
        Score.OnScoreChanged -= CheckScore;
    }

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
            while (GameManager.gameOnPause) yield return null;

            float elapsedTime = 0f;
            while (elapsedTime < spawnFrequancy)
            {
                if (!GameManager.gameOnPause)
                {
                    elapsedTime += Time.deltaTime;
                }
                yield return null;
            }

            SelectEnemyToSpawn();
        }
    }



    void SelectEnemyToSpawn()
    {
        WaveProbabilities currentWaveProbabilities = waves[currentWaveIndex].probabilities;
        float totalChance = currentWaveProbabilities.Manager_Chance + currentWaveProbabilities.Police_Chance + currentWaveProbabilities.Army_Chance;

        if (totalChance <= 0f)
        {
            Debug.Log("chance impostata male");
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

            if (!enemy.Transform.gameObject.activeInHierarchy)
            {

                enemy.Transform.position = transform.localPosition;

                enemy.Transform.gameObject.SetActive(true);

                break;
            }
        }
        //Debug.Log("Nemico spawnato: " + currentSpawned);

    }

    void CheckScore(float score)
    {
        if (currentWaveIndex + 1 >= waves.Count) return;
        if (score >= waves[currentWaveIndex+1].scoreForThisWave)
        {
            SetCurrentWave(currentWaveIndex+1);
        }
        Debug.Log("Wave corrente "+currentWaveIndex);
    }

    void SetCurrentWave(int index)
    {
        if (index >= 0 && index < waves.Count)
        {
            currentWaveIndex = index;
            Debug.Log("Onda corrente impostata a: " + currentWaveIndex);
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
        public float scoreForThisWave;
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



