using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour {
	[SerializeField]
	EmailAndScreenshotService emailScreenshotService;

	void Start() {
		emailScreenshotService.Show(true);
	}
}
