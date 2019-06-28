using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogController : MonoBehaviour
{
    string fileName = "./Assets/Log/debug.txt";

    public void Clear()
    {
        Write("", false);
    }

    public void Log(string s)
    {
        Write(s);
    }

    private void Write(string s, bool append = true)
    {
        StreamWriter sw = new StreamWriter(this.fileName, append);
        sw.WriteLine(s);
        sw.Flush();
        sw.Close();
    }
}
