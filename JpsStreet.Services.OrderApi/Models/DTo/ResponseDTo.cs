﻿namespace JpsStreet.Services.OrderApi.Models.DTo
{
    public class ResponseDTo
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}