//using ClientTokenProvider.Business.Shared.Models.Abstractions;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//using System.Text.Json;

//namespace ClientTokenProvider.Persistence.Shared.Converters;


//internal sealed class ConfigurationDataConverter :
//    ValueConverter<IConfigurationData, string>
//{
//    public ConfigurationDataConverter(
//        JsonSerializerOptions? jsonSerializerOptions = null)
//        : base(
//            value => Serialize(value, jsonSerializerOptions),
//            value => Deserialize(value, jsonSerializerOptions))
//    {
//    }

//    private static string Serialize(
//     IConfigurationData value,
//     JsonSerializerOptions? jsonSerializerOptions = null)
//    {
//        return JsonSerializer.Serialize(
//            value,
//            jsonSerializerOptions);
//    }

//    private static IConfigurationData Deserialize(
//        string value,
//        JsonSerializerOptions? jsonSerializerOptions = null)
//    {
//        return JsonSerializer.Deserialize<IConfigurationData>(
//            value,
//            jsonSerializerOptions)!;
//    }
//}
