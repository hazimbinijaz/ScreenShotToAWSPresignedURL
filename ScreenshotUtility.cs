using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;



public class ScreenshotUtility 
{
    private TextureFormat transp = TextureFormat.ARGB32;
    private TextureFormat nonTransp = TextureFormat.RGB24;

    /// <summary>
    ///   <para>Capture function which is used to capture the screen.</para>
    /// </summary>
    /// <param name="imageSize">Represents the desired size of the image that is to be created.</param>
    /// <param name="enlargeCOEF">Enlarge Coefficent used to scale an image</param>
    /// <param name="OpenDirectoryAfterSave">Decides if the Screen shot should be saved in the a directory</param>
    /// <param name="SaveToDirectory">Decides if the directory where the image is saved should open up after saving</param>
    /// <returns>Returns a 2D Texture of the image captured in the Game View</returns>
    public Texture2D Capture(ImageSize imageSize, bool SaveToDirectory = false, bool OpenDirectoryAfterSave = false)
    {
        Texture2D screenShot = CreateTexture(imageSize);
        
        
        if(SaveToDirectory)
            this.SaveToDirectory(screenShot,imageSize);
        
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
        if (SaveToDirectory && OpenDirectoryAfterSave)
            OpenFileDirectory();
#endif

        return screenShot;
    }

    private Texture2D CreateTexture(ImageSize imageSize)
    {
        TextureFormat textForm = nonTransp;
        
        RenderTexture renderTexture = new RenderTexture(imageSize.Width * imageSize.EnlargeCOEF, imageSize.Height * imageSize.EnlargeCOEF, 24);
        Camera.main.targetTexture = renderTexture;
        Texture2D screenShot = new Texture2D(imageSize.Width * imageSize.EnlargeCOEF, imageSize.Height * imageSize.EnlargeCOEF, textForm, false);
        Camera.main.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, imageSize.Width * imageSize.EnlargeCOEF, imageSize.Height * imageSize.EnlargeCOEF), 0, 0);
        screenShot.Apply();
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Object.Destroy(renderTexture);
        return screenShot;
    }
    
    /// <summary>
    ///   <para>Saves the image to the path</para>
    /// </summary>
    /// <param name="screenShot">The texture of the Screen Shot</param>
    /// <param name="imageSize">Represents the size of the image</param>
    private void SaveToDirectory(Texture2D screenShot,ImageSize imageSize)
    {
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenshotName("Screenshot+ ", (imageSize.Width * imageSize.EnlargeCOEF).ToString(), (imageSize.Height * imageSize.EnlargeCOEF).ToString());
       
        if (!Directory.Exists(Application.persistentDataPath + "/../screenshots/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/../screenshots/");

        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }
    
    /// <summary>
    ///   <para>A method that opens the file directory where the screenshot is saved.</para>
    /// </summary>
    private void OpenFileDirectory()=>Process.Start(Application.persistentDataPath + "/../screenshots/");

    
    /// <summary>
    ///   <para>A method that create a name string for the screenshot.</para>
    /// </summary>
    /// /// <param name="platform">Platform name</param>
    /// /// <param name="width">Width of the image</param>
    /// /// <param name="height">Height of the image</param>
    private string ScreenshotName(string platform, string width, string height)
    {
        return string.Format("{0}/../screenshots/" + "_" + platform + "screen_{1}x{2}_{3}.png", Application.persistentDataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    
}

/// <summary>
///   <para>A class to represent the size of an image.</para>
/// </summary>
public class ImageSize
{
    public ImageSize(int x, int y)
    {
        Width = x;
        Height = y;
        EnlargeCOEF = 1;
    }
    public ImageSize(int x, int y, int enlargeCoef)
    {
        Width = x;
        Height = y;
        EnlargeCOEF = enlargeCoef;
    }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int EnlargeCOEF { get; private set; }
}