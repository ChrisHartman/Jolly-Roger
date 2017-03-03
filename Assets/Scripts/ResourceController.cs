using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
	private int Gold = 0;
    private int Fabric = 0;
    private int Metal = 0;
    private int Wood = 0;
	// Use this for initialization
	void Start () {
		FindObjectOfType<GameManager>().OnWin += SaveResources;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GetResources(int g, int f, int m, int w) {
		Gold += g;
		GameObject.Find("Gold").GetComponent<ResourcesDisplay>().ChangeAmount(Gold);
		Fabric += f;
		GameObject.Find("Fabric").GetComponent<ResourcesDisplay>().ChangeAmount(Fabric);
		Metal += m;
		GameObject.Find("Metal").GetComponent<ResourcesDisplay>().ChangeAmount(Metal);
		Wood += w; 
		GameObject.Find("Wood").GetComponent<ResourcesDisplay>().ChangeAmount(Wood);
	}

	public void SaveResources() {
		PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + Gold);
		PlayerPrefs.SetInt("Metal", PlayerPrefs.GetInt("Metal") + Metal);
		PlayerPrefs.SetInt("Fabric", PlayerPrefs.GetInt("Fabric") + Fabric);
		PlayerPrefs.SetInt("Wood", PlayerPrefs.GetInt("Wood") + Wood);

	}
}
