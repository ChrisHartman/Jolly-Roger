using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesDisplay : MonoBehaviour {


	public void ChangeAmount(string resource, float amount)
    {
        GetComponent<Text>().text = resource +": " + amount; 
    }
}
