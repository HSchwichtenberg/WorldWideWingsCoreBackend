
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ITVisions.EFC
{



 public static class DbContextExtensionLogging
 {

  public static void Log(this DbContext ctx, string path = "")
  {

   // ServiceProvider erzeugen
   var serviceProvider = ctx.GetInfrastructure<IServiceProvider>();
   // Logger-Factory hinzufügen
   var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
   // Provider zur Factory hinzufügen
   loggerFactory.AddProvider(new FileLoggerProvider(path, true));

  }


 }


 public class FileLoggerProvider : ILoggerProvider
 {
  private static List<string> _categories = new List<string>
  {
  typeof(RelationalCommand).FullName
//,
//typeof(Microsoft.Data.Entity.Storage.Internal.SqlServerConnection).FullName
  };

  //Kategorien
  //Microsoft.Data.Entity.DbContext
  //Microsoft.Data.Entity.Storage.Internal.SqlServerConnection
  //Microsoft.Data.Entity.Storage.Internal.RelationalCommandBuilderFactory
  //Microsoft.Data.Entity.Internal.RelationalModelValidator
  //Microsoft.Data.Entity.Query.Internal.SqlServerQueryCompilationContextFactory
  //Microsoft.Data.Entity.Query.ExpressionTranslators.Internal.SqlServerCompositeMethodCallTransl
  //Microsoft.Data.Entity.Query.Internal.QueryCompiler

  string Path;
  public FileLoggerProvider(string path, bool CommandsOnly = false)
  {
   this.Path = path;
   if (!CommandsOnly) _categories.Clear();

  }
  public ILogger CreateLogger(string categoryName)
  {
   Console.WriteLine("Logger festlegen für Kategorie: " + categoryName);
   return new FileLogger(this.Path);

   if (_categories.Count == 0 || _categories.Contains(categoryName))
   {
    return new FileLogger(this.Path);
   }

   return new NullLogger();
  }

  public void Dispose()
  { }

  private class FileLogger : ILogger
  {
   string Path;
   public FileLogger(string path)
   {
    this.Path = path;
   }
   public bool IsEnabled(LogLevel logLevel)
   {
    return true;
   }

   public static long Count = 0;

   public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
   {
    Count++;

    string text = $"{Count:000}:{logLevel} #{eventId.Id} {eventId.Name}:{formatter(state, exception)}";
    if (!String.IsNullOrEmpty(this.Path)) File.AppendAllText(this.Path, text);
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(text);
    System.Diagnostics.Debug.WriteLine(text);
    //if (text.Contains("insert")) { Count++; Console.WriteLine($"{Count:000}: INSERT");}
    Console.ForegroundColor = ConsoleColor.White;
   }

   public IDisposable BeginScope<TState>(TState state)
   {
    return null;
   }
  }

  private class NullLogger : ILogger
  {
   public bool IsEnabled(LogLevel logLevel)
   {
    return false;
   }



   public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
   {
    Console.WriteLine(formatter(state, exception));
    System.Diagnostics.Debug.WriteLine(formatter(state, exception));
   }

   public IDisposable BeginScope<TState>(TState state)
   {
    throw new NotImplementedException();
   }
  }
 }
}

