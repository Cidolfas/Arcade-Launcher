using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	
	public GameObject title;
	public GameObject description;
	public GameObject authors;
	
	public int titleWidth = 30;
	public int descriptionWidth = 25;
	public int authorsWidth = 25;
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	public void Process (GameInformation info)
	{
		title.GetComponent<TextMesh> ().text = WordWrap (info.gameName, titleWidth);
		description.GetComponent<TextMesh> ().text = WordWrap (info.description, descriptionWidth);
		authors.GetComponent<TextMesh> ().text = WordWrap (System.String.Join ("\n", info.authors), authorsWidth);
	}
	
	private string WordWrap (string unwrap, int lineWidth)
	{
		string ret = "";
		string[] strLines = unwrap.Split (new char[] {'\n'});
		for (int j = 0; j < strLines.Length; j++) {
			string[] strBits = strLines [j].Split (new char[] {' '});
		
			int chars = lineWidth;
			for (int i = 0; i < strBits.Length; i++) {
				if (strBits [i].Length >= lineWidth) {
					Debug.Log ("foo: " + i);
					if (i == 0) {
						ret += strBits [0];
					} else {
						ret += "\n" + strBits [i];
					}
					chars = -1;
				} else {
					chars -= strBits [i].Length;
					if (chars < 0) {
						ret += "\n";
						chars = lineWidth - strBits [i].Length;
					}
					ret += strBits [i] + " ";
					chars -= 1;
				}
			}
			if (j != strLines.Length - 1)
				ret += "\n";
		}
		return ret;
	}
	
}
