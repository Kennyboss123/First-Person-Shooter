using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI ammoSize;
    public TextMeshProUGUI magazineSize;
    public Image ammoTypeUI;
        
    [Header("Weapons")]
    public Image activeWeaponUI;
    public Image inActiveWeaponUI;

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
    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon inactiveWeapon = GetInActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            ammoSize.text = $"{activeWeapon.bulletsLeft}";
            magazineSize.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);
        }
        if (inactiveWeapon)
        {
            inActiveWeaponUI.sprite = GetWeaponSprite(inactiveWeapon.thisWeaponModel);
        }
    }
    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch(model)
        {
            case Weapon.WeaponModel.Pistol:
                return Instantiate(Resources.Load<GameObject>("PistolWeapon")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M16:
                return Instantiate(Resources.Load<GameObject>("M16Weapon")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }
    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Instantiate(Resources.Load<GameObject>("PistolAmmo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponModel.M16:
                return Instantiate(Resources.Load<GameObject>("M16Ammo")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }
    private GameObject GetInActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }
}
