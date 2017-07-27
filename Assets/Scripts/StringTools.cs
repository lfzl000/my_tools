using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class StringTools
{
    /// <summary>  
    /// Unicode字符串转为正常字符串0  
    /// </summary>  
    /// <param name="srcText">Unicode字符(格式为："\uxxxx" ，举例："\u67f3_abc123")</param>  
    /// <returns>正常字符串</returns>  
    public static string UnicodeToString(string srcText)
    {
        return Regex.Unescape(srcText);
    }

    /// <summary>  
    /// 字符串转为UniCode码字符串  
    /// </summary>  
    /// <param name="s">正常字符串</param>  
    /// <returns>Unicode字符(格式为："\uxxxx" ，举例："\u67f3_abc123")</returns>  
    public static string StringToUnicode(string s)
    {
        char[] charbuffers = s.ToCharArray();
        byte[] buffer;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < charbuffers.Length; i++)
        {
            buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
            sb.Append(String.Format("//u{0:X2}{1:X2}", buffer[1], buffer[0]));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 字符串转为UTF-8
    /// </summary>
    /// <param name="s">正常字符串</param>
    /// <returns>UTF-8字符串</returns>
    public static string StringToUTF8(string s)
    {
        byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(s);
        string str = "";

        foreach (byte b in buffer) str += string.Format("%{0:X}", b);
        return str;
    }

    /// <summary>
    /// UTF8转换成GB2312
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string utf8_gb2312(string text)
    {
        //声明字符集   
        System.Text.Encoding utf8, gb2312;
        //utf8   
        utf8 = System.Text.Encoding.GetEncoding("utf-8");
        //gb2312   
        gb2312 = System.Text.Encoding.GetEncoding("gb2312");
        byte[] utf;
        utf = utf8.GetBytes(text);
        utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
        //返回转换后的字符   
        return gb2312.GetString(utf);
    }
}