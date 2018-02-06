 using UnityEngine;
 using System.Collections;
 using System.IO;
 // To take screenshots with but not necessary.
 public class ScreenShotter : MonoBehaviour
 {
     public int captureWidth = 1920;
     public int captureHeight = 1080;
 
     public GameObject hideGameObject; 
 
     public bool optimizeForManyScreenshots = true;
 
     public enum Format { RAW, JPG, PNG, PPM };
     public Format format = Format.PNG;
 
     public string folder;
 
     private Rect rect;
     private RenderTexture renderTexture;
     private Texture2D screenShot;
     private int counter = 0; // image #
 
     private bool captureScreenshot = false;
     private bool captureVideo = false;
 
     private string uniqueFilename(int width, int height)
     {
         if (folder == null || folder.Length == 0)
         {
             folder = Application.dataPath;
             if (Application.isEditor)
             {
                 // put screenshots in folder above asset path so unity doesn't index the files
                 var stringPath = folder + "/..";
                 folder = Path.GetFullPath(stringPath);
             }
             folder += "/screenshots";
 
             // make sure directoroy exists
             System.IO.Directory.CreateDirectory(folder);
 
             // count number of files of specified format in folder
             string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
             counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
         }
 
         // use width, height, and counter for unique file name
         var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());
 
         // up counter for next call
         ++counter;
 
         // return unique filename
         return filename;
     }
 
     public void CaptureScreenshot()
     {
         captureScreenshot = true;
     }
 
     void Update()
     {
         // check keyboard 'k' for one time screenshot capture and holding down 'v' for continious screenshots
         captureScreenshot |= Input.GetKeyDown(KeyCode.PageUp);
 
         if (captureScreenshot)
         {
             captureScreenshot = false;
 
             // hide optional game object if set
             if (hideGameObject != null) hideGameObject.SetActive(false);
 
             // create screenshot objects if needed
             if (renderTexture == null)
             {
                 // creates off-screen render texture that can rendered into
                 rect = new Rect(0, 0, captureWidth, captureHeight);
                 renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                 screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
             }
         
             Camera camera = Camera.main; // NOTE: added because there was no reference to camera in original script; must add this script to Camera
             camera.targetTexture = renderTexture;
             camera.Render();
 
             RenderTexture.active = renderTexture;
             screenShot.ReadPixels(rect, 0, 0);
 
             camera.targetTexture = null;
             RenderTexture.active = null;
 
             // get our unique filename
             string filename = uniqueFilename((int) rect.width, (int) rect.height);
 
             byte[] fileHeader = null;
             byte[] fileData = null;
             if (format == Format.RAW)
             {
                 fileData = screenShot.GetRawTextureData();
             }
             else if (format == Format.PNG)
             {
                 fileData = screenShot.EncodeToPNG();
             }
             else if (format == Format.JPG)
             {
                 fileData = screenShot.EncodeToJPG();
             }
             new System.Threading.Thread(() =>
             {
                 // create file and write optional header with image bytes
                 var f = System.IO.File.Create(filename);
                 if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                 f.Write(fileData, 0, fileData.Length);
                 f.Close();
                 Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
             }).Start();
 
             // unhide optional game object if set
             if (hideGameObject != null) hideGameObject.SetActive(true);
 
             // cleanup if needed
             if (optimizeForManyScreenshots == false)
             {
                 Destroy(renderTexture);
                 renderTexture = null;
                 screenShot = null;
             }
         }
     }
 }
 