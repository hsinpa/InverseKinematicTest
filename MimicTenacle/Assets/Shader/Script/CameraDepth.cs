using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDepth : MonoBehaviour
{
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();    
    }

    void SetUp() {
        camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
    }

}
