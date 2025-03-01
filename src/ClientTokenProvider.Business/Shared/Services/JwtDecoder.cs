﻿using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;

namespace ClientTokenProvider.Business.Shared.Services;

internal sealed class JwtDecoder :
    IJwtDecoder
{
    public DecodedJwt Decode(string jwtToken)
    {
        var jwtTokenParts = jwtToken.Split('.');

        var header = string.Empty;
        var payload = string.Empty;
        var signature = string.Empty;

        if (jwtTokenParts.Length > 0 &&
            TryDecodeJwtTokenPart(jwtTokenParts[0], out var decodedHeader))
        {
            header = decodedHeader;
        }

        if (jwtTokenParts.Length > 1 &&
            TryDecodeJwtTokenPart(jwtTokenParts[1], out var decodedPayload))
        {
            payload = decodedPayload;
        }

        // Signature cannot be decoded
        if (jwtTokenParts.Length > 2)
        {
            signature = jwtTokenParts[2];
        }

        return new DecodedJwt
        {
            Header = header,
            Payload = payload,
            Signature = signature
        };
    }

    private bool TryDecodeJwtTokenPart(
        string jwtTokenPart,
        out string decodedJwtTokenPart)
    {
        if (string.IsNullOrEmpty(jwtTokenPart))
        {
            decodedJwtTokenPart = string.Empty;

            return false;
        }

        var validJwtTokenPart = jwtTokenPart
               .Replace('_', '/')
               .Replace('-', '+');

        switch (validJwtTokenPart.Length % 4)
        {
            case 2: validJwtTokenPart += "=="; break;
            case 3: validJwtTokenPart += "="; break;
        }

        try
        {
            var bytes = Convert.FromBase64String(validJwtTokenPart);

            decodedJwtTokenPart = System.Text.Encoding.UTF8.GetString(bytes);

            return true;
        }
        catch
        {
            decodedJwtTokenPart = string.Empty;

            return false;
        }
    }
}
