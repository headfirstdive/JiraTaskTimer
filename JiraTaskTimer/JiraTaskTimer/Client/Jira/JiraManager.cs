using System;
using System.IO;
using System.Net;
using System.Text;

public enum JiraResource
{
    project
}

public class JiraManager
{
    private readonly string m_BaseUrl = "http://localhost.:8080/rest/api/latest/";
    private string m_Username;
    private string m_Password;

    public JiraManager(string url, string username, string password)
    {
        m_BaseUrl   = url;
        m_Username  = username;
        m_Password  = password;
    }


    public void RunQuery(
        JiraResource resource,
        string argument = null,
        string data = null,
        string method = "GET")
    {
        string url = string.Format("{0}/{1}/", m_BaseUrl, resource.ToString());

        if (argument != null)
        {
            url = string.Format("{0}/{1}/", url, argument);
        }

        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        request.ContentType = "application/json";
        request.Method = method;

        if (data != null)
        {
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }
        }

        string base64Credentials = GetEncodedCredentials();
        request.Headers.Add("Authorization", "Basic " + base64Credentials);

        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        string result = string.Empty;
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            result = reader.ReadToEnd();
        }

        Console.WriteLine(result);
    }


    private string GetEncodedCredentials()
    {
        string mergedCredentials = string.Format("{0}:{1}", m_Username, m_Password);
        byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
        return Convert.ToBase64String(byteCredentials);
    }
}