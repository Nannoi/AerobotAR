using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TextSpeech;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace YourNamespace
{
    public class Main : MonoBehaviour
    {
        private InputField text;

        public GameObject robotPrefab;
        public GameObject wayPointPrefab;
        public GameObject meshPrefab;
        public int numRobots;
        public GameObject gameObjectsContainer; // Reference to the empty GameObject
        private Text numData;


        void Start()
        {
            numRobots = 0; // Set an initial value
            // Instantiate objects based on the initial number of robots
            InstantiateObjects();

            GameObject canvasObject = GameObject.Find("Canvas");
            Transform speechResultTransform = canvasObject.transform.Find("SpeechResult");
            text = speechResultTransform.GetComponent<InputField>();
            // Add a listener to the onValueChanged event of the InputField component
            text.onValueChanged.AddListener(OnInputFieldValueChanged);
        
            // Add a listener to the Dropdown component to trigger when its value changes
            /* dropdown.onValueChanged.AddListener(OnDropdownValueChanged);*/
        }
        void OnInputFieldValueChanged(string result)
        {
             rNum(result);

        // Instantiate new objects with a delay to ensure the old ones are destroyed
       
        }

        void rNum (string result)
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            GameObject num = FindObjectByName(canvasObject.transform, "NumRobots");
            numData = num.GetComponent<Text>();
    

            string[] keywords = new string[]
            {
            "reset",
            "3 robots",
            "4 robots",
            "5 robots",
            "6 robots",
            "7 robots",
            "8 robots",
            "9 robots",
            "10 robots",
            "11 robots",
            "12 robots"
            };

            if (keywords.Any(keyword => result.Contains(keyword)))
            {
                foreach (Transform child in gameObjectsContainer.transform)
                {
                    Destroy(child.gameObject);
                }

                if (result.Contains("3 robots"))
                {
                    numRobots = 3;      
                }
                else if (result.Contains("4 robots"))
                {
                    numRobots = 4;
                 
                }
                else if (result.Contains("5 robots"))
                {
                    numRobots = 5;
                    
                }
                else if (result.Contains("6 robots"))
                {
                    numRobots = 6;
                    
                }
                else if (result.Contains("7 robots"))
                {
                    numRobots = 7;
                   
                }
                else if (result.Contains("8 robots"))
                {
                    numRobots = 8;
                    
                }
                else if (result.Contains("9 robots"))
                {
                    numRobots = 9;
                    
                }
                else if (result.Contains("10 robots"))
                {
                    numRobots = 10;
                   
                }
                else if (result.Contains("11 robots"))
                {
                    numRobots = 11;
                   
                }
                else if (result.Contains("12 robots"))
                {
                    numRobots = 12;
                    
                }
                StartCoroutine(InstantiateObjectsWithDelay());
                Debug.Log("Generate " + numRobots + " Robots");
            }
            numData.text = numRobots.ToString();
            
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
        IEnumerator InstantiateObjectsWithDelay()
{
 // Wait for 2 seconds
 yield return new WaitForSeconds(1.0f);

 // Now instantiate new objects
 InstantiateObjects();


}

void InstantiateObjects()
{
 Vector3 placedObjectLocation = FindObjectOfType<AutoPlacementOfObjectsInPlane>().PlacedObjectLocation;
 for (var i = 1; i <= numRobots; i++)
 {

     GameObject robot = Instantiate(robotPrefab,placedObjectLocation + new Vector3((i * 4.0f) - ((numRobots + 1) * 2f), 0, 8), Quaternion.identity);

     // Set the parent of the robot to the gameObjectsContainer
     robot.transform.SetParent(gameObjectsContainer.transform);
     // Optionally, you can rename the robot for easier identification
     robot.name = "Robot" + i.ToString();

     GameObject wayPoint = Instantiate(wayPointPrefab, Vector3.zero, Quaternion.identity);
     wayPoint.transform.SetParent(gameObjectsContainer.transform);
     wayPoint.name = "WayPoint" + i.ToString();

     GameObject fabric = Instantiate(meshPrefab, Vector3.zero, Quaternion.identity);
     // Set the parent of the robot to the gameObjectsContainer
     fabric.transform.SetParent(gameObjectsContainer.transform);
     // Optionally, you can rename the robot for easier identification
     fabric.name = "Fabric" + i.ToString();  
 }
 updateRobot();
}

private void updateRobot()
{
Vector3 placedObjectLocation = FindObjectOfType<AutoPlacementOfObjectsInPlane>().PlacedObjectLocation;
 //numRobots = dropdown.value;
 for (var i = 1; i <= numRobots; i++)
 {
     string robotId = "Robot" + i.ToString();
     string NumId = i.ToString();
     string waypointId = "WayPoint" + i.ToString();
     string fabricId = "Fabric" + i.ToString();

     GameObject robot = GameObject.Find("Robot" + i.ToString());
     List<GameObject> neighbours = GetNeighbours(i);

     RobotBehaviour robotBehaviour = robot.AddComponent<RobotBehaviour>();
     robotBehaviour.Initialize(NumId, placedObjectLocation, waypointId, numRobots, neighbours);
     robotBehaviour.Evaluate();

     GameObject fabric = GameObject.Find("Fabric" + i.ToString());
     GameObject Fneighbour = fabricNeighbour(i);

     MainFabric mainmesh = fabric.AddComponent<MainFabric>();
     DrawMesh drawmesh = fabric.AddComponent<DrawMesh>();
     mainmesh.Initialize(robotId, fabricId, numRobots, Fneighbour);
 }
}
private List<GameObject> GetNeighbours(int robotIndex)
{
 List<GameObject> neighbours = new List<GameObject>();

 for (int i = 1; i <= numRobots; i++)
 {
     if (i != robotIndex)
     {
         GameObject neighbour = GameObject.Find("Robot" + i.ToString());
         if (neighbour != null)
         {
             neighbours.Add(neighbour);
         }
     }

 }
 return neighbours;
}

private GameObject fabricNeighbour(int robotIndex)
{
 GameObject Fneighbour = null;

 for ( robotIndex = 1; robotIndex <= numRobots; robotIndex++)
 {

         if (robotIndex == numRobots)
         {
             Fneighbour = GameObject.Find("Robot1");

         }
         else
         {
             int m = robotIndex + 1;
             Fneighbour = GameObject.Find("Robot" + m.ToString());

         }

 }
 return Fneighbour;
}

}
}

