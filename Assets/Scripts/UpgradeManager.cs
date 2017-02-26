using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
	private UpgradeController[] upgrades;
	// Use this for initialization
	void Awake() {
		upgrades = GetComponentsInChildren<UpgradeController>();
	}
	public void ApplyUpgrades() {
		foreach (UpgradeController upgrade in upgrades) {
			for (int i = 1; i <= upgrade.NumberOfUpgrades; i++) {
				if (PlayerPrefs.GetInt(upgrade.UpgradeName + i) == 1) {
					upgrade.ApplyUpgrade();
				} else {
					break;
				}
			}
		}
	}
		
	public List<UpgradeController> getAllAvailableUpgrades() {
		List<UpgradeController> availableUpgrades = new List<UpgradeController>();
		Debug.Log(upgrades.Length);
		foreach (UpgradeController upgrade in upgrades) {
			for (int i = 1; i <= upgrade.NumberOfUpgrades; i++) {
				if (PlayerPrefs.GetInt(upgrade.UpgradeName + i) == 0) {
					availableUpgrades.Add(upgrade);
				}
			}
		}
		return availableUpgrades;
	}
}
