using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Logging
{
  public static class Logger
  {
    public static void Error(string source, string message)
    {
      Console.WriteLine($"ERROR: {source} > {message}");
    }

    public static void Info(string source, string message)
    {
      Console.WriteLine($"INFO: {source} > {message}");
    }

    public static void Warn(string source, string message)
    {
      Console.WriteLine($"WARN: {source} > {message}");
    }
  }
}
