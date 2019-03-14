using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void ServiceCloseEvent() ;

public class EmailAndScreenshotService : MonoBehaviour {

	[SerializeField]
	private EmailManager emailManager;

	[SerializeField]
	private CameraScreenCapture cameraCapture;

	public event ServiceCloseEvent OnServiceClose = delegate{};

	void Start () {
		if (emailManager == null)
			 emailManager = transform.Find("Service/Email").GetComponent<EmailManager>();

		if (cameraCapture == null)
			 cameraCapture = transform.Find("Service/ScreenShot").GetComponent<CameraScreenCapture>();

		if (emailManager != null && cameraCapture != null) {
			cameraCapture.SetUp(Camera.main);
			emailManager.SetUp(cameraCapture);
		}
	}

	public void Show(bool open) {
		//Email panel is always close by default
		emailManager.OpenEmailModal(false);
		cameraCapture.Show(open);

		if (!open) {
			if (OnServiceClose != null)
				OnServiceClose();
		}
	}

	void OnValidate()
	{
		Start();		
	}
}
