using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeController : MonoBehaviour {

	// Use this for initialization
	abstract public string UpgradeName {get; set;}
	abstract public int NumberOfUpgrades {get; set;} 
	abstract public int GoldCost {get; set;} 
	abstract public int FabricCost {get; set;} 
	abstract public int MetalCost {get; set;} 
	abstract public int WoodCost {get; set;} 

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract void ApplyUpgrade();
	public void BuyUpgrade() {
		for (int i = 1; i <= NumberOfUpgrades; i++) {
			if (PlayerPrefs.GetInt(UpgradeName + i) == 0) {
				PlayerPrefs.SetInt(UpgradeName + i, 1);
				break;
			}
			
		}
		PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - GoldCost);
		PlayerPrefs.SetInt("Metal", PlayerPrefs.GetInt("Metal") - MetalCost);
		PlayerPrefs.SetInt("Fabric", PlayerPrefs.GetInt("Fabric") - FabricCost);
		PlayerPrefs.SetInt("Wood", PlayerPrefs.GetInt("Wood") - WoodCost);
	}

}
