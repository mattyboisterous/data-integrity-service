using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Core.Logging
{
  public static class Logger
  {
    public static void Error(string message)
    {
      Console.WriteLine($"ERROR: {message}");
    }

    public static void Info(string message)
    {
      Console.WriteLine(message);
    }

    public static void Warn(string message)
    {
      Console.WriteLine($"WARN: {message}");
    }
  }
}
