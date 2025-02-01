﻿using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.AzureAd.Models;

public sealed class AzureAdConfigurationData :
    IConfigurationData
{
    public required string AuthorityUrl { get; init; }

    public required string Scope { get; init; }

    public required string Audience { get; init; }

    public required string ClientId { get; init; }

    public required string ClientSecret { get; init; }

    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Audience) ||
             string.IsNullOrEmpty(AuthorityUrl) ||
             string.IsNullOrEmpty(ClientId) ||
             string.IsNullOrEmpty(ClientSecret) ||
             string.IsNullOrEmpty(Scope))
        {
            return false;
        }

        return true;
    }

    public bool IsEmpty()
    {
        if (string.IsNullOrEmpty(Audience) &&
            string.IsNullOrEmpty(AuthorityUrl) &&
            string.IsNullOrEmpty(ClientId) &&
            string.IsNullOrEmpty(ClientSecret) &&
            string.IsNullOrEmpty(Scope))
        {
            return true;
        }

        return false;
    }

    public static AzureAdConfigurationData Empty =>
        new()
        {
            AuthorityUrl = string.Empty,
            Scope = string.Empty,
            Audience = string.Empty,
            ClientId = string.Empty,
            ClientSecret = string.Empty,
        };
}