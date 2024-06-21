using UnityEngine;
using Random = UnityEngine.Random;

public class CarActivator : MonoBehaviour
{
    [SerializeField, Range(0, 1f)] private float ChanceToSpawn;
    private float rand;
    [SerializeField] private GameObject[] Cars;

    private void Awake()
    {
        foreach (GameObject Car in Cars)
        {
            rand = Random.Range(0, 1f);
            if (rand < ChanceToSpawn)
            {
                Car.SetActive(true);
            }
        }
    }

}
