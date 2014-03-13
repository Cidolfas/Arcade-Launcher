using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {
	
	public bool forReals;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseUpAsButton ()
	{
		MenuManager.Instance.ConfirmQuit (forReals);
	}
	
}
