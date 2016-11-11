using UnityEngine;
using System.Collections;

public class ColorChange_undead: MonoBehaviour {

	public Texture ColorTexture;
	public Material ColorMaterial;
	public Material ColorMaterial2;
//	public Texture GuiSwitch;
	public Color ColorNormal;
	public Color ColorSelect;
	public GUITexture Element1;
	public GUITexture Element2;
	public GUITexture Element3;

	// Use this for initialization
	void OnMouseUp () {

		Element1.color = ColorNormal;
		Element2.color = ColorNormal;
		Element3.color = ColorNormal;
		ColorMaterial.mainTexture = ColorTexture;
		ColorMaterial2.mainTexture = ColorTexture;
		this.GetComponent<GUITexture>().color = ColorSelect;
	//	this.guiTexture.texture = GuiSwitch;

				}
		
	}
	
	// Update is called once per frame
	//void Update () {
	
	//	}
	
	//DLNK ASSETS

