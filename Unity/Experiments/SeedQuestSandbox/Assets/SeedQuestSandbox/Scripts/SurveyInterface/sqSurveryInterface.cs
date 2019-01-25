﻿using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class sqSurveyInterface
{

    // POSTs the survey response data to the server. Will need to have the URL changed eventually
    public static IEnumerator postRequest(string url=null, string textResponse=null)
    {
        if (url==null)
            url = "http://localhost:8080/surveys";

        string json = jsonBodyBuilder("hello from unity");

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    // Sends a GET request to the survey server 
    public static IEnumerator getRequest(string url=null)
    {
        string getResult;
        if (url == null)
            url = "http://localhost:8080/surveys";

        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        getResult = uwr.downloadHandler.text;

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        string[] getData = getResult.Split(':');
    
    }

    // I'm not 100% sure what the final survey will look like, but here's a preliminary 
    //  function for formatting the JSON for the POST request
    public static string jsonBodyBuilder(List<string> questions, List<string> responses)
    {
        string dateTime = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
        string testName = "xyz";
        string testEmail = "xyz@domain.com";

        Debug.Log("Date: " + dateTime);

        string body;
        string textResponseOne;

        textResponseOne = groupResponses(questions, responses);

        body = "{";

        body += "\"Name\": \"" + testName + "\",";
        body += "\"Email\": \"" + testEmail + "\",";
        body += "\"Response\": \"" + textResponseOne + "\"";

        body += "}";
        Debug.Log("Json body: " + body);

        return body;
    }

    // Another JSON formatter function. Again, don't know what the survey will look like yet
    public static string responseFormatter(string questionId, string userResponse)
    {
        string json = "\"" + questionId + "\": \"" + userResponse + "\"";
        return json;
    }

    // Another JSON formatter function. Again, don't know what the survey will look like yet
    public static string groupResponses(List<string> questions, List<string> responses)
    {
        if (questions.Count != responses.Count)
        {
            Debug.Log("Error: insufficient responses for number of questions");
            return "";
        }

        string json = "{";

        for (int i = 0; i < questions.Count; i++)
        {
            json += responseFormatter(questions[i], responses[i]);
            if (i + 1 != questions.Count)
                json += ",";
        }

        Debug.Log("Json group formatted: " + json);

        return json;
    }


}