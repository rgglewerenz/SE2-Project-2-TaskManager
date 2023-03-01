// See https://aka.ms/new-console-template for more information
using DataAcess;
using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Globalization;
using TaskChecker;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

        TaskProgram program = new TaskProgram(config);

        await program.Run();
    }
}




