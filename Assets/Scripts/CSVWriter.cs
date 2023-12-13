using UnityEngine;
using System;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    private string _Title;
    [SerializeField] string filename;
    private void Start()
    {
        _Title = "Day" + System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "h_" + System.DateTime.Now.Minute.ToString() + "min_" + System.DateTime.Now.Second.ToString() + "sec";
    }

    public void WriteCSV(string line)
    {
        StreamWriter streamWriter;
        FileInfo fileInfo;
        fileInfo = new FileInfo(Application.dataPath + "/StreamingAssets/" + filename + "_" + _Title + ".csv");
        streamWriter = fileInfo.AppendText();
        streamWriter.WriteLine(line);
        streamWriter.Flush();
        streamWriter.Close();
    }
    
}
