using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CameraRender : MonoBehaviour
{

    public Camera _renderCam;
    public RenderTexture rt;

    public bool _render = false;
    public Texture2D _image;

    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.

        rt = new RenderTexture(Screen.width*2, Screen.height*2, 16, RenderTextureFormat.ARGB32);
        rt.Create();
        camera.targetTexture = rt;


        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        // Render the camera's view.
        camera.Render();

        

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }

    void Update()
    {
        if (_render)
        {
            _render = false;
            _image = RTImage(_renderCam);

            byte[] bytes = _image.EncodeToPNG();
            var stream = Application.dataPath + "/../exports/";

            if (!Directory.Exists(stream))
            {
                Directory.CreateDirectory(stream);
            }
            File.WriteAllBytes(stream + "MapImage.png", bytes);
        }
    }
}
