using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ScreenShot : EditorWindow {

    const string WidthPrefName = "screenshot_width";
    const string HeightPrefName = "screenshot_height";
	const string PrependPrefName = "screenshot_prepend_name";
	const string QualityPrefName = "screenshot_quality";
	
	const string BasePathFormat = "{0}/../ScreenShots";
	const string FileNameFormat = "{0}{1}x{2}_{3}.jpg";
	
	static string BasePath;
	static string FileNameBaseFormat;

    [MenuItem("Labuurii SDK/ScreenShot")]
	static void TakeScreenShot() {
        if(string.IsNullOrEmpty(BasePath))
        {
            BasePath = string.Format(BasePathFormat, Application.dataPath);
            FileNameBaseFormat = BasePath + "/" + FileNameFormat;
        }

        var width = EditorPrefs.GetInt(WidthPrefName, 512);
        var height = EditorPrefs.GetInt(HeightPrefName, 512);
        var camera = Camera.current;
        if(camera == null)
        {
            Debug.Log("Could not take picture as there are no camera.");
            return;
        }

		RenderTexture rt = new RenderTexture(width, height, 24);
		camera.targetTexture = rt;
		Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
		camera.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		camera.targetTexture = null;
		RenderTexture.active = null;
		DestroyImmediate(rt);
		var bytes = screenShot.EncodeToJPG(EditorPrefs.GetInt(QualityPrefName, 75));
		var filename = ScreenShotName(width, height);
		var dir_name = Path.GetDirectoryName(filename);
		if(!Directory.Exists(dir_name))
			Directory.CreateDirectory(dir_name);
		
		File.WriteAllBytes(filename, bytes);
		Debug.Log(string.Format("Saved screenshot at: {0}", filename));
	}
 
     public static string ScreenShotName(int width, int height) {
         return string.Format(FileNameBaseFormat,
								EditorPrefs.GetString(PrependPrefName, ""),
								width, height, 
								System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
     }
	 
	 [MenuItem("Labuurii SDK/ScreenShot Settings")]
	 static void OpenScreenShotSettings() {
		 EditorWindow.GetWindow(typeof(ScreenShot));
	 }
	 
	 void OnGUI() {
		GUILayout.Label ("Resolution", EditorStyles.boldLabel);
        var new_width = EditorGUILayout.TextField ("Width", EditorPrefs.GetInt(WidthPrefName, 512).ToString());
		var new_height = EditorGUILayout.TextField("Height", EditorPrefs.GetInt(HeightPrefName, 512).ToString());
		var new_prepend = EditorGUILayout.TextField("Name prepend", EditorPrefs.GetString(PrependPrefName, ""));
		var new_quality = EditorGUILayout.TextField("Quality", EditorPrefs.GetInt(QualityPrefName, 75).ToString());
		int new_width_int, new_height_int, new_quality_int;
		if(int.TryParse(new_width, out new_width_int))
			EditorPrefs.SetInt(WidthPrefName, new_width_int);
        if (int.TryParse(new_height, out new_height_int))
            EditorPrefs.SetInt(HeightPrefName, new_height_int);
		EditorPrefs.SetString(PrependPrefName, new_prepend);
		if(int.TryParse(new_quality, out new_quality_int))
			EditorPrefs.SetInt(QualityPrefName, MathOps.Clamp(new_quality_int, 25, 100));
	 }
}