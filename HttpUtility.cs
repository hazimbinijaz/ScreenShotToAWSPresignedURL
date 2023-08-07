using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient 
{
    public static async Task<T> Get<T>(string endpoint)
    {
        var getRequest = CreateRequest(endpoint);
        getRequest.SendWebRequest();
        while (!getRequest.isDone) await Task.Delay(5);
        return JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
    }

    public static async Task<T> Post<T>(string endpoint, object data)
    {
        var postRequest = CreateRequest(endpoint,RequestType.POST,data);
        postRequest.SendWebRequest();

        while(!postRequest.isDone) await Task.Delay(2);
        
        return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
    }

    public static async Task<T> Put<T>(string endpoint, object data)
    {
        var postRequest = CreateRequest(endpoint,RequestType.PUT,data);
        postRequest.SendWebRequest();
    
        while(!postRequest.isDone) await Task.Delay(5);
        return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
    }
    
    public static UnityWebRequest CreateRequest(string path,RequestType type = RequestType.GET,object data= null)
    {
        var request = new UnityWebRequest(path,type.ToString());
        if (data != null)
        {
            var bodyRaw = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private static void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }
    
    public enum RequestType 
    {
        GET=0,
        POST=1,
        PUT=2,
    }
}
