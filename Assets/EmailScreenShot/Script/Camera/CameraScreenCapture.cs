using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;

public class CameraScreenCapture : MonoBehaviour {

	public RenderTexture tempRenderTexture;

	[Range(0, 100)]
	public int jpgQuality = 75;

	private Camera _camera;

	[SerializeField]
	public Transform cameraFooter;

	private string rootFolder;

	public string[] _souvenirPics {
		get {
			if(Directory.Exists(rootFolder))
			{
				return Directory.GetFiles(rootFolder);
			}
			return new string[0];
		}
	}

#region Public Function
	public void SetUp(Camera p_camera) {
		_camera = p_camera;
		rootFolder = Application.persistentDataPath +"/ScreenCapture/";

		ClearStorage();
	}

	public void Show(bool open) {
		if (cameraFooter != null)
			cameraFooter.gameObject.SetActive(open);
	}

	public void TakePicture() {
		if (_camera) {
			Texture2D screenTexture = RTImage(_camera);

			//Save to disk
			byte[] bytes = screenTexture.EncodeToJPG(jpgQuality);
			Object.Destroy(screenTexture);

			// For testing purposes, also write to a file in the project folder
			string subname = System.DateTime.Now.ToString().Replace("/", "_").Replace("\\", ":");
			string path = rootFolder + "SouvenirPic" + subname + ".png";

			if(!Directory.Exists(rootFolder))
			{
				Directory.CreateDirectory(rootFolder);
			}

			File.WriteAllBytes(path, bytes);
		}
	}

	public void ClearStorage() {
		foreach (string path in _souvenirPics)
			File.Delete(path);
	}

#endregion

#region Private Function
    Texture2D RTImage(Camera camera)
    {
		camera.targetTexture = tempRenderTexture;

        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();
		Debug.Log(camera.targetTexture);

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture, and remove targettexture from camera
        RenderTexture.active = currentRT;
		camera.targetTexture = null;

        return image;
    }
#endregion
}
