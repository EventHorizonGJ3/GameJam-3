using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("NewAudioData"),menuName =("AudioData"))]
public class AudioData : ScriptableObject
{
    [SerializeField] public AudioClip Music_nome;
    [SerializeField] public AudioClip SFX_OpenPauseMenu;
    [SerializeField] public AudioClip SFX_UI_Navigate;
    [SerializeField] public AudioClip SFX_UI_Select;
    [SerializeField] public AudioClip SFX_UIMAIN_ButtonClick;
}
