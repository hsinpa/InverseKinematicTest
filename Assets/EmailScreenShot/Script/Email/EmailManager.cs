using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailManager : MonoBehaviour {
	public Transform emailModal;

	EmailService _emailService;
	CameraScreenCapture _cameraCapture;
	string email_pref_key = "User.Email";

	public void SetUp(CameraScreenCapture p_cameraCapture) {
		string email = "publicsafety.tw@gmail.com";
		string password = "Safety2018";
		string company_name = "公共安全衛生局";

		_cameraCapture = p_cameraCapture;
		_emailService = new EmailService(email, password, company_name);
	}

	public void OpenEmailModal(bool open) {
		if (emailModal) {
			emailModal.gameObject.SetActive(open);
			if (open) {
				//Set email if already exist
				emailModal.GetComponentInChildren<InputField>().text = PlayerPrefs.GetString(email_pref_key, "");
			}
		}
	}

	public void SendEmail(InputField p_input) {
		SendEmail(p_input.text);
	}

	private void SendEmail(string p_target_email) {
		if (string.IsNullOrEmpty(p_target_email)) return;
		if (_emailService != null) {

			_emailService.Send("歡迎再臨AR牆展覽", "您在AR牆所拍攝的相片, 已含在附件裡.", 
								new string[] { p_target_email }, _cameraCapture._souvenirPics);
			PlayerPrefs.SetString(email_pref_key, p_target_email);
		}
	}

}
