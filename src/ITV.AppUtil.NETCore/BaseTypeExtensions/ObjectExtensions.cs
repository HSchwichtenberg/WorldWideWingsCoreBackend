using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
namespace ITVisions
{
 public static class ObjectExtensions
 {
  /// <summary>
  /// Kopieren gleichnamiger Attribut auf ein anderes, neues Objekt
  /// </summary>
  //public static T CopyTo<T>(this object from)
  //   where T : new()
  //{
  // T to = new T();
  // return CopyTo<T>(from, to);
  //}
  /// <summary>
  /// Kopieren gleichnamiger Attribut auf ein anderes, bestehendes Objekt
  /// </summary>
  //public static T CopyTo<T>(this object from, T to)
  //    where T : new()
  //{
  // Type FromType = from.GetType();
  // Type ToType = to.GetType();
  // // Kopieren der einzelnen Fields
  // foreach (FieldInfo f in FromType.GetFields())
  // {
  //  FieldInfo t = ToType.GetField(f.Name);
  //  if (t != null)
  //  {
  //   t.SetValue(to, f.GetValue(from));
  //  }
  // }
  // // Kopieren der einzelnen Properties
  // foreach (PropertyInfo f in FromType.GetProperties())
  // {
  //  object[] Empty = new object[0];
  //  PropertyInfo t = ToType.GetProperty(f.Name);
  //  if (t != null)
  //  {
  //   //Console.WriteLine(f.GetValue(from, Empty));
  //   t.SetValue(to, f.GetValue(from, Empty), Empty);
  //  }
  // }
  // return to;
  //}
  ///// <summary>
  ///// Liefert den Inhalt eines Objekts als Zeichenkette nur der Werte mit Zeilenumbruch
  ///// </summary>
  ///// <param name="obj"></param>
  ///// <param name="OnlyWithValue"></param>
  ///// <returns></returns>
  //public static string AsValueStringCSV(this Object obj, string Trenner = ";", bool OnlyWithValue = false)
  //{
  // Dictionary<string, object> inhalt = obj.AsNameValueDictionary(OnlyWithValue);
  // string s = "";
  // foreach (var de in inhalt)
  // {
  //  if (s != "")
  //  {
  //   s += Trenner;
  //  }
  //  s += de.Value;
  // }
  // return s;
  //}
  //public static string AsValueStringCR(this Object obj, bool OnlyWithValue = false)
  //{
  // return ToNameValueString(obj, "\n", OnlyWithValue, showNames);
  //}
  /// <summary>
  /// Liefert den Inhalt eines Objekts als Zeichenketten der Form Name:Wert
  /// </summary>
  public static string ToNameValueString(this Object obj, bool onlyWithValue = false, string attributeSeparator = "\n", string nameValueSeparator = ": ", bool showNames = true)
  {
   if (obj == null) return "";
   if (attributeSeparator == "\n") attributeSeparator = System.Environment.NewLine;
   SortedDictionary<string, object> inhalt;
   if (obj is string || obj is ValueType || obj is Version)
   {
    inhalt = new SortedDictionary<string, object>() { { "Value", obj.ToString() } };
   }
   else
   {
    inhalt = obj.ToNameValueDictionary(onlyWithValue);
   }
   StringBuilder sb = new StringBuilder();
   foreach (var de in inhalt)
   {
    if (sb.Length > 0)
    {
     sb.Append(attributeSeparator);
    }
    sb.Append((showNames ? de.Key + nameValueSeparator : "") + de.Value);
   }
   return sb.ToString();
  }
  /// <summary>
  /// Liefert den Inhalt eines Objekts als Dictionary-Objekt mit Namen und Werten
  /// </summary>
  /// <param name="obj">Das auszugebende Objekt</param>
  /// <param name="onlyWithValue">Nur Fields und Properties ausgeben, die einen Wert haben</param>
  /// <param name="skipException">Fehler nicht ausgeben</param>
  /// <returns></returns>
  public static SortedDictionary<string, object> ToNameValueDictionary(this Object obj, bool onlyWithValue = false, bool skipException = false)
  {
   SortedDictionary<string, object> result = new SortedDictionary<string, object>();
   if (obj == null) return result;
   System.Type t = obj.GetType();
   // --- Schleife über alle Fields
   foreach (FieldInfo f in t.GetRuntimeFields())
   {
    try
    {
     object Wert = f.GetValue(obj);
     if (onlyWithValue == false || (Wert != null && Wert.ToString() != "") && Wert.ToString() != f.FieldType.ToString())
     {
      if (!result.ContainsKey(f.Name)) AddToDictionary(result, f.Name, Wert, onlyWithValue, f.FieldType);
     }
    }
    catch (Exception ex)
    {
     if (!skipException && !result.ContainsKey(f.Name)) result.Add(f.Name, ex.Message);
    }
   }
   // --- Schleife über alle Properties
   foreach (PropertyInfo p in t.GetRuntimeProperties())
   {
    try
    {
     object Wert = p.GetValue(obj, null);
     if (onlyWithValue == false || (Wert != null && Wert.ToString() != "") && Wert.ToString() != p.PropertyType.ToString())
     {
      //if (p.Name == "testname") Debugger.Break();
      if (!result.ContainsKey(p.Name)) AddToDictionary(result, p.Name, Wert, onlyWithValue, p.PropertyType);
     }
    }
    catch (Exception ex)
    {
     if (!skipException && !result.ContainsKey(p.Name)) result.Add(p.Name, ex.Message);
    }
   }
   return result;
  }
  /// <summary>
  /// Hilfsroutine, die Werte an Dictionary anfügt und dabei über Liste iteriert, wenn Wert eine Liste ist
  /// </summary>
  private static SortedDictionary<string, object> AddToDictionary(SortedDictionary<string, object> dic, string name, Object value, bool onlyWithValue, Type type, string separator = ";")
  {
   if (value is IEnumerable && !(value is string))
   {
    StringBuilder sb = new StringBuilder();
    foreach (var item in (value as IEnumerable))
    {
     if (onlyWithValue == false || (item != null && item.ToString() != "") && item.ToString() != type.ToString())
     {
      sb.Append((sb.Length > 0 ? separator : "") + item.ToString());
     }
    }
    dic.Add(name, sb.ToString());
   }
   else
   {
    if (onlyWithValue == false || (value != null && value.ToString() != "") && value.ToString() != type.ToString())
    {
     dic.Add(name, value);
    }
   }
   return dic;
  }
 }
}