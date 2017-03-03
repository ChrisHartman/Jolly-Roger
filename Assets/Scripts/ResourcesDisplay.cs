using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesDisplay : MonoBehaviour {

    public string resourceName;

	public void ChangeAmount(float amount)
    {
        GetComponent<Text>().text = resourceName +": " + amount; 
    }
}
