/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.Responses;
using FacebookApiSharp.Helpers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
namespace FacebookApiSharp.Classes
{
    public class Result<T> : IResult<T>
    {
        public Result(bool succeeded, T value, ResultInfo info)
        {
            Succeeded = succeeded;
            Value = value;
            Info = info;
        }

        public Result(bool succeeded, ResultInfo info)
        {
            Succeeded = succeeded;
            Info = info;
        }

        public Result(bool succeeded, T value)
        {
            Succeeded = succeeded;
            Value = value;
        }

        public bool Succeeded { get; }
        public T Value { get; }
        public ResultInfo Info { get; } = new ResultInfo("");
    }

    public static class Result
    {
        public static IResult<T> Success<T>(T resValue)
        {
            return new Result<T>(true, resValue, new ResultInfo(ResponseType.OK, "oK"));
        }

        public static IResult<T> Success<T>(string successMsg, T resValue)
        {
            return new Result<T>(true, resValue, new ResultInfo(ResponseType.OK, successMsg));
        }

        public static IResult<T> Fail<T>(Exception exception)
        {
            return new Result<T>(false, default, new ResultInfo(exception));
        }

        public static IResult<T> Fail<T>(string errMsg)
        {
            return new Result<T>(false, default, new ResultInfo(errMsg));
        }

        public static IResult<T> Fail<T>(string errMsg, T resValue)
        {
            return new Result<T>(false, resValue, new ResultInfo(errMsg));
        }

        public static IResult<T> Fail<T>(Exception exception, T resValue)
        {
            return new Result<T>(false, resValue, new ResultInfo(exception));
        }

        public static IResult<T> Fail<T>(Exception exception, T resValue, ResponseType responseType)
        {
            return new Result<T>(false, resValue, new ResultInfo(exception, responseType));
        }

        public static IResult<T> Fail<T>(ResultInfo info, T resValue)
        {
            return new Result<T>(false, resValue, info);
        }

        public static IResult<T> Fail<T>(string errMsg, ResponseType responseType, T resValue)
        {
            return new Result<T>(false, resValue, new ResultInfo(responseType, errMsg));
        }

        public static IResult<T> Fail<T>(Exception exception, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                var resultInfo = new ResultInfo(exception, ResponseType.UnExpectedResponse);
                return new Result<T>(false, default, resultInfo);
            }
            else
            {
                return new Result<T>(false, default, GenerateResult(json, null, exception));
            }
        }
        public static IResult<T> UnExpectedResponse<T>(HttpResponseMessage response, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                var resultInfo = new ResultInfo(ResponseType.UnExpectedResponse,
                    $"Unexpected response status: {response.StatusCode}");
                return new Result<T>(false, default, resultInfo);
            }
            else
            {
                return new Result<T>(false, default, GenerateResult(json));
            }
        }

        public static IResult<T> UnExpectedResponse<T>(HttpResponseMessage response, 
            string message,
            string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                var resultInfo = new ResultInfo(ResponseType.UnExpectedResponse,
                    $"{message}\r\nUnexpected response status: {response.StatusCode}");
                return new Result<T>(false, default, resultInfo);
            }
            else
            {
                return new Result<T>(false, default, GenerateResult(json, message));
            }
        }
        private static ResultInfo GenerateResult(string json, string userMessage = null, Exception exception = null)
        {
            if (json.Contains("\"error\""))
            {
                var obj = JsonConvert.DeserializeObject<FacebookErrorResultContainer>(json);
                string msg;
                if (!string.IsNullOrEmpty(userMessage))
                    msg = userMessage + Environment.NewLine + obj.Error?.Message;
                else
                    msg = obj.Error?.Message;

                var resultInfo = new ResultInfo(ResponseType.UnExpectedResponse, msg)
                {
                    Exception = exception
                };
                if(obj.Error?.Type == "OAuthException")
                {
                    resultInfo.ResponseType = ResponseType.OAuthException;
                }
                resultInfo.Errors.Add(obj.Error);
                return resultInfo;
            }
            else
            {
                var obj = JsonConvert.DeserializeObject<FacebookPaginationResultResponse<object>>(json);
                string msg = string.Empty;
                if (!string.IsNullOrEmpty(userMessage))
                    msg = userMessage + Environment.NewLine;

                if (obj.Errors?.Count > 0)
                {
                    foreach (var error in obj.Errors)
                    {
                        if (!string.IsNullOrEmpty(error.Summary))
                        {
                            msg += $"{error.Message}{Environment.NewLine}//{Environment.NewLine}";
                        }
                        else
                        {
                            msg += $"{error.Summary}{Environment.NewLine}{error.Description}{Environment.NewLine}//{Environment.NewLine}";
                        }
                    }
                }

                return new ResultInfo(ResponseType.UnExpectedResponse, msg)
                {
                    Errors = obj.Errors,
                    Exception = exception
                };
            }
        }
    }
}
