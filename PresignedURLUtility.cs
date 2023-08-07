using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class PresignedURLUtility
{

    public string EndPoint;
    /// <summary>
    ///   <para>Get Presigned URL.</para>
    /// </summary>
    /// <param name="PRS">Object to send the path of Presigned URL.</param>
    /// <returns>Returns the string path of the texture </returns>
    public async Task<GetAPIData> GetPresignedURL(PreSignedRequest PRS)
    {
        return await HttpClient.Post<GetAPIData>(EndPoint,
                PRS);
    }

    /// <summary>
    ///   <para>Sends the Texture to the Presigned URL to be uploaded.</para>
    /// </summary>
    /// <param name="PRS">GetAPIData object used to represent the path where the request is being sent.</param>
    /// <param name="texturetoUpload">The texture to be uploaded.</param>
    /// <returns>Returns the string URL of the texture </returns>
    public async Task<string> SendTextureForUploading(GetAPIData data,Texture2D texturetoUpload)
    {
        byte[] bytes = ImageConversion.EncodeArrayToJPG(texturetoUpload.GetRawTextureData(),texturetoUpload.graphicsFormat, (uint)texturetoUpload.width, (uint)texturetoUpload.height);
        UnityWebRequest request = HttpClient.CreateRequest(data.payload.Url, HttpClient.RequestType.PUT, bytes);
        request.SendWebRequest();
        while(!request.isDone) await Task.Delay(5);
        return request.result != UnityWebRequest.Result.Success ? "Failed to Upload Texture" : data.payload.Url.Split('?')[0];
    }
}

public class PreSignedRequest
{
    public string url { get; set; }
}
public class GetAPIData
{
    public string status { get; set; }
    public string message { get; set; }
    public Link payload { get; set; }
}
public class Link
{
    public string Url { get; set; }
    public string File_name { get; set; }
}