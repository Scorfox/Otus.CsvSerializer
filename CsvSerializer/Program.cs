// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using CsvSerializer.Models;
using CsvSerializer.Services;
using Newtonsoft.Json;

const int measurementsNumber = 1000;
const string fInstanceCsv = "I1;I2;I3;I4;I5\r\n1;2;3;4;5";
const string fInstanceJson = "{\"I1\":1,\"I2\":2,\"I3\":3,\"I4\":4,\"I5\":5}";

var csvSerializerMeasurements = new List<TimeSpan>();
var csvSerializerOutputMeasurements = new List<TimeSpan>();
var csvDeserializerMeasurements = new List<TimeSpan>();

var newtonSerializerMeasurements = new List<TimeSpan>();
var newtonSerializerOutputMeasurements = new List<TimeSpan>();
var newtonDeserializerMeasurements = new List<TimeSpan>();

ICsvSerializer csvSerializer = new CsvSerializer.Services.CsvSerializer();
var stopwatch = new Stopwatch();
var fInstance = new F { I1 = 1, I2 = 2, I3 = 3, I4 = 4, I5 = 5 };

// Csv Serializer
for (var i = 0; i < measurementsNumber; i++)
{
    stopwatch.Restart();
    var result = csvSerializer.Serialize(fInstance);
    stopwatch.Stop();
    csvSerializerMeasurements.Add(stopwatch.Elapsed);
    
    stopwatch.Restart();
    Console.WriteLine($"Serialized output: {result}");
    stopwatch.Stop();
    csvSerializerOutputMeasurements.Add(stopwatch.Elapsed);
}

// Newtonsoft Json Serializer
for (var i = 0; i < measurementsNumber; i++)
{
    stopwatch.Restart();
    var result = JsonConvert.SerializeObject(fInstance);
    stopwatch.Stop();
    newtonSerializerMeasurements.Add(stopwatch.Elapsed);
    
    stopwatch.Restart();
    Console.WriteLine($"Serialized output: {result}");
    stopwatch.Stop();
    newtonSerializerOutputMeasurements.Add(stopwatch.Elapsed);
}

// Csv DeSerializer
for (var i = 0; i < measurementsNumber; i++)
{
    stopwatch.Restart();
    var result = csvSerializer.Deserialize<F>(fInstanceCsv);
    stopwatch.Stop();
    csvDeserializerMeasurements.Add(stopwatch.Elapsed);
}

// Newtonsoft Json Deserializer
for (var i = 0; i < measurementsNumber; i++)
{
    stopwatch.Restart();
    var result = JsonConvert.DeserializeObject<F>(fInstanceJson);
    stopwatch.Stop();
    newtonDeserializerMeasurements.Add(stopwatch.Elapsed);
}

Console.WriteLine($"Csv Serializer Average took {csvSerializerMeasurements.Average(e => e.TotalNanoseconds):#.##} nanoseconds");
Console.WriteLine($"Csv Serializer Output Average took {csvSerializerOutputMeasurements.Average(e => e.TotalNanoseconds):#.##} nanoseconds");
Console.WriteLine($"Csv Deserializer Average took {csvDeserializerMeasurements.Average(e => e.TotalNanoseconds):#.##} nanoseconds");

Console.WriteLine($"Newtonsoft Serializer Average took {newtonSerializerMeasurements.Average(e => e.TotalNanoseconds):#.##} nanoseconds");
Console.WriteLine($"Newtonsoft Serializer Output Average took {newtonSerializerOutputMeasurements.Average(e => e.TotalNanoseconds):#.##} nanoseconds");
Console.WriteLine($"Newtonsoft Deserializer Average took {newtonDeserializerMeasurements.Average(e => e.TotalNanoseconds):#.##} nanoseconds");
