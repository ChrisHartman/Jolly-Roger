using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamUpgrade : UpgradeController {

	override public string UpgradeName {get {return "Get Ram";} set{}}
	override public int NumberOfUpgrades {get {return 1;} set{}}
	override public int GoldCost {get {return 15;} set{}}
	override public int FabricCost {get {return 0;} set{}}
	override public int MetalCost {get {return 2;} set{}}
	override public int WoodCost {get {return 10;} set{}}

	// Use this for initialization


	void Start () {
		
	}
	override public void ApplyUpgrade() {
		RamController ram = FindObjectOfType<RamController>();
		Debug.Log("Ram time!");
		ram.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
