﻿using System.Text.Json.Serialization;

namespace Application.Common
{
    public class BaseResponse<T> where T : class
    {
        public T Data { get; set; }
        public Error Error { get; set; } = Error.None;

        [JsonIgnore]
        public bool HasError => this.Error != Error.None;
    }
}
