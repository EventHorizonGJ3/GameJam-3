using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OST_Changer : MonoBehaviour
{
    AudioClip originalOST;
    AudioClip bossMusic;
    AudioClip stacyMusic;
    AudioSource audioSource;

    int bossIndex;

    private void Awake()
    {
        originalOST = GetComponent<AudioClip>();
        audioSource = GetComponent<AudioSource>();
        //bossMusic = AudioManager.instance.AudioData.OST_Boss;
        //stacyMusic = AudioManager.instance.AudioData.OST_Stacy;
    }

    private void Start()
    {
        bossMusic = AudioManager.instance.AudioData.OST_Boss;
        stacyMusic = AudioManager.instance.AudioData.OST_Stacy;
        BossHp.OnBossDead += ChangeOst;
    }

    private void OnEnable()
    {
        SupremeSpawner.OnFinalBoss += ChangeOSTToBoss;
        StacySpawner.OnStacy += ChangeOSTToStacy;
    }
    private void OnDisable()
    {
        SupremeSpawner.OnFinalBoss -= ChangeOSTToBoss;
        StacySpawner.OnStacy -= ChangeOSTToStacy;
    }

    void ChangeOSTToBoss()
    {
        audioSource.clip = bossMusic;
        audioSource.Stop();
        audioSource.Play();
    }

    void ChangeOSTToStacy()
    {
        audioSource.Stop();
        audioSource.clip = stacyMusic;
        audioSource.Play();
    }

    void ChangeOst()
    {
        audioSource.Stop();
        audioSource.clip = originalOST;
        audioSource.Play();

    }
}
