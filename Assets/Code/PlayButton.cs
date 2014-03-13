using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseEnter ()
	{
		MenuManager.Instance.PlayButtonEnter (this);
	}
	
	void OnMouseExit ()
	{
		MenuManager.Instance.PlayButtonExit (this);
	}
	
	void OnMouseUpAsButton ()
	{
		MenuManager.Instance.PlayButtonClick ();
	}
	
}
