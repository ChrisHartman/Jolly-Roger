using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {


	public int Gold = 5;
    public int Fabric = 5;
    public int Metal = 5;
    public int Wood = 5;
	// Use this for initialization
	void Start () {
		GetComponent<Health>().OnDeath += GiveResources;
	}
	void GiveResources() {
		FindObjectOfType<ResourceController>().GetResources(Gold, Fabric, Metal, Wood);
	}
}
