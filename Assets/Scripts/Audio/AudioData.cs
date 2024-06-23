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
    [SerializeField] public AudioClip SFX_HitBonk; //Reproduce when hit enemy or hitten by enemy
    [SerializeField] public AudioClip SFX_BrokenWood; //Reproduce when destroy shelves and Mannequines
    [SerializeField] public AudioClip SFX_BrokenGlass; //Reproduce when destroy Mirror and GlassObjects
    [SerializeField] public AudioClip SFX_HitCar; //Reproduce when hit Cars
    [SerializeField] public AudioClip SFX_Dash; //WhenPlayerDash
    [SerializeField] public AudioClip SFX_BrokenHydrant; //Reproduce when destroy Hydrant
    [SerializeField] public AudioClip SFX_Money; //Reproduce when destroy Money register
    [SerializeField] public AudioClip SFX_BrokenStreetLight; //Reproduce when destroy Street Lamps
    [SerializeField] public AudioClip SFX_BrokenTree; //Reproduce when destroy Tree
    [SerializeField] public AudioClip WeaponPickUp; //Reproduce when pick up weapon
    
    [Header("OSTs")]
    [SerializeField] public AudioClip OST_Main; //MainScene Soundtrack
    [SerializeField] public AudioClip OST_Menu; //MainMenu
    [SerializeField] public AudioClip OST_Intro; //Reproduce during Prologue scenes
    [SerializeField] public AudioClip OST_Berserk; //Reproduce when Berserk mode is active
    [SerializeField] public AudioClip OST_Stacy; //Reproduce when Stacy is summoned
    [SerializeField] public AudioClip OST_Boss; //Reproduce when Supreme Manager is summoned
}
