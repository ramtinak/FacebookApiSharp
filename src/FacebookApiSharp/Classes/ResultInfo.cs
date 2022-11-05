/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.Responses;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FacebookApiSharp.Classes
{
    public class ResultInfo
    {
        public ResultInfo(string message)
        {
            Message = message;
            HandleMessages(message);
        }

        public ResultInfo(Exception exception)
        {
            Exception = exception;
            Message = exception?.Message;
            ResponseType = ResponseType.InternalException;
            HandleMessages(Message);
        }

        public ResultInfo(Exception exception, ResponseType responseType)
        {
            Exception = exception;
            Message = exception?.Message;
            ResponseType = responseType;
            HandleMessages(Message);
        }

        public ResultInfo(ResponseType responseType, string errorMessage)
        {
            ResponseType = responseType;
            Message = errorMessage;
            HandleMessages(errorMessage);
        }
        public ResultInfo(ResponseType responseType, DefaultResponse status)
        {
            ResponseType = responseType;
            HandleMessages(Message);
        }
        public void HandleMessages(string errorMessage)
        {
            if (errorMessage?.Contains("task was canceled") ?? false)
                Timeout = true;
        }

        public Exception Exception { get; internal set; }

        public string Message { get; }

        public ResponseType ResponseType { get; internal set; }

        public bool Timeout { get; internal set; }

        
        public List<FacebookErrorResult> Errors { get; internal set; } = new List<FacebookErrorResult>();

        public override string ToString()
        {
            return $"{ResponseType}: {Message}.";
        }
    }
}
