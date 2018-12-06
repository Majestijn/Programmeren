using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceUtility : MonoBehaviour {

	public static T GetInterfaces<T>(GameObject objectToSearch) where T : class
	{
		return objectToSearch.GetComponentInChildren<T>();
	}
}

public interface ISelectable
{
	void Select();
	void Deselect();
}

public interface IMovable
{
	void MoveToHex(Hex targetHex);
	void MoveToEnemy(Hex targetHex);
}

public interface IAttackable
{
	void SetTarget(Health enemyHealth);
	void Attack();
}
