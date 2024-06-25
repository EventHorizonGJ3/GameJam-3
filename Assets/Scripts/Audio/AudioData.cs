using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("NewAudioData"),menuName =("AudioData"))]
public class AudioData : ScriptableObject
{
    [Header("UI")]
    [SerializeField] public AudioClip SFX_OpenPauseMenu;
    [SerializeField] public AudioClip SFX_UI_Navigate;
    [SerializeField] public AudioClip SFX_UI_Select;
    [SerializeField] public AudioClip SFX_UIMAIN_ButtonClick;
    
    [Header("SFXs")]
    [SerializeField] public AudioClip SFX_HitBonk; //DONE
    [SerializeField] public AudioClip SFX_BrokenWood; //missin shelves
    [SerializeField] public AudioClip SFX_BrokenGlass; //DONE
    [SerializeField] public AudioClip SFX_HitCar; //DONE
    [SerializeField] public AudioClip SFX_Dash; //DONE
    [SerializeField] public AudioClip SFX_BrokenHydrant; //DONE
    [SerializeField] public AudioClip SFX_Money; //Reproduce when destroy Money register - MISSING PREFAB
    [SerializeField] public AudioClip SFX_BrokenStreetLight; //DONE
    [SerializeField] public AudioClip SFX_BrokenTree; //DISPOSED
    [SerializeField] public AudioClip SFX_WeaponPickUp; //DONE
    
    [Header("OSTs")]
    [SerializeField] public AudioClip OST_Main; //MainScene Soundtrack
    [SerializeField] public AudioClip OST_Menu; //MainMenu
    [SerializeField] public AudioClip OST_Intro; //Reproduce during Prologue scenes
    [SerializeField] public AudioClip OST_Berserk; //Reproduce when Berserk mode is active
    [SerializeField] public AudioClip OST_Stacy; //Reproduce when Stacy is summoned
    [SerializeField] public AudioClip OST_Boss; //Reproduce when Supreme Manager is summoned
}
