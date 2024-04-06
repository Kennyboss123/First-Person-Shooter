using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource pistolShootingClip;
    public AudioSource pistolReloadingClip;
    public AudioSource pistolEmptyGunClip;

    public AudioSource m16ShootingClip;
    public AudioSource m16ReloadingClip;
    public AudioSource m16EmptyGunClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                pistolShootingClip.Play();
                break;

            case WeaponModel.M16:
                m16ShootingClip.Play();
                break;
        }
    }
    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                pistolReloadingClip.Play();
                break;

            case WeaponModel.M16:
                m16ReloadingClip.Play();
                break;
        }
    }
    public void PlayEmptySound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                m16EmptyGunClip.Play();
                break;

            case WeaponModel.M16:
                m16EmptyGunClip.Play();
                break;
        }
    }
}
