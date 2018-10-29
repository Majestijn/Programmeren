using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HelperClass : MonoBehaviour {

	public static bool IsPointerOverUIElement()
	{
		PointerEventData eventData = new PointerEventData(EventSystem.current);

		eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> resultList = new List<RaycastResult>();

		EventSystem.current.RaycastAll(eventData, resultList);
		return resultList.Count > 0;
	}
}
