using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresignedURLTextureUploader:MonoBehaviour 
{
    private PreSignedRequest ExampleURL;
    private ScreenshotUtility screenshotUtility;
    private PresignedURLUtility presignedURLUtility;

    [ContextMenu("Start Process")]
    public void StartProcessToUploadTexture()
    {
        SnapAndUploadToPresignedURL();
    }
    
    public async void SnapAndUploadToPresignedURL()
    {
        ExampleURL = new PreSignedRequest();
        screenshotUtility = new ScreenshotUtility();
        presignedURLUtility = new PresignedURLUtility();
        ExampleURL.url = "/123/1234/" + Random.Range(10, 80) + "54324/img.png";
        
        
        Texture2D ss = screenshotUtility.Capture(new ImageSize(1920,1080));
        GetAPIData data=await presignedURLUtility.GetPresignedURL(ExampleURL);
        string path = await presignedURLUtility.SendTextureForUploading(data, ss);
        Debug.Log("The PAth is: " + path);
    }
}
