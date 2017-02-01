using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform Target;
	public float Smoothing = 5f;
	public float TargetDistance = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
			 Gizmos.color = Color.blue;
			 Vector3 topleft, topright, bottomleft, bottomright;
			 topleft = new Vector3(transform.position.x + TargetDistance, transform.position.y - TargetDistance, 0);
			 topright = new Vector3(transform.position.x + TargetDistance, transform.position.y + TargetDistance, 0);
			 bottomleft = new Vector3(transform.position.x - TargetDistance, transform.position.y - TargetDistance, 0);
			 bottomright = new Vector3(transform.position.x - TargetDistance, transform.position.y + TargetDistance, 0);
			 
       Gizmos.DrawLine(topleft, topright);
			 Gizmos.DrawLine(topright, bottomright);
			 Gizmos.DrawLine(bottomright, bottomleft);
			 Gizmos.DrawLine(topleft, bottomleft);

	}

	void Update () {
		Vector2 cameraToTarget  = Target.transform.position - transform.position;
		Vector3 targetPosition = transform.position;
		if (cameraToTarget.x > TargetDistance) {
			targetPosition.x = targetPosition.x + TargetDistance;
			Debug.Log("Too right!");
		} else if (cameraToTarget.x < -TargetDistance) {
			targetPosition.x = targetPosition.x - TargetDistance;
			Debug.Log("Too left!");
		}
		if (cameraToTarget.y > TargetDistance) {
			targetPosition.y = targetPosition.y + TargetDistance;
			Debug.Log("Too high!");
		} else if (cameraToTarget.y < -TargetDistance) {
			targetPosition.y = targetPosition.y - TargetDistance;
			Debug.Log("Too low!");
		}
		//transform.position = targetPosition;
		transform.position = Vector3.Lerp(transform.position, targetPosition, Smoothing*Time.deltaTime);
	}
}
