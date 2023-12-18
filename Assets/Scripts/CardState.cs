using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Renderer))]
public class CardState : MonoBehaviour
{
    //public bool isOpen;// { get; set; }
    public Vector3 Number;// { get; set; }
    public bool MyCard;
    public int ID { get; set; }
    GameObject thisgameobject;
    Renderer thisrenderer;
    TextMeshProUGUI text_x;
    TextMeshProUGUI text_y;
    TextMeshProUGUI text_z;

    Transform pos_center;
    Transform pos_x;
    Transform pos_y;
    Transform pos_z;
    MeshFilter meshfilter;
    public CardState Initialize(GameObject _thisgameobject, bool _myCard, int _ID)
    {
        MyCard = _myCard;
        thisrenderer = GetComponent<Renderer>();
        //isOpen = false;
        thisgameobject = _thisgameobject;
        text_x = thisgameobject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        text_y = thisgameobject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        text_z = thisgameobject.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        pos_center = this.transform.GetChild(1).transform;
        pos_x = this.transform.GetChild(1).GetChild(0).transform;
        pos_y = this.transform.GetChild(1).GetChild(1).transform;
        pos_z = this.transform.GetChild(1).GetChild(2).transform;
        meshfilter = this.transform.GetChild(1).GetComponent<MeshFilter>();
        this.transform.GetChild(1).GetComponent<Renderer>().material.renderQueue = 4000; // UIよりも高い値
        ID = _ID;
        Close();
        return this;
    }

    public void Open()
    {
        //isOpen = true;
        text_x.text = Number.x.ToString();
        text_y.text = Number.y.ToString();
        text_z.text = Number.z.ToString();
        thisrenderer.material.color = Color.white;
        SetMesh();
    }
    public void Close()
    {
        //isOpen = false;
        text_x.text = "";
        text_y.text = "";
        text_z.text = "";
        thisrenderer.material.color = Color.gray;
        FadeMesh();
    }

    /*public void Clicked()
    {
        thisrenderer.material.color = Color.yellow;
    }*/
    public void HostClicked()
    {
        thisrenderer.material.color = new Color(0f, 1.0f, 0.9f); //Color.yellow;
    }
    public void ClientClicked()
    {
        thisrenderer.material.color = new Color(0f, 1.0f, 0f); //Color.green;
    }
    public void Clicked_deep()
    {
        thisrenderer.material.color = new Color(0.0f, 0.6f, 0.5f); //new Color(0.8f, 0.8f, 0.0f, 1.0f);
    }
    public void UnClicked()
    {
        thisrenderer.material.color = Color.white;
    }
    void SetMesh()
    {

        // 頂点を定義
        Vector3[] vertices = {
            pos_center.InverseTransformPoint(VerticePos((int)Number.x, MyCard?5:10, pos_center.position, pos_x.position)),
            pos_center.InverseTransformPoint(VerticePos((int)Number.y, MyCard?5:10, pos_center.position, pos_y.position)),
            pos_center.InverseTransformPoint(VerticePos((int)Number.z, MyCard?5:10, pos_center.position, pos_z.position)),
            // 他の頂点を追加...
        };

        // 三角形を定義
        int[] triangles = {
            0, 2, 1
            // 他の三角形を追加...
        };

        meshfilter.mesh.Clear();
        meshfilter.mesh.vertices = vertices;
        meshfilter.mesh.triangles = triangles;
    }
    void FadeMesh()
    {
        meshfilter.mesh.Clear();
    }
    private Vector3 VerticePos(int i, int _numberofcards, Vector3 start, Vector3 end)
    {
        return Vector3.Lerp(start, end, (float)i / ((float)_numberofcards));
    }
}
