using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : ScriptableObject
{
	public static MaterialManager instance;

	[SerializeField] private MaterialObject Base, Selected, Blocked, Line, SelectedLine;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize()
	{
		instance = Resources.Load<MaterialManager>("MaterialManager");
	}

	private static Dictionary<MaterialName, MaterialObject> materials
	{
		get
		{
			return new Dictionary<MaterialName, MaterialObject>()
				{
					{MaterialName.BaseMaterial, instance.Base},
					{MaterialName.BlockedMaterial, instance.Blocked},
					{MaterialName.SelectedMaterial, instance.Selected},
					{MaterialName.LineMaterial, instance.Line},
					{MaterialName.SelectedLineMaterial, instance.SelectedLine}
				};
		}
	}

	public Material GetMaterial(MaterialName name)
	{
		return materials[name].material;
	}
}
