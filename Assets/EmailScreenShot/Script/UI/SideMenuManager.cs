using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuManager : MonoBehaviour {
	private bool isEnable = false;
	public Transform hiddenMenu;

	public void EnableSideMenu() {
		isEnable = !isEnable;

		//Set Color
		Color disAbleColor = Color.white;
		Color enableColor = new Color(0.75f, 0.75f, 0.75f);
		GetComponent<Image>().color = (isEnable) ? enableColor : disAbleColor;

		//Open Modal
		if (hiddenMenu) {
			hiddenMenu.gameObject.SetActive(isEnable);
		}

	}
}
