using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GunBase> weapons = new List<GunBase>(); // Store all weapons
    private int currentWeaponIndex = 0;

    void Start()
    {
        SelectWeapon(currentWeaponIndex);
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0f) // Scroll up
        {
            NextWeapon();
        }
        else if (scrollInput < 0f) // Scroll down
        {
            PreviousWeapon();
        }

        // Allow switching with number keys (1, 2, 3...)
        for (int i = 0; i < weapons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectWeapon(i);
            }
        }
    }

    private void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        SelectWeapon(currentWeaponIndex);
    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Count - 1;
        }
        SelectWeapon(currentWeaponIndex);
    }

    private void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }
        Debug.Log("Switched to: " + weapons[index].gameObject.name);
    }
}
