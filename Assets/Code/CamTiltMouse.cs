using UnityEngine;
using System.Collections;

public class CamTiltMouse : MonoBehaviour {
	
	public bool lookAtMouse = false;
	
	public float maxDegreesX;
	public float maxDegreesY;
	
	public bool isActive = false;
	
	private Quaternion initialRotation;
	
	// Use this for initialization
	void Start ()
	{
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isActive) {
			Vector3 mousePos = Input.mousePosition;
			if (lookAtMouse) {
				mousePos -= Camera.main.WorldToScreenPoint (transform.position);
				mousePos.x += Screen.width / 2f;
				mousePos.y += Screen.height / 2f;
			}
			Vector3 rotAngles = Vector3.zero;
			mousePos.x = Mathf.Clamp01 (mousePos.x / (float)Screen.width);
			mousePos.y = Mathf.Clamp01 (mousePos.y / (float)Screen.height);
			
			mousePos.x = mousePos.x * 2f - 1f;
			mousePos.y = mousePos.y * 2f - 1f;
			rotAngles.y = mousePos.x * maxDegreesY;
			rotAngles.x = mousePos.y * maxDegreesX;
			Vector3 tmpeuler = initialRotation.eulerAngles + rotAngles;
		
			Quaternion tmpq = transform.rotation;
			tmpq.eulerAngles = tmpeuler;
			transform.rotation = tmpq;
		}
	}
	
	public void TurnOn ()
	{
		isActive = true;
	}
	
	public void TurnOff ()
	{
		isActive = false;
		transform.rotation = initialRotation;
	}
}
