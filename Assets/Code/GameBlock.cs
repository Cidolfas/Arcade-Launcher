using UnityEngine;
using System.Collections;

public class GameBlock : MonoBehaviour {
	
	public GameInformation info;
	
	public bool currSelected = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Setup (GameInformation gi)
	{
		info = gi;
		if (info.gameName != null) {
			name = info.gameName;
		} else {
			name = "Unnamed Game";
			info.gameName = "Unnamed Game";
		}
		
		if (info.imagePath != null) {
			WWW www = new WWW (info.imagePath);
			loadImageUrl (www);

			renderer.material.mainTexture = www.texture;
			www.Dispose ();
			www = null;
		} else {
			renderer.material.mainTexture = MenuManager.Instance.GetNoImageTex ();
		}
	}
	
	IEnumerator loadImageUrl (WWW www)
	{
		yield return www;

		if (www.error != null) {
			Debug.LogError ("WWW Error: " + www.error);
		}
	}
	
	void OnMouseEnter ()
	{
		MenuManager.Instance.BlockMouseEnter (this);
	}
	
	void OnMouseExit ()
	{
		MenuManager.Instance.BlockMouseExit (this);
	}
	
	void OnMouseUpAsButton ()
	{
		MenuManager.Instance.BlockMouseClick (this);
	}
	
}
