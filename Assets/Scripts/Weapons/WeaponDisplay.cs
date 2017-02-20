using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour {
	public void ChangeActiveWeapon(string newWeapon)
    {
        GetComponent<Text>().text = "Active Weapon: " + newWeapon; 
    }
}
