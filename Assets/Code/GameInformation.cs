using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Xml;

[System.Serializable]
public class GameInformation {
	
	public string gameName;
	public string description;
	public string longDescription;
	public string programLocation;
	public string arguments;
	public string[] authors;
	public int requiredPlayers = 0;
	public string releaseDate;
	public string website;
	public string imagePath;
	
	private bool shouldFullscreen;
	
	public GameInformation (XmlNode node)
	{
		if (node ["Title"] != null)
			gameName = node ["Title"].InnerText;
		if (node ["Description"] != null)
			description = node ["Description"].InnerText;
		if (node ["LongDescription"] != null)
			longDescription = node ["LongDescription"].InnerText;
		if (node ["FileLocation"] != null)
			programLocation = node ["FileLocation"].InnerText;
		if (node ["Arguments"] != null)
			arguments = node ["Arguments"].InnerText;
		if (node ["Website"] != null)
			website = node ["Website"].InnerText;
		if (node ["CreationDate"] != null)
			releaseDate = node ["CreationDate"].InnerText;
		if (node ["NumberOfPlayers"] != null)
			requiredPlayers = int.Parse (node ["NumberOfPlayers"].InnerText);
		
		if (node ["Authors"] != null) {
			XmlNodeList aths = node ["Authors"].GetElementsByTagName ("Author");
			authors = new string[aths.Count];
			for (int i = 0; i < aths.Count; i++) {
				authors [i] = aths [i].InnerText;
			}
		}
		
		if (node ["ImageLocation"] != null) {
			imagePath = "file://" + Application.dataPath + node ["ImageLocation"].InnerText;
		}
	}
	
	public void Launch ()
	{
		shouldFullscreen = Screen.fullScreen;
		
		if (Screen.fullScreen) {
			Screen.fullScreen = false;
		}
		
		MenuManager.Instance.music.Pause ();
		
		Process proc = new Process ();
		ProcessStartInfo pinfo = new ProcessStartInfo ();
		pinfo.FileName = Application.dataPath + programLocation;
		pinfo.Arguments = arguments;
		pinfo.UseShellExecute = false;
		proc.StartInfo = pinfo;
		try {
			proc.Start ();
		} catch {
			UnityEngine.Debug.LogError ("Unable to open " + pinfo.FileName);
			MenuManager.Instance.music.Play ();
		
			if (shouldFullscreen) {
				Screen.fullScreen = true;
			}
			return;
		}
		
		proc.WaitForExit ();
		
		MenuManager.Instance.music.Play ();
		
		if (shouldFullscreen) {
			Screen.fullScreen = true;
		}
	}
	
}
