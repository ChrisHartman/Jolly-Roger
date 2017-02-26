using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeController {


	override public string UpgradeName {get {return "Upgrade Health";} set{}}
	override public int NumberOfUpgrades {get {return 3;} set{}}
	override public int GoldCost {get {return 10;} set{}}
	override public int FabricCost {get {return 5;} set{}}
	override public int MetalCost {get {return 0;} set{}}
	override public int WoodCost {get {return 5;} set{}}

	// Use this for initialization


	void Start () {
		
	}
	override public void ApplyUpgrade() {
		ShipController ship = FindObjectOfType<ShipController>();
		ship.GetComponent<Health>().InitialHealth += 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
