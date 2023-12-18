using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class BlackJackRecorder : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void DownloadFile(string filename, string content);

    [SerializeField] BlackJackManager _BlackJackManager;
    private PracticeSet _PracticeSet => _BlackJackManager._PracticeSet;
    public List<int> MyNumberList { get; set; } = new List<int>();
    public List<int> YourNumberList { get; set; } = new List<int>();
    public List<Vector3> MySelectedNumberList { get; set; } = new List<Vector3>();
    public List<Vector3> YourSelectedNumberList { get; set; } = new List<Vector3>();
    public List<bool> ScoreList { get; set; } = new List<bool>();
    private List<List<Vector3>> MyCardsPracticeList => _PracticeSet.MyCardsPracticeList;
    private List<Vector3> FieldCardsPracticeList => _PracticeSet.FieldCardsPracticeList;
    private int TrialAll => _PracticeSet.TrialAll;
    private List<float> MySelectedTime => _PracticeSet.MySelectedTime;
    private List<float> YourSelectedTime => _PracticeSet.YourSelectedTime;

    public void RecordResult(int mynumber, int yournumber, Vector3 myselectednumber, Vector3 yourselectednumber, bool score)
    {
        MyNumberList.Add(mynumber);
        YourNumberList.Add(yournumber);
        MySelectedNumberList.Add(myselectednumber);
        YourSelectedNumberList.Add(yourselectednumber);
        ScoreList.Add(score);
    }
    private string _Title;
    private void Start()
    {
        _Title = "Day" + System.DateTime.Now.Day.ToString() + "_" + ((System.DateTime.Now.Hour < 10) ? ("0"+ System.DateTime.Now.Hour.ToString()): System.DateTime.Now.Hour.ToString()) + "h_" + ((System.DateTime.Now.Minute < 10) ? ("0" + System.DateTime.Now.Minute.ToString()) : System.DateTime.Now.Minute.ToString()) + "min_" + ((System.DateTime.Now.Second < 10) ? ("0" + System.DateTime.Now.Second.ToString()) : System.DateTime.Now.Second.ToString()) + "sec";
    }
    string WriteContent()
    {
        string Content = "";
        Content += "FieldNumber_x,FieldNumber_y,FieldNumber_z";
        for (int i = 0; i < MyCardsPracticeList[0].Count; i++) Content += ",MyCards" + (i + 1).ToString() + "_x" + ",MyCards" + (i + 1).ToString() + "_y" + ",MyCards" + (i + 1).ToString() + "_z";
        Content += ",MyNumber,YourNumber,MySelectedNumber_x,MySelectedNumber_y,MySelectedNumber_z,YourSelectedNumber_x,YourSelectedNumber_y,YourSelectedNumber_z,MySelectedTime,YourSelectedTime,Score\n";
        for (int i = 0; i < TrialAll; i++)
        {
            Content += FieldCardsPracticeList[i].x.ToString() + "," + FieldCardsPracticeList[i].y.ToString() + "," + FieldCardsPracticeList[i].z.ToString();
            for (int j = 0; j < MyCardsPracticeList[i].Count; j++) Content += "," + MyCardsPracticeList[i][j].x.ToString() + "," + MyCardsPracticeList[i][j].y.ToString() + "," + MyCardsPracticeList[i][j].z.ToString();
            Content += "," + MyNumberList[i].ToString() + "," + YourNumberList[i].ToString() + "," + MySelectedNumberList[i].x.ToString() + "," + MySelectedNumberList[i].y.ToString() + "," + MySelectedNumberList[i].z.ToString() + "," + YourSelectedNumberList[i].x.ToString() + "," + YourSelectedNumberList[i].y.ToString() + "," + YourSelectedNumberList[i].z.ToString() + "," + MySelectedTime[i].ToString() + "," + YourSelectedTime[i].ToString() + "," + ScoreList[i].ToString() + "\n";
        }
        return Content;
    }
    public void ExportCsv()
    {
        DownloadFile("result_monsterslayer_" + _Title + ".csv", WriteContent());
    }

    /*public void WriteResult()
    {
        string Content = "";
        Content += "FieldNumber";
        for (int i = 0; i < MyCardsPracticeList[0].Count; i++) Content += ",MyCards" + (i + 1).ToString();
        for (int i = 0; i < YourCardsPracticeList[0].Count; i++) Content += ",YourCards" + (i + 1).ToString();
        Content += ",MyNumber,YourNumber,Score\n";
        for(int i = 0;i < TrialAll; i++)
        {
            Content += FieldCardsPracticeList[i].ToString();
            for (int j = 0; j < MyCardsPracticeList[i].Count; j++) Content += "," + MyCardsPracticeList[i][j].ToString();
            for (int j = 0; j < YourCardsPracticeList[i].Count; j++) Content += "," + YourCardsPracticeList[i][j].ToString();
            Content += "," + MyNumberList[i].ToString() + "," + YourNumberList[i].ToString() + "," + ScoreList[i].ToString() + "\n";
        }
        _CSVWriter.WriteCSV(Content);
    }*/
}
