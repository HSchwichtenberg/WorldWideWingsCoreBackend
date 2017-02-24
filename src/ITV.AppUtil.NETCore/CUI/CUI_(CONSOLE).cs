using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using ITVisions;
using System.Diagnostics;

// (C) www.IT-Visions.de - Dr. Holger Schwichtenberg

namespace ITVisions
{

 /// <summary>
 /// Hilfsroutinen für Konsole-UIs
 /// </summary>
 public static class CUI
 {

  public static bool IsDebug = false;
  public static bool IsVerbose = false;

  [DllImport("kernel32.dll", ExactSpelling = true)]
  private static extern IntPtr GetConsoleWindow();

  private static IntPtr MyConsole = GetConsoleWindow();

  [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
  public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
  const int SWP_NOSIZE = 0x0001;

  public static void SetConsolePos(int xpos, int ypos)
  {
   SetWindowPos(MyConsole, 0, xpos, ypos, 0, 0, SWP_NOSIZE);
  }


  public static bool DOTNETTraceActive = true;

  public static string TraceDirectory = "";




  public static void StopTrace()
  {
   TraceDirectory = "";
  }

  /// <summary>
  /// Ausgabe an Console, Trace und Datei
  /// </summary>
  /// <param name="s"></param>
  public static void Print(string s)
  {
   //if (DOTNETTraceActive) Trace.WriteLine(s);
   Console.WriteLine(s);
  }


  /// <summary>
  /// Ausgabe an Console, Trace und Datei
  /// </summary>
  /// <param name="s"></param>
  public static void Print(object s)
  {
   //if (DOTNETTraceActive) Trace.WriteLine(s.ToString());
   Console.WriteLine(s.ToString());
  }

  public static void PrintSuccess(string s)
  {
   Print(s, ConsoleColor.Green);
  }

  public static void PrintError(string s)
  {
   Print(s, ConsoleColor.Red);
  }

  public static void MainHeadline(string s)
  {
   var altB = Console.BackgroundColor;
   var altF = Console.ForegroundColor;
   Console.BackgroundColor = ConsoleColor.Yellow;
   Console.ForegroundColor = ConsoleColor.Black;
   Console.WriteLine(s);
   Console.BackgroundColor = altB;
   Console.ForegroundColor = altF;
  }


  public static void Headline(string s)
  {
   Console.ForegroundColor = ConsoleColor.Yellow;
   Console.WriteLine(s);
   Console.ForegroundColor = ConsoleColor.Gray;
  }

  public static void HeaderFooter(string s)
  {
   Console.ForegroundColor = ConsoleColor.Green;
   Console.WriteLine(s);
   Console.ForegroundColor = ConsoleColor.Gray;
  }


  public static void Print(string s, ConsoleColor farbe)
  {
   object x = 1;
   lock (x)
   {
    ConsoleColor alteFarbe = Console.ForegroundColor;
    ConsoleColor alteHFarbe = Console.BackgroundColor;
    Console.ForegroundColor = farbe;
    if (farbe.ToString().Contains("Dark")) Console.BackgroundColor = ConsoleColor.White;
    Print(s);
    Console.ForegroundColor = alteFarbe;
    Console.BackgroundColor = alteHFarbe;
   }
  }



 }
}
