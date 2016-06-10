using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TimeGallery.Models.Javascript
{
    public class RequestResult
    {
        [JsonProperty("state")]
        public RequestResultTypeDefine State { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }

        public RequestResult(RequestResultTypeDefine state, string message = "")
        {
            State = state;
            Message = message;
        }
    }

    public class RequestResult<T>
    {
        [JsonProperty("state")]
        public RequestResultTypeDefine State { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        public RequestResult(RequestResultTypeDefine state, string message = "")
        {
            State = state;
            Message = message;
        }
    }

    public enum RequestResultTypeDefine
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 1
    }
}