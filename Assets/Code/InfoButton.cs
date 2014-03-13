using UnityEngine;
using System.Collections;

public class InfoButton : MonoBehaviour {
	
	public bool returning;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseUpAsButton ()
	{
		MenuManager.Instance.InfoButtonClick (returning);
	}
	
}
