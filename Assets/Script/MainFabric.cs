using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.AI;


public class MainFabric : MonoBehaviour
{
    //private Text uiText;

    public GameObject[] loc;
    private MeshFilter meshFilter;
    private DrawMesh drawmesh;

    private string robotId;
    private string fabricId;
    private List<GameObject> Fneighbour = new List<GameObject>();
    private List<GameObject> locs = new List<GameObject>();
    private int numRobots;
    private GameObject neighbour;
    private List<GameObject> initialLocs = new List<GameObject>();
    private InputField text;
    private Text fabricData;
    private GameObject popup;

    // Start is called before the first frame update
    public void Initialize(string robotId, string fabricId,int numRobots,GameObject neighbour)
    {
        this.robotId = robotId;
        this.fabricId = fabricId;
        // this.locs = locs;
        this.numRobots = numRobots;
        this.neighbour = neighbour;

    }

    void Start()
    {
        loc = new GameObject[10];
        locs = Getlocs();
        loc = locs.ToArray();
        initialLocs.AddRange(locs);

        GameObject canvasObject = GameObject.Find("Canvas");
        Transform speechResultTransform = canvasObject.transform.Find("SpeechResult");
        text = speechResultTransform.GetComponent<InputField>();
        // Add a listener to the onValueChanged event of the InputField component
        text.onValueChanged.AddListener(OnInputFieldValueChanged);

        meshFilter = GetComponent<MeshFilter>();
        drawmesh = GetComponent<DrawMesh>();
        drawmesh.Initialize(robotId, numRobots, neighbour, loc);

        locs.Clear();

        GameObject fcon = FindObjectByName(canvasObject.transform, "Connection");
        fabricData = fcon.GetComponent<Text>();
    }

    void OnInputFieldValueChanged(string result)
    {
        MeshCreation(result);
    }

    void MeshCreation(string result)
    {
        string[] sizeKeys = new string[]
           {
            "system a",
            "system b",
            "system c",
            "disconnect"
           };

        if (sizeKeys.Any(sizeKeys => result.Contains(sizeKeys)))
        {
            if (result.Contains("system a"))
            {
                fabricData.text = "System A";
                Connect();
            }
            else if (result.Contains("system b"))
            {
                fabricData.text = "System B";
                Route();
            }
            else if (result.Contains("system c"))
            {
                fabricData.text = "System C";
                Form();
            }
            else if (result.Contains("disconnect"))
            {
                fabricData.text = "None";
                Disconnect();
            }
            Debug.Log("Fabric Action: "+fabricData.text);
        }
       
    }

    private void Connect()
    {
        for (int i = 1; i <= numRobots; i++)
        {
            locs.Clear();
            Fneighbour = new List<GameObject>();
            Fneighbour = fabricNeighbour();
            neighbour = Fneighbour[i - 1];
            GameObject robot = GameObject.Find("Robot" + i.ToString());
            robotId = robot.name;
            GameObject fabric = GameObject.Find("Fabric" + i.ToString());
            locs = Getlocs();
            loc = new GameObject[10];
            loc = locs.ToArray();
            DrawMesh drawMesh = fabric.GetComponent<DrawMesh>();

            if (drawMesh != null)
            {
                drawMesh.Initialize(robotId, numRobots, neighbour, loc);
                drawMesh.CreateFabricMesh();
            }
        }
    }

    private void Form()
    {
        for (int i = 1; i <= numRobots; i++)
        {
            locs.Clear();
            Fneighbour = new List<GameObject>();
            Fneighbour = fabricNeighbourC();
            neighbour = Fneighbour[i - 1];
            GameObject robot = GameObject.Find("Robot" + i.ToString());
            robotId= robot.name;
            GameObject fabric = GameObject.Find("Fabric" + i.ToString());
            locs = Getlocs();
            loc = new GameObject[10];
            loc = locs.ToArray();
            DrawMesh drawMesh = fabric.GetComponent<DrawMesh>();
     
            if (drawMesh != null)
            {

                drawMesh.Initialize(robotId, numRobots, neighbour, loc);
                drawMesh.CreateFabricMesh();
            }
        }
        
    }

    private void Route()
    {
        if (numRobots % 2 != 1)
        {
            for (int i = 1; i <= numRobots; i++)
            {
                locs.Clear();
                Fneighbour = new List<GameObject>();
                Fneighbour = fabricNeighbourB();
                neighbour = Fneighbour[i - 1];
                GameObject robot = GameObject.Find("Robot" + i.ToString());
                robotId = robot.name;
                GameObject fabric = GameObject.Find("Fabric" + i.ToString());
                locs = Getlocs();
                loc = new GameObject[10];
                loc = locs.ToArray();
                DrawMesh drawMesh = fabric.GetComponent<DrawMesh>();

                if (drawMesh != null)
                {

                    drawMesh.Initialize(robotId, numRobots, neighbour, loc);
                    drawMesh.CreateFabricMesh();
                }
            }
        }
        else
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            popup = FindObjectByName(canvasObject.transform, "Error Panel");
            if (popup != null)
            {
                popup.SetActive(true);
            }
            Debug.Log("Robot numbers are not enough!");
        }

    }


    private void Disconnect()
    {
        for (int i = 1; i <= numRobots; i++)
        {
            GameObject fabric = GameObject.Find("Fabric" + i.ToString());
            DrawMesh drawMesh = fabric.GetComponent<DrawMesh>();
            if (drawMesh != null)
            {
                Debug.Log("Disabling DrawMesh for Fabric" + i.ToString());
                drawMesh.ResetFabricMesh();
            }

        }
    }
    private List<GameObject> Getlocs()
    {
            for (int j = 0; j < 10; j++)
            {
            GameObject me = GameObject.Find(robotId);
            //List<GameObject> locs = new List<GameObject>();
            string meVer = "Vertice" + j.ToString();

            GameObject childMeVertice = FindObjectByName(me.transform, meVer);

            string neiVer = "Vertice" + (j+1).ToString();

            GameObject childNeiVertice = FindObjectByName(neighbour.transform, neiVer);

                if (j % 2 == 0)
                {
                    locs.Add(childMeVertice);
                    locs.Add(childNeiVertice);
                }
            }
        return locs;
    }

    public GameObject FindObjectByName(Transform parent, string objectName)
    {
        if (parent.name == objectName)
        {
            return parent.gameObject;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            GameObject foundObject = FindObjectByName(child, objectName);

            if (foundObject != null)
            {
                return foundObject;
            }
        }

        return null;
    }
    public List<GameObject> fabricNeighbourB()
    {

        for (var p = 1; p <= numRobots; p++)
        {
            GameObject neighbour;
            if (numRobots % 2 != 1)
            {
                if (p == numRobots/2)
                {
                    neighbour = GameObject.Find("Robot"+ (numRobots / 2).ToString());
                    Fneighbour.Add(neighbour);
                }
                else if (p == numRobots)
                {
                    neighbour = GameObject.Find("Robot"+ numRobots.ToString());
                    Fneighbour.Add(neighbour);
                }

                else
                {
                    int m = p + 1;
                    neighbour = GameObject.Find("Robot" + m.ToString());
                    Fneighbour.Add(neighbour);

                }
            }
            
        }

        return Fneighbour;
    }

    public List<GameObject> fabricNeighbourC()
    {

        for (var p = 1; p <= numRobots; p++)
        {
            GameObject neighbour;

            if (p == 3)
            {
                neighbour = GameObject.Find("Robot1");
                Fneighbour.Add(neighbour);
            }
            else if (p == numRobots)
            {
                neighbour = GameObject.Find("Robot4");
                Fneighbour.Add(neighbour);
            }

            else
            {
                int m = p + 1;
                neighbour = GameObject.Find("Robot" + m.ToString());
                Fneighbour.Add(neighbour);

            }
        }

        return Fneighbour;
    }

    private List<GameObject> fabricNeighbour()
    {

        for (var p = 1; p <= numRobots; p++)
        {
            GameObject neighbour;

            if (p == numRobots)
            {
                neighbour = GameObject.Find("Robot1");
                Fneighbour.Add(neighbour);
            }
            else
            {
                int m = p + 1;
                neighbour = GameObject.Find("Robot" + m.ToString());
                Fneighbour.Add(neighbour);

            }
        }

        return Fneighbour;
    }

}