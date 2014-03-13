using UnityEngine;
using System.Collections;

public class ArrowButton : MonoBehaviour {
	
	public bool pointsRight;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseEnter ()
	{
		MenuManager.Instance.ArrowButtonEnter (this);
	}
	
	void OnMouseExit ()
	{
		MenuManager.Instance.ArrowButtonExit (this);
	}
	
	void OnMouseDrag ()
	{
		MenuManager.Instance.ArrowButtonClick (pointsRight);
	}
	
}
