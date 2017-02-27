using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour {

	// Use this for initialization
	Hashtable ButtonsToUpgrades = new Hashtable();
	private Text resourceDisplay;
	void Start () {
		//PlayerPrefs.DeleteAll();
		//PlayerPrefs.SetInt("Gold", 30);
		//PlayerPrefs.SetInt("Metal", 30);
		//PlayerPrefs.SetInt("Fabric", 30);
		//PlayerPrefs.SetInt("Wood", 30);
		resourceDisplay = GameObject.Find("Resource Display").GetComponent<Text>();
		UpdateDisplay();
		List<UpgradeController> upgrades = FindObjectOfType<UpgradeManager>().getAllAvailableUpgrades();
		Debug.Log(upgrades.Count);
		int upgradeCount = 0;
		foreach (Button button in GetComponentsInChildren<Button>()) {
			if (upgradeCount >= upgrades.Count ||
				!SufficientFunds(upgrades[upgradeCount])) {
					Destroy(button.gameObject);
			} else {
				button.GetComponentInChildren<Text>().text = upgrades[upgradeCount].UpgradeName 
															 + "\nCosts " + upgrades[upgradeCount].GoldCost + " Gold, "
															 + upgrades[upgradeCount].FabricCost + " Fabric, "
															 + upgrades[upgradeCount].MetalCost + " Metal, and "
															 + upgrades[upgradeCount].WoodCost + " Wood";
				button.onClick.AddListener(upgrades[upgradeCount].BuyUpgrade);
				button.onClick.AddListener(CheckFunds);
				button.onClick.AddListener(UpdateDisplay);
				ButtonsToUpgrades.Add(button, upgrades[upgradeCount]);
				upgradeCount++;
			}
		}
	}

	public bool SufficientFunds(UpgradeController u) {
		if (u.GoldCost > PlayerPrefs.GetInt("Gold") ||
		    u.FabricCost > PlayerPrefs.GetInt("Fabric") || 
		    u.MetalCost > PlayerPrefs.GetInt("Metal") ||
		    u.WoodCost > PlayerPrefs.GetInt("Wood")) {
				return false;
		}
		return true;
	}

	private void UpdateDisplay() {
		resourceDisplay.text = "Gold: " + PlayerPrefs.GetInt("Gold") 
							   + "\nFabric: " + PlayerPrefs.GetInt("Fabric")
							   + "\nMetal: " + PlayerPrefs.GetInt("Metal")
							   + "\nWood: " + PlayerPrefs.GetInt("Wood");
	}

	public void Kill(Button button) {
		button.interactable = false;
	}

	public void CheckFunds() {
		List<Button> UnafordableButtons = new List<Button>();
		Debug.Log(PlayerPrefs.GetInt("Gold"));
		foreach (Button button in ButtonsToUpgrades.Keys) {
			if (!SufficientFunds((UpgradeController)ButtonsToUpgrades[button])) {
				UnafordableButtons.Add(button);
				Kill(button);
			}
		}
		foreach (Button button in UnafordableButtons) {
			ButtonsToUpgrades.Remove(button);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
