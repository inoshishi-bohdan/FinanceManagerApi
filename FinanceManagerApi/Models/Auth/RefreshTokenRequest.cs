﻿namespace FinanceManagerApi.Models.Auth
{
    public class RefreshTokenRequest
    {
        public int? UserId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
