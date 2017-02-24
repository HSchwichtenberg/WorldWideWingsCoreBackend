using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools
{
 public class Program
 {
  public static void Main(string[] args)
  {
   Console.WriteLine("Start...");
   GL.Datengenerator.Run(300);
   Console.WriteLine("Ende!");
   Console.ReadLine();
  }
 }
}
