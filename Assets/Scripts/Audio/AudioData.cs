using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("NewAudioData"),menuName =("AudioData"))]
public class AudioData : ScriptableObject
{
    [SerializeField] AudioClip Music_nome;
    [SerializeField] AudioClip SFX_nome;
}
