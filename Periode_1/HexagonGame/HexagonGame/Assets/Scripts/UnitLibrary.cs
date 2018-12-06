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

public enum AllyType
{
	WarriorCommon,
	ArcherCommon,
	MageCommon,
	Settler
}

public class UnitLibrary : ScriptableObject {

	public static UnitLibrary instance;

	[SerializeField] private UnitData	TreantData, DragonData, GiantBeeData,
										GolemData, KingCobraData, MagmaData,
										BatData, SpiderData, ChestData, WolfData;

	[SerializeField] private UnitData WarriorData, ArcherData, MageData, SettlerData;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		instance = Resources.Load<UnitLibrary>("UnitLibrary");
	}

	private static Dictionary<EnemyType, GameObject> enemyDict
	{
		get
		{
			return new Dictionary<EnemyType, GameObject>()
				{
					{EnemyType.Treant, instance.TreantData.prefab},
					{EnemyType.Dragon, instance.DragonData.prefab},
					{EnemyType.GiantBee, instance.GiantBeeData.prefab},
					{EnemyType.Golem, instance.GolemData.prefab},
					{EnemyType.KingCobra, instance.KingCobraData.prefab},
					{EnemyType.Magma, instance.MagmaData.prefab},
					{EnemyType.Bat, instance.BatData.prefab},
					{EnemyType.Spider, instance.SpiderData.prefab},
					{EnemyType.Chest, instance.ChestData.prefab},
					{EnemyType.Wolf, instance.WolfData.prefab},
				};
		}
	}

	private static Dictionary<AllyType, GameObject> allyDict
	{
		get
		{
			return new Dictionary<AllyType, GameObject>()
				{
					{AllyType.WarriorCommon, instance.WarriorData.prefab},
					{AllyType.ArcherCommon, instance.ArcherData.prefab},
					{AllyType.MageCommon, instance.MageData.prefab},
					{AllyType.Settler, instance.SettlerData.prefab}
				};
		}
	}

	public GameObject GetAllyData(AllyType type)
	{
		foreach (KeyValuePair<AllyType, GameObject> keyValuePair in allyDict)
		{
			if (keyValuePair.Key == type)
			{
				return keyValuePair.Value;
			}
		}

		return null;
	}

	public GameObject GetEnemyData(EnemyType type)
	{
		foreach (KeyValuePair<EnemyType, GameObject> keyValuePair in enemyDict)
		{
			if (keyValuePair.Key == type)
			{
				return keyValuePair.Value;
			}
		}

		return null;
	}
}
