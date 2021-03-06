﻿using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public static class sqSurveyInterface
{

    // POSTs the survey response data to the server. Will need to have the URL changed eventually
    //  used for testing purposes
    public static IEnumerator testPostRequest(string url = null, string textResponse = null)
    {
        if (url == null)
            url = "http://178.128.0.208:8000/surveys";

        Debug.Log("url: " + url);

        List<string> questions = new List<string>();
        List<string> responses = new List<string>();

        questions.Add("q1");
        questions.Add("q2");
        questions.Add("bad question{}:><>?{}}}{");
        questions.Add("q4");
        questions.Add("q5");
        questions.Add("q6");
        questions.Add("q7");
        questions.Add("q8");
        questions.Add("q9");
        questions.Add("q10");

        responses.Add("r1");
        responses.Add("r2");
        responses.Add("bad response{}:><>?{}}}{\"\'/");
        responses.Add("0123456789");
        responses.Add("0123456789");
        responses.Add("0123456789");
        responses.Add("0123456789");
        responses.Add("0123456789");
        responses.Add("0123456789");

        string json = jsonBodyBuilder(questions, responses);

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.useHttpContinue = false;

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            //Application.Quit();
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            //Application.Quit();
        }
    }

    // POSTs the survey response data to the server. Will need to have the URL changed eventually
    public static IEnumerator postRequest(List<string> questions, List<string> responses, string url = null)
    {
        if (url == null)
            url = "http://178.128.0.208:8000/surveys";

        string json = jsonBodyBuilder(questions, responses);

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.useHttpContinue = false;

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            Application.Quit();
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Application.Quit();
        }

    }

    // Sends a GET request to the survey server - probably not needed within Unity
    public static IEnumerator getRequest(string url = null)
    {
        string getResult;
        if (url == null)
            url = "http://localhost:8000/surveys";

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
        //Debug.Log("Date: " + dateTime);

        string version = Application.version;
        //Debug.Log("Version number: " + version);

        string body;
        string response = groupResponses(questions, responses); ;

        body = "{";
        body += "\"Response\": " + response;
        body += ",\"Version\": \"" + version + "\"";
        //body += ",\"Timestamp\": \"" + dateTime + "\"";
        body += "}";
        //Debug.Log("Json body: " + body);

        return body;
    }

    // Another JSON formatter function. Again, don't know what the survey will look like yet
    public static string responseFormatter(string questionId, string userResponse)
    {
        questionId = sanitizeInput(questionId);
        userResponse = sanitizeInput(userResponse);
        if (userResponse.Length <= 0)
            userResponse = ".";
        
        string json = "\"" + questionId + "\": \"" + userResponse + "\"";
        return json;
    }

    // Another JSON formatter function. Again, don't know what the survey will look like yet
    public static string groupResponses(List<string> questions, List<string> responses)
    {
        int maxResponses = Math.Min(questions.Count, responses.Count);
        string json = "{";

        for (int i = 0; i < maxResponses; i++)
        {
            json += responseFormatter(questions[i], responses[i]);
            if (i + 1 != maxResponses)
                json += ",";
        }

        json += "}";
        Debug.Log("Json group formatted: " + json);

        return json;
    }

    // Sanitize input strings by replacing invalid characters with empty strings
    public static string sanitizeInput(string input)
    {
        if (input != null)
        {
            try
            {
                return Regex.Replace(input, @"[^\w\.@\s-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
        return "";
    }

    // I'm not sure what format the responses will be in - this may be helpful
    public static void addToLists(List<string> questions, List<string> responses, string quetsionToAdd, string responseToAdd)
    {
        questions.Add(quetsionToAdd);
        responses.Add(responseToAdd);
    }

}