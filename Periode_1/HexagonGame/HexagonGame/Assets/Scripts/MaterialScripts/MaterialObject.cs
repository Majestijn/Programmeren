using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialName
{
	BaseMaterial,
	SelectedMaterial,
	BlockedMaterial,
	LineMaterial,
	SelectedLineMaterial
}

[System.Serializable]
[CreateAssetMenu(fileName ="Material", menuName ="Custom/MaterialObject", order =0)]
public class MaterialObject : ScriptableObject {

	[SerializeField] private MaterialName m_MaterialName;
	[SerializeField] private Material m_Material;

	public MaterialName materialName
	{
		get { return m_MaterialName;}
	}

	public Material material
	{
		get { return m_Material;}
	}
}
