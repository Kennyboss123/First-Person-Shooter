using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public int ammoAmount = 200;
    public Ammotype ammoType;
    public enum Ammotype
    {
        M16Ammo,
        PistolAmmo
    }
}
