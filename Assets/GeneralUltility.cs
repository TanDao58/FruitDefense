using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class GeneralUltility
{
    private const string SEPARATOR = "";
    private static StringBuilder buidler = new StringBuilder();
    public static bool enableDebug = true;

    public static string BuildString(string[] collection, string separator = SEPARATOR)
    {
        buidler.Clear();
        for (int i = 0; i < collection.Length; i++)
        {
            buidler.AppendFormat("{0}{1}",separator,collection[i]);
        }
        return buidler.ToString();
    }

    public static string BuildString(string separator = SEPARATOR, params string[] collection)
    {
        buidler.Clear();
        for (int i = 0; i < collection.Length; i++)
        {
            buidler.AppendFormat("{0}{1}", separator, collection[i]);
        }
        return buidler.ToString();
    }

    public static void Log(params string[] collection) {
        buidler.Clear();
        for (int i = 0; i < collection.Length; i++) {
            buidler.AppendFormat("{0}", collection[i]);
        }
        if (enableDebug) {
            Debug.Log(buidler.ToString());
        }
    }

    public static void LogError(params string[] collection)
    {
        buidler.Clear();
        for (int i = 0; i < collection.Length; i++)
        {
            buidler.AppendFormat("{0}", collection[i]);
        }
        if (enableDebug)
        {
            Debug.LogError(buidler.ToString());
        }
    }
}
