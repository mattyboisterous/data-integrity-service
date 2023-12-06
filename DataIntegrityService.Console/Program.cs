using Microsoft.Extensions.Configuration;
using DataIntegrityService.Core.Configuration;
using DataIntegrityService.Core;

EntryPoint.Run();

//// Build a config object, using env vars and JSON providers.
//IConfigurationRoot config = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json")
//    .AddEnvironmentVariables()
//    .Build();

//// Get values from the config given their key and their target type.
//Settings? settings = config.GetRequiredSection("settings").Get<Settings>();

//// Write the values to the console.
//Console.WriteLine($"Environment = {settings?.Environment}");
//Console.WriteLine($"Version = {settings?.Version}");

//foreach(var service in settings!.DataServices)
//{
//  Console.WriteLine($"Key = {service.Key}");
//  Console.WriteLine($"DatasetName = {service.DatasetName}");
//  Console.WriteLine($"DatasetGroup = {service.DatasetGroup}");
//}