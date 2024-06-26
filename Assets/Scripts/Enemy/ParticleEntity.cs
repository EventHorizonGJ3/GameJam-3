using UnityEngine;

public class ParticleEntity : MonoBehaviour
{
    ParticleSystem particles;
    [SerializeField] float duration = 2;
    float timer = 0;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particles.Play();
    }

    private void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        timer = 0;
        particles.Stop();
    }
}
