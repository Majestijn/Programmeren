using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour {

	public delegate void EndTurnAction();

	public event EndTurnAction OnEndTurn;

	public static GameManager instance;

	private void Start()
	{
		instance = this;
	}

	public void EndTurn()
	{
		OnEndTurn?.Invoke();
		SelectionManager.instance.SelectUnit();
	}
}
