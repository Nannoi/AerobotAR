using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using System.Linq;
using System;
using System.Text;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;

namespace YourNamespace
{
    public class Formation : MonoBehaviour
    {
        private Text uiText;
        private Text previousText;

        private Text shapeData;
        private Text areaData;
        private Text lengthData;
        public string robotId;
        Vector3 centerLocation;
        public int numIterations;
        public List<GameObject> neighbours;
        private string waypointId;
        private bool isCompleted;
        private float radius = 4.0f;
        private float radiusS;
        private float sideLength = 8.0f;
        private float sideTLength = 10.0f;
        private float scaleFactor = 1f;
        private Vector2 lineStart = new Vector2(-10.0f, 0f);
        private Vector2 lineEnd = new Vector2(10.0f, 0f);
        public double area;
        private Action currentShape;

        private LineFormation lineFormationProcess;
        private CircleFormation circleFormationProcess;
        private TriangleFormation triangleFormationProcess;
        private SquareFormation squareFormationProcess;
        private FlowerFormation flowerFormationProcess;
        private SeaFormation seaFormationProcess;
        private CaveFormation caveFormationProcess;
        private RobotControllerA robotControllerAProcess;
        private RobotControllerB robotControllerBProcess;
        private RobotControllerC robotControllerCProcess;
        private InputField text;
        private GameObject popup;





        public void Initialize(string robotId, Vector3 centerLocation, string waypointId, int numIterations, List<GameObject> neighbours)
        {
            this.robotId = robotId;
            this.centerLocation = centerLocation;
            this.waypointId = waypointId;
            this.numIterations = numIterations;
            this.neighbours = neighbours;

        }

        private void Start()
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            Transform speechResultTransform = canvasObject.transform.Find("SpeechResult");
            text = speechResultTransform.GetComponent<InputField>();
            // Add a listener to the onValueChanged event of the InputField component
            text.onValueChanged.AddListener(OnInputFieldValueChanged);

            GameObject shapeG = FindObjectByName(canvasObject.transform, "ShapeText");
            shapeData = shapeG.GetComponent<Text>();

            GameObject areaG = FindObjectByName(canvasObject.transform, "AreaText");
            areaData = areaG.GetComponent<Text>();

            GameObject lengthG = FindObjectByName(canvasObject.transform, "RadiusText");
            lengthData = lengthG.GetComponent<Text>();


        }

        // The method that will be executed when the InputField's value changes
        private void OnInputFieldValueChanged(string result)
        {
            FormationSpeech(result);
            SizeSpeech(result);

            if (currentShape != null)
            {
                shapeData.text = currentShape.Method.Name; // Convert the Action to a string
                areaData.text = area.ToString()+" sqm";
                lengthData.text = radiusS.ToString()+" m";
            }
            
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

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public IEnumerator Execute()
        {
            // Perform the formation logic here
            Debug.Log($"Formation executing for robot {robotId}");

            // Simulate the formation process by waiting for a few seconds
            yield return new WaitForSeconds(2.0f);

            // Set the completion flag
            isCompleted = true;

            // Send the formation completion message
            SendDone("FORMATION_COMPLETED");
        }

        void FormationSpeech(string result)
        {
            string[] formKeys = new string[]
            {
            "line",
            "circle",
            "triangle",
            "Square",
            "flower",
            "water",
            "cave",
            };

            if (formKeys.Any(formKeys => result.Contains(formKeys)))
            {
                if (result.Contains("line"))
                {
                    Line();
                    currentShape = Line;
                }
                else if (result.Contains("circle"))
                {
                    Circle();
                    currentShape = Circle;
                }
                else if (result.Contains("triangle"))
                {
                    Triangle();
                    currentShape = Triangle;
                }
                else if (result.Contains("Square"))
                {
                    Square();
                    currentShape = Square;
                }

                else if (result.Contains("flower"))
                {
                    Flower();
                    currentShape = Flower;
                }
                else if (result.Contains("water"))
                {
                    Sea();
                    currentShape = Sea;
                }
                else if (result.Contains("cave"))
                {
                    Cave();
                    currentShape = Cave;
                }
                Debug.Log("Start: " + currentShape.Method.Name);
            }
        }
        void SizeSpeech(string result)
        {
            string[] sizeKeys = new string[]
           {
            "line",
            "circle",
            "triangle",
            "Square",
            "flower",
            "water",
            "cave",
            "smaller",
            "larger",
            "area 20",
            "area 25",
            "area 30",
            "area 35",
            "area 40",
            "area 45",
            "area 50",
            "area 55",
            "area 65",
            "area 70",
            "area 75",
            "area 80",
            "area 85",
            "area 90",
            "area 95",
            "area 100"
           };

            if (sizeKeys.Any(sizeKeys => result.Contains(sizeKeys)))
            {
                if (result.Contains("smaller"))
                {
                    ScaleDown();
                }
                else if (result.Contains("larger"))
                {
                    ScaleUp();
                }

                else if (result.Contains("area 20"))
                {
                    TwentySqm();
                }

                else if (result.Contains("area 25"))
                {
                    TwentyfiveSqm();
                }

                else if (result.Contains("area 30"))
                {
                    ThirtySqm();
                }

                else if (result.Contains("area 35"))
                {
                    ThirtyfiveSqm();
                }

                else if (result.Contains("area 40"))
                {
                    FortySqm();
                }

                else if (result.Contains("area 45"))
                {
                    FortyfiveSqm();
                }

                else if (result.Contains("area 50"))
                {
                    FiftySqm();
                }
                else if (result.Contains("area 55"))
                {
                    FiftyfiveSqm();
                }
                else if (result.Contains("area 60"))
                {
                    SixtySqm();
                }

                else if (result.Contains("area 65"))
                {
                    SixtyfiveSqm();
                }

                else if (result.Contains("area 70"))
                {
                    SeventySqm();
                }
                else if (result.Contains("area 75"))
                {
                    SeventyfiveSqm();
                }

                else if (result.Contains("area 80"))
                {
                    EightySqm();
                }

                else if (result.Contains("area 85"))
                {
                    EightyfiveSqm();
                }

                else if (result.Contains("area 90"))
                {
                    NintySqm();
                }
                else if (result.Contains("area 95"))
                {
                    NintyfiveSqm();
                }
                else if (result.Contains("area 100"))
                {
                    HundredSqm();
                }
                CalculateArea();
                Debug.Log("Area: " + area);
            }

        }

        private void Line()
        {
            Vector2 lineStartF = new Vector3(-numIterations * 1.5f,0,0) * scaleFactor;
            Vector2 lineEndF = new Vector3(numIterations * 1.5f, 0, 0) * scaleFactor;
            int FrobotId = 1;
            radiusS = lineStartF.x+lineEndF.x;
                
                string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (robot != null)
                {

                lineFormationProcess = robot.GetComponent<LineFormation>();
                circleFormationProcess = robot.GetComponent<CircleFormation>();
                squareFormationProcess = robot.GetComponent<SquareFormation>();
                triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                seaFormationProcess = robot.GetComponent<SeaFormation>();
                caveFormationProcess = robot.GetComponent<CaveFormation>();
                robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                    triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                    || caveFormationProcess != null)
                {
                    Destroy(lineFormationProcess);
                    Destroy(circleFormationProcess);
                    Destroy(squareFormationProcess);
                    Destroy(triangleFormationProcess);
                    Destroy(flowerFormationProcess);
                    Destroy(seaFormationProcess);
                    Destroy(caveFormationProcess);
                    Destroy(robotControllerAProcess);
                    //Destroy(robotControllerBProcess);
                    Destroy(robotControllerCProcess);
                }

                // Add the CircleFormation component to the robot GameObject
                lineFormationProcess = robot.AddComponent<LineFormation>();
                lineFormationProcess.Initialize(robotId,FrobotId,centerLocation, lineStartF, lineEndF, numIterations, neighbours);
                if (robotControllerBProcess != null)
                {
                    RobotControllerB robotControllerB = robot.GetComponent<RobotControllerB>();
                    robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                }
                else
                {
                    RobotControllerB robotControllerB = robot.AddComponent<RobotControllerB>();
                    robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                }

            }
                else
                {
                    Debug.Log("Robot object is null!");
                }
            
        }


        private void Circle()
        {
            float radiusF = radius * scaleFactor;
            radiusS = radiusF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations >= 5) 
            { 

            if (robot != null)
            {

                lineFormationProcess = robot.GetComponent<LineFormation>();
                circleFormationProcess = robot.GetComponent<CircleFormation>();
                squareFormationProcess = robot.GetComponent<SquareFormation>();
                triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                seaFormationProcess = robot.GetComponent<SeaFormation>();
                caveFormationProcess = robot.GetComponent<CaveFormation>();
                robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                    triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                    || caveFormationProcess != null)
                {
                    Destroy(lineFormationProcess);
                    Destroy(circleFormationProcess);
                    Destroy(squareFormationProcess);
                    Destroy(triangleFormationProcess);
                    Destroy(flowerFormationProcess);
                    Destroy(seaFormationProcess);
                    Destroy(caveFormationProcess);
                    //Destroy(robotControllerAProcess);
                    Destroy(robotControllerBProcess);
                    Destroy(robotControllerCProcess);
                }

                circleFormationProcess = robot.AddComponent<CircleFormation>();
                circleFormationProcess.Initialize(robotId, centerLocation, radiusF, numIterations, neighbours);
                if (robotControllerAProcess != null)
                {
                    RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                    robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                }
                else
                {
                    RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                    robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                }

            }
            else
            {
                Debug.Log("Robot object is null!");
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
    Debug.Log("Number of Robots is not enough!");}

        }

        private void Square()
        {
            //Square formation
            float halfLength = (sideLength / 2) * scaleFactor;
            radiusS = sideLength*scaleFactor;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (numIterations % 4 != 1 && numIterations % 4 != 2 && numIterations % 4 != 3)
            {
                if (robot != null)
                {

                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    // Add the CircleFormation component to the robot GameObject
                    squareFormationProcess = robot.AddComponent<SquareFormation>();
                    squareFormationProcess.Initialize(robotId, centerLocation, halfLength, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }

                }
                else
                {
                    Debug.LogError("Robot object is null!");
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
                Debug.Log("Number of Robots is not enough!");
            }

        }

        private void Triangle()
        {
            float sideTLengthF = sideTLength * scaleFactor;
            //Triangle formation
            radiusS= sideTLengthF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations % 3 != 1 && numIterations % 3 != 2)
            {
                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null || 
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    // Add the TriangleFormation component to the robot GameObject
                    triangleFormationProcess = robot.AddComponent<TriangleFormation>();
                    triangleFormationProcess.Initialize(robotId, 1, centerLocation, sideTLengthF, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                }
                else
                {
                    Debug.Log("Robot object is null!");
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
                Debug.Log("Number of Robots is not enough!");
            }
        }

        private void DrawShape()
        {

            if (currentShape == Circle)
            {
                Circle();
            }
            else if (currentShape == Triangle)
            {
                Triangle();
            }
            else if (currentShape == Square)
            {
                Square();
            }
            else if (currentShape == Line)
            {
                Line();
            }
            else if (currentShape == Sea)
            {
                Sea();
            }
            else if (currentShape == Flower)
            {
                Flower();
            }
            else if (currentShape == Cave)
            {
                Cave();
            }
        }

        private void CalculateArea()
        {

            if (currentShape == Circle)
            {
                CalculateCircleArea();
            }
            else if (currentShape == Triangle)
            {
                CalculateTriangleArea();
            }
            else if (currentShape == Square)
            {
                CalculateSquareArea();
            }
            else if (currentShape == Line)
            {
                CalculateLineLength();
            }
            else if (currentShape == Sea)
            {
                CalculateSeaLength();
            }
            else if (currentShape == Flower)
            {
                CalculateFlowerArea();
            }
            else if (currentShape == Cave)
            {
                CalculateCaveArea();
            }

        }

        private double CalculateCircleArea()
        {
            area= Math.Round(Mathf.PI * (radius * radius * scaleFactor * scaleFactor),2);
            return area;
        }

        private double CalculateTriangleArea()
        {
            area = Math.Round(0.5f * sideTLength * sideTLength * scaleFactor * scaleFactor,2);
            return area;
        }

        private double CalculateSquareArea()
        {
            area = Math.Round(sideLength * sideLength * scaleFactor * scaleFactor,2);
            return area;
        }

        private double CalculateLineLength()
        {
            area = Math.Round(((-lineStart.x * scaleFactor) + (lineEnd.x * scaleFactor)),2);
            return area;
        }

        private double CalculateSeaLength()
        {
            area = Math.Round(((-lineStart.x * scaleFactor) + (lineEnd.x * scaleFactor))*(numIterations-3)/numIterations, 2);
            return area;
        }

        private double CalculateFlowerArea()
        {
            area = Math.Round(Mathf.PI * (radius * radius * scaleFactor * scaleFactor), 2);
            return area;
        }

        private double CalculateCaveArea()
        {
            area = Math.Round((-lineStart.x * scaleFactor * 2.6), 2);
            return area;
        }

        private void ScaleUp()
        {
            scaleFactor *= 2f;
            DrawShape();
            Debug.Log("scale up: "+ scaleFactor);
        }

        private void ScaleDown()
        {
            scaleFactor /= 2f;
            DrawShape();
            Debug.Log("scale down: "+ scaleFactor);
        }


        private void TwentySqm()
        {
            radius = Mathf.Sqrt(20f / Mathf.PI);
            sideLength = Mathf.Sqrt(20f);
            sideTLength = Mathf.Sqrt(2 * 20f);
            lineStart = new Vector3(-20f / 2, 0f);
            lineEnd = new Vector3(20f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void TwentyfiveSqm()
        {
            radius = Mathf.Sqrt(25f / Mathf.PI);
            sideLength = Mathf.Sqrt(25f);
            sideTLength = Mathf.Sqrt(2 * 25f);
            lineStart = new Vector3(-25f / 2, 0f);
            lineEnd = new Vector3(25f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void ThirtySqm()
        {
            radius = Mathf.Sqrt(30f / Mathf.PI);
            sideLength = Mathf.Sqrt(30f);
            sideTLength = Mathf.Sqrt(2 * 30f);
            lineStart = new Vector3(-30f / 2, 0f);
            lineEnd = new Vector3(30f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }


        private void ThirtyfiveSqm()
        {
            radius = Mathf.Sqrt(35f / Mathf.PI);
            sideLength = Mathf.Sqrt(35f);
            sideTLength = Mathf.Sqrt(2 * 35f);
            lineStart = new Vector3(-35f / 2, 0f);
            lineEnd = new Vector3(35f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void FortySqm()
        {
            radius = Mathf.Sqrt(40f / Mathf.PI);
            sideLength = Mathf.Sqrt(40f);
            sideTLength = Mathf.Sqrt(2 * 40f);
            lineStart = new Vector3(-40f / 2, 0f);
            lineEnd = new Vector3(40f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void FortyfiveSqm()
        {
            radius = Mathf.Sqrt(45f / Mathf.PI);
            sideLength = Mathf.Sqrt(45f);
            sideTLength = Mathf.Sqrt(2 * 45f);
            lineStart = new Vector3(-45f / 2, 0f);
            lineEnd = new Vector3(45f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void FiftySqm()
        {
            radius = Mathf.Sqrt(50f / Mathf.PI);
            sideLength = Mathf.Sqrt(50f);
            sideTLength = Mathf.Sqrt(2 * 50f);
            lineStart = new Vector3(-50f / 2, 0f);
            lineEnd = new Vector3(50f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void FiftyfiveSqm()
        {
            radius = Mathf.Sqrt(55f / Mathf.PI);
            sideLength = Mathf.Sqrt(55f);
            sideTLength = Mathf.Sqrt(2 * 55f);
            lineStart = new Vector3(-55f / 2, 0f);
            lineEnd = new Vector3(55f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SixtySqm()
        {
            radius = Mathf.Sqrt(60f / Mathf.PI);
            sideLength = Mathf.Sqrt(60f);
            sideTLength = Mathf.Sqrt(2 * 60f);
            lineStart = new Vector3(-60f / 2, 0f);
            lineEnd = new Vector3(60f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SixtyfiveSqm()
        {
            radius = Mathf.Sqrt(65f / Mathf.PI);
            sideLength = Mathf.Sqrt(65f);
            sideTLength = Mathf.Sqrt(2 * 65f);
            lineStart = new Vector3(-65f / 2, 0f);
            lineEnd = new Vector3(65f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SeventySqm()
        {
            radius = Mathf.Sqrt(70f / Mathf.PI);
            sideLength = Mathf.Sqrt(70f);
            sideTLength = Mathf.Sqrt(2 * 70f);
            lineStart = new Vector3(-70f / 2, 0f);
            lineEnd = new Vector3(70f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }
        private void SeventyfiveSqm()
        {
            radius = Mathf.Sqrt(75f / Mathf.PI);
            sideLength = Mathf.Sqrt(75f);
            sideTLength = Mathf.Sqrt(2 * 75f);
            lineStart = new Vector3(-75f / 2, 0f);
            lineEnd = new Vector3(75f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void EightySqm()
        {
            radius = Mathf.Sqrt(80f / Mathf.PI);
            sideLength = Mathf.Sqrt(80f);
            sideTLength = Mathf.Sqrt(2 * 80f);
            lineStart = new Vector3(-80f / 2, 0f);
            lineEnd = new Vector3(80f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void EightyfiveSqm()
        {
            radius = Mathf.Sqrt(85f / Mathf.PI);
            sideLength = Mathf.Sqrt(85f);
            sideTLength = Mathf.Sqrt(2 * 85f);
            lineStart = new Vector3(-85f / 2, 0f);
            lineEnd = new Vector3(85f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void NintySqm()
        {
            radius = Mathf.Sqrt(90f / Mathf.PI);
            sideLength = Mathf.Sqrt(90f);
            sideTLength = Mathf.Sqrt(2 * 90f);
            lineStart = new Vector3(-90f / 2, 0f);
            lineEnd = new Vector3(90f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void NintyfiveSqm()
        {
            radius = Mathf.Sqrt(95f / Mathf.PI);
            sideLength = Mathf.Sqrt(95f);
            sideTLength = Mathf.Sqrt(2 * 95f);
            lineStart = new Vector3(-95f / 2, 0f);
            lineEnd = new Vector3(95f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }

        private void HundredSqm()
        {
            radius = Mathf.Sqrt(100f / Mathf.PI);
            sideLength = Mathf.Sqrt(100f);
            sideTLength = Mathf.Sqrt(2 * 100f);
            lineStart = new Vector3(-100f / 2, 0f);
            lineEnd = new Vector3(100f / 2, 0f);
            scaleFactor = 1f;
            DrawShape();
        }


        private void Sea()
        {
            currentShape = Sea;
            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);
            if (numIterations >= 6)
            {
                Vector3 StartF = new Vector3(-numIterations*1.5f, 0f, 0f);
                Vector3 EndF = new Vector3(numIterations*1.5f, 0f, 0f);

                Vector2 lineStartF = StartF * scaleFactor;
                Vector2 lineEndF = EndF * scaleFactor;

                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        Destroy(robotControllerAProcess);
                        //Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }
                    seaFormationProcess = robot.AddComponent<SeaFormation>();


                    //circleFormationProcess.enabled = true;
                    seaFormationProcess.Initialize(robotId, centerLocation, lineStartF, lineEndF, numIterations, neighbours);
                    if (robotControllerBProcess != null)
                    {
                        RobotControllerB robotControllerB = robot.GetComponent<RobotControllerB>();
                        robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerB robotControllerB = robot.AddComponent<RobotControllerB>();
                        robotControllerB.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }

                }
                else
                {
                    Debug.Log("Robot object is null!");
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

        private void Flower()
        {
            float radiusF = radius * scaleFactor;
            radiusS = radiusF;
            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (numIterations >= 6)
            {

                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        //Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        Destroy(robotControllerCProcess);
                    }

                    flowerFormationProcess = robot.AddComponent<FlowerFormation>();
                    flowerFormationProcess.Initialize(robotId, centerLocation, radiusF, numIterations, neighbours);
                    if (robotControllerAProcess != null)
                    {
                        RobotControllerA robotControllerA = robot.GetComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerA robotControllerA = robot.AddComponent<RobotControllerA>();
                        robotControllerA.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                }

                else
                {
                    Debug.Log("Robot object is null!");
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

        private void Cave()
        {
            float radiusF = radius * scaleFactor;
            radiusS = radiusF;

            string robotname = "Robot" + robotId.ToString();
            GameObject robot = GameObject.Find(robotname);

            if (numIterations >= 6 && numIterations % 2 !=1)
            {
                Vector3 StartF = new Vector3(0f, 0f, -numIterations * 1.5f);
                Vector3 EndF = new Vector3(0f, 0f, numIterations * 1.5f);
                
                Vector3 lineStartF = StartF * scaleFactor;
                Vector3 lineEndF = EndF * scaleFactor;
                if (robot != null)
                {
                    lineFormationProcess = robot.GetComponent<LineFormation>();
                    circleFormationProcess = robot.GetComponent<CircleFormation>();
                    squareFormationProcess = robot.GetComponent<SquareFormation>();
                    triangleFormationProcess = robot.GetComponent<TriangleFormation>();
                    flowerFormationProcess = robot.GetComponent<FlowerFormation>();
                    seaFormationProcess = robot.GetComponent<SeaFormation>();
                    caveFormationProcess = robot.GetComponent<CaveFormation>();
                    robotControllerAProcess = robot.GetComponent<RobotControllerA>();
                    robotControllerBProcess = robot.GetComponent<RobotControllerB>();
                    robotControllerCProcess = robot.GetComponent<RobotControllerC>();

                    if (lineFormationProcess != null || circleFormationProcess != null || squareFormationProcess != null ||
                        triangleFormationProcess != null || flowerFormationProcess != null || seaFormationProcess != null
                        || caveFormationProcess != null)
                    {
                        Destroy(lineFormationProcess);
                        Destroy(circleFormationProcess);
                        Destroy(squareFormationProcess);
                        Destroy(triangleFormationProcess);
                        Destroy(flowerFormationProcess);
                        Destroy(seaFormationProcess);
                        Destroy(caveFormationProcess);
                        Destroy(robotControllerAProcess);
                        Destroy(robotControllerBProcess);
                        //Destroy(robotControllerCProcess);
                    }

                    caveFormationProcess = robot.AddComponent<CaveFormation>();
                    caveFormationProcess.Initialize(robotId, centerLocation, lineStartF, lineEndF, numIterations, neighbours);
                    if (robotControllerCProcess != null)
                    {
                        RobotControllerC robotControllerC = robot.GetComponent<RobotControllerC>();
                        robotControllerC.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                    else
                    {
                        RobotControllerC robotControllerC = robot.AddComponent<RobotControllerC>();
                        robotControllerC.Initialize(robotId, centerLocation, waypointId, numIterations, neighbours);
                    }
                }

                else
                {
                    Debug.Log("Robot object is null!");
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


        private void SendDone(string message)
        {
            // Send the message using your Unity-specific communication method
            // Replace the code below with your actual communication method
            Debug.Log($"Message Sent: {message}");
        }

    }

    [System.Serializable]
    public class PositionData
    {
        public string senderId;
        public double x;
        public double y;

        public PositionData(string senderId, double x, double y)
        {
            this.senderId = senderId;
            this.x = x;
            this.y = y;
        }
    }
}

