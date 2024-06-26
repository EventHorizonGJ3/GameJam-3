using UnityEngine;

public class AudioFilter : MonoBehaviour
{
    AudioLowPassFilter lowPassFilter;

    private void Awake()
    {
        lowPassFilter = GetComponent<AudioLowPassFilter>();
    }
    private void Start()
    {
        lowPassFilter.cutoffFrequency = 22000;
    }

    private void OnEnable()
    {
        GameManager.OnPause += HandleFilter;
    }
    private void OnDisable()
    {
        GameManager.OnPause -= HandleFilter;
    }

    void HandleFilter()
    {
        if (GameManager.gameOnPause) lowPassFilter.cutoffFrequency = 1900;
        if (!GameManager.gameOnPause) lowPassFilter.cutoffFrequency = 22000;
    }

}
