using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
	Treant,
	Dragon,
	GiantBee,
	Golem,
	KingCobra,
	Magma,
	Bat,
	Spider,
	Chest,
	Wolf
};

public class UnitLibrary : ScriptableObject {

	public static UnitLibrary instance;

	//[SerializeField] private UnitData	TreantData, DragonData, GiantBeeData, 
	//									GolemData, KingCobraData, MagmaData, 
	//									BatData, SpiderData, ChestData, WolfData;

	[SerializeField] private List<UnitData> unitTypes;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		instance = Resources.Load<UnitLibrary>("UnitLibrary");
	}

	//private static Dictionary<EnemyType, UnitData> enemyTypes
	//{
	//	get
	//	{
	//		return new Dictionary<EnemyType, UnitData>()
	//			{
	//				{EnemyType.Treant, instance.TreantData},
	//				{EnemyType.Dragon, instance.DragonData},
	//				{EnemyType.GiantBee, instance.GiantBeeData},
	//				{EnemyType.Golem, instance.GolemData},
	//				{EnemyType.KingCobra, instance.KingCobraData},
	//				{EnemyType.Magma, instance.MagmaData},
	//				{EnemyType.Bat, instance.BatData},
	//				{EnemyType.Spider, instance.SpiderData},
	//				{EnemyType.Chest, instance.ChestData},
	//				{EnemyType.Wolf, instance.WolfData},
	//			};
	//	}
	//}

	public UnitData GetUnitData(string _name)
	{
		foreach (UnitData data in unitTypes)
		{
			if (data.name == _name)
			{
				return data;
			}
		}

		return null;
	}
}
