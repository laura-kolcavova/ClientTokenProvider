using KittyToken.Application.Shared.JwtDecoding;

namespace KittyToken.Application.Tests.Shared.JwtDecoding;

public class JwtDecoderTests
{
    [Fact]
    public void Decode_ValidJwt_ReturnsDecodedHeaderPayloadAndRawSignature()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9" +
            ".eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ" +
            ".SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"alg\":\"HS256\",\"typ\":\"JWT\"}", decodedJwt.Header);
        Assert.Equal("{\"sub\":\"1234567890\",\"name\":\"John Doe\",\"iat\":1516239022}", decodedJwt.Payload);
        Assert.Equal("SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c", decodedJwt.Signature);
    }

    [Fact]
    public void Decode_PartLengthModFourIsTwo_AddsPaddingAndDecodes()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhIjoxfQ.signature";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"a\":1}", decodedJwt.Payload);
    }

    [Fact]
    public void Decode_PartLengthModFourIsThree_AddsPaddingAndDecodes()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhYiI6MX0.signature";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"ab\":1}", decodedJwt.Payload);
    }

    [Fact]
    public void Decode_PartContainsBase64UrlCharacters_DecodesPart()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ4Ijoifn5-Pz8_Pj4-In0.signature";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"x\":\"~~~???>>>\"}", decodedJwt.Payload);
    }

    [Fact]
    public void Decode_EmptyString_ReturnsEmptyParts()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = string.Empty;

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal(string.Empty, decodedJwt.Header);
        Assert.Equal(string.Empty, decodedJwt.Payload);
        Assert.Equal(string.Empty, decodedJwt.Signature);
    }

    [Fact]
    public void Decode_OnlyHeader_ReturnsHeaderAndEmptyPayloadAndSignature()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"alg\":\"HS256\",\"typ\":\"JWT\"}", decodedJwt.Header);
        Assert.Equal(string.Empty, decodedJwt.Payload);
        Assert.Equal(string.Empty, decodedJwt.Signature);
    }

    [Fact]
    public void Decode_HeaderAndPayloadWithoutSignature_ReturnsEmptySignature()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhIjoxfQ";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"alg\":\"HS256\",\"typ\":\"JWT\"}", decodedJwt.Header);
        Assert.Equal("{\"a\":1}", decodedJwt.Payload);
        Assert.Equal(string.Empty, decodedJwt.Signature);
    }

    [Fact]
    public void Decode_EmptyPayload_ReturnsEmptyPayloadAndKeepsOtherParts()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..signature";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"alg\":\"HS256\",\"typ\":\"JWT\"}", decodedJwt.Header);
        Assert.Equal(string.Empty, decodedJwt.Payload);
        Assert.Equal("signature", decodedJwt.Signature);
    }

    [Fact]
    public void Decode_InvalidBase64Header_ReturnsEmptyHeaderAndDecodesPayload()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "not*base64.eyJhIjoxfQ.signature";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal(string.Empty, decodedJwt.Header);
        Assert.Equal("{\"a\":1}", decodedJwt.Payload);
        Assert.Equal("signature", decodedJwt.Signature);
    }

    [Fact]
    public void Decode_MoreThanThreeParts_IgnoresExtraParts()
    {
        // Arrange
        var jwtDecoder = new JwtDecoder();

        var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhIjoxfQ.signature.extra";

        // Act
        var decodedJwt = jwtDecoder.Decode(jwtToken);

        // Assert
        Assert.Equal("{\"alg\":\"HS256\",\"typ\":\"JWT\"}", decodedJwt.Header);
        Assert.Equal("{\"a\":1}", decodedJwt.Payload);
        Assert.Equal("signature", decodedJwt.Signature);
    }
}
