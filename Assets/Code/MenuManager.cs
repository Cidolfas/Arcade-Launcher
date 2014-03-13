using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class MenuManager : MonoBehaviour {
	
	public static MenuManager Instance;
	
	public UIManager manager;
	
	public Texture2D[] noImageTexs;
	private System.Collections.Generic.List<Texture2D> unusedNoImageTexs = new System.Collections.Generic.List<Texture2D>();
	
	public int currentSelection = 0;
	
	public GameBlock[] blockslist;
	
	private float nextSelectionTime = 0f;
	private Vector3 cameraMoveTarget;
	private Quaternion cameraRotateTarget;
	
	public float keyDelay = 0.2f;
	public int rowWidth = 4;
	public float separation = 1.5f;
	
	private bool canNavUpDown = false;
	
	public AudioSource music;
	
	// Use this for initialization
	void Awake ()
	{
		Instance = this;
		
		foreach (Texture2D tex in noImageTexs) {
			unusedNoImageTexs.Add (tex);
		}
		
		XmlDocument xmld = new XmlDocument ();
		Debug.Log (Application.dataPath + "/GameData/GameLauncher.xml");
		xmld.Load (Application.dataPath + "/GameData/GameLauncher.xml");
		
		XmlNodeList infos = xmld.GetElementsByTagName ("GameInfo");
		
		blockslist = new GameBlock[infos.Count];
		
		Quaternion tmpQ = Quaternion.identity;
		tmpQ.eulerAngles = new Vector3 (0f, 180f, 0f);
		
		for (int i = 0; i < infos.Count; i++) {
			GameObject foo = Instantiate (Resources.Load ("Prefabs/GameBlock"), Vector3.zero, tmpQ) as GameObject;
			blockslist [i] = foo.GetComponent<GameBlock> ();
			blockslist [i].Setup (new GameInformation (infos [i]));
		}
		
		BuildMenu ();
		
		SelectBlock (blockslist [currentSelection]);
	}
	
	void Start ()
	{
		cameraMoveTarget = transform.position;
		cameraRotateTarget = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.LeftArrow) && 
			currentSelection > 0 && 
			(!canNavUpDown || currentSelection % rowWidth != 0) && 
			Time.time > nextSelectionTime) {
			
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection--;
			SelectBlock (blockslist [currentSelection]);
			
		} else if (Input.GetKey (KeyCode.RightArrow) && 
			currentSelection < blockslist.Length - 1 && 
			(!canNavUpDown || currentSelection % rowWidth != rowWidth - 1) && 
			Time.time > nextSelectionTime) {
			
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection++;
			SelectBlock (blockslist [currentSelection]);
			
		} else if (canNavUpDown && 
			Input.GetKey (KeyCode.UpArrow) && 
			currentSelection >= rowWidth && 
			Time.time > nextSelectionTime) {
			
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection -= rowWidth;
			SelectBlock (blockslist [currentSelection]);
			
		} else if (canNavUpDown && 
			Input.GetKey (KeyCode.DownArrow) && 
			currentSelection < blockslist.Length - rowWidth && 
			Time.time > nextSelectionTime) {
			
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection += rowWidth;
			SelectBlock (blockslist [currentSelection]);
		} else if (Input.GetKeyDown (KeyCode.Return)) {
			PlayButtonClick ();
		} else if (Input.GetKeyDown (KeyCode.Escape)) {
			ConfirmQuit (false);
		}
		
		if (transform.position != cameraMoveTarget) {
			float dist = (Vector3.Distance (transform.position, cameraMoveTarget) * 10f + 3f) * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, cameraMoveTarget, dist);
		}
		
		if (Camera.main.transform.rotation != cameraRotateTarget) {
			Camera.main.transform.rotation = Quaternion.RotateTowards (Camera.main.transform.rotation, cameraRotateTarget, 180f * Time.deltaTime);
		}
	}
	
	public Texture2D GetNoImageTex ()
	{
		if (noImageTexs.Length == 0) {
			Debug.LogError ("No default textures set!");
			return null;
		}
		
		if (unusedNoImageTexs.Count == 0) {
			foreach (Texture2D tex in noImageTexs) {
				unusedNoImageTexs.Add (tex);
			}
		}
		
		Texture2D foo = unusedNoImageTexs [Random.Range (0, unusedNoImageTexs.Count - 1)];
		unusedNoImageTexs.Remove (foo);
		
		return foo;
	}
	
	private void BuildMenu ()
	{
		for (int i = 0; i < blockslist.Length; i++) {
			blockslist [i].transform.position = new Vector3 (i * separation, 0f, 0f);
		}
		
		for (int i = 1; i < 4; i++) {
			Instantiate (Resources.Load ("Prefabs/EmptyCube"), new Vector3 (i * -separation, 0f, 0f), Quaternion.identity);
		}
		
		for (int i = 0; i < 3; i++) {
			Instantiate (Resources.Load ("Prefabs/EmptyCube"), new Vector3 ((i + blockslist.Length) * separation, 0f, 0f), Quaternion.identity);
		}
	}
	
	public void SelectBlock (GameBlock block)
	{
		block.currSelected = true;
		
		iTween.MoveTo (block.gameObject, iTween.Hash ("z", -1f, "y", 1.2f, "time", 0.5f, "easetype", iTween.EaseType.easeOutCubic));
		iTween.ScaleTo (block.gameObject, iTween.Hash ("x", 1.5f, "y", 1.5f, "z", 1.5f, "time", 0.5f, "easetype", iTween.EaseType.linear));
		block.GetComponent<CamTiltMouse> ().TurnOn ();
		
		cameraMoveTarget.x = blockslist [currentSelection].transform.position.x;
		
		manager.Process (block.info);
	}
	
	public void DeselectBlock (GameBlock block)
	{
		block.currSelected = false;
		
		iTween.MoveTo (block.gameObject, iTween.Hash ("z", 0f, "y", 0f, "time", 0.5f, "easetype", iTween.EaseType.easeOutCubic));
		iTween.ScaleTo (block.gameObject, iTween.Hash ("x", 1f, "y", 1f, "z", 1f, "time", 0.5f, "easetype", iTween.EaseType.linear));
		block.GetComponent<CamTiltMouse> ().TurnOff ();
	}
	
	public void BlockMouseEnter (GameBlock block)
	{
		if (!block.currSelected)
			iTween.ScaleTo (block.gameObject, iTween.Hash ("x", 1.3f, "y", 1.3f, "z", 1.1f, "time", 0.2f, "easetype", iTween.EaseType.linear));
	}
	
	public void BlockMouseExit (GameBlock block)
	{
		if (!block.currSelected)
			iTween.ScaleTo (block.gameObject, iTween.Hash ("x", 1f, "y", 1f, "z", 1f, "time", 0.2f, "easetype", iTween.EaseType.linear));
	}
	
	public void BlockMouseClick (GameBlock block)
	{
		if (!block.currSelected) {
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection = System.Array.IndexOf (blockslist, block);
			SelectBlock (blockslist [currentSelection]);
		}
	}
	
	public void PlayButtonEnter (PlayButton button)
	{
		
	}
	
	public void PlayButtonExit (PlayButton button)
	{
		
	}
	
	public void PlayButtonClick ()
	{
		blockslist [currentSelection].info.Launch ();
	}
	
	public void ArrowButtonEnter (ArrowButton button)
	{
		
	}
	
	public void ArrowButtonExit (ArrowButton button)
	{
		
	}
	
	public void ArrowButtonClick (bool goingRight)
	{
		if (!goingRight && 
			currentSelection > 0 && 
			(!canNavUpDown || currentSelection % rowWidth != 0) && 
			Time.time > nextSelectionTime) {
			
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection--;
			SelectBlock (blockslist [currentSelection]);
			
		} else if (goingRight && 
			currentSelection < blockslist.Length - 1 && 
			(!canNavUpDown || currentSelection % rowWidth != rowWidth - 1) && 
			Time.time > nextSelectionTime) {
			
			nextSelectionTime = Time.time + keyDelay;
			DeselectBlock (blockslist [currentSelection]);
			currentSelection++;
			SelectBlock (blockslist [currentSelection]);
			
		}
	}
	
	public void ConfirmQuit (bool forReals)
	{
		if (forReals)
			Application.Quit ();
		
		cameraRotateTarget = new Quaternion (0f, -1f, 0f, 0f);
	}
	
	public void InfoButtonClick (bool returning)
	{
		if (!returning) {
			cameraRotateTarget = new Quaternion (-0.7f, 0f, 0f, 0.7f);
		} else {
			cameraRotateTarget = Quaternion.identity;
		}
	}
	
}
