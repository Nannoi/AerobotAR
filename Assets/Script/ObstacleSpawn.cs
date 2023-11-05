using System.Collections;
using System.Collections.Generic;
using TextSpeech;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace YourNamespace
{
    public class ObstacleSpawn : MonoBehaviour
    {

        public GameObject obstaclePrefab;
        private Toggle obstacleTrigger;
        public GameObject gameObjectsContainer; // Reference to the empty GameObject


        void Start()
        {
            obstacleTrigger=GetComponent<Toggle>();
            obstacleTrigger.onValueChanged.AddListener(OnDropdownValueChanged);

        }
        void OnDropdownValueChanged(bool value)
        {
            if (value == true)
            { 
            InstantiateObjects();
            Debug.Log("copy");
            }
            else
            {
                // Handle obstacle deletion here
                foreach (Transform child in gameObjectsContainer.transform)
                {
                    Destroy(child.gameObject); // Assuming child objects are the obstacles
                }
            }

        }

        void InstantiateObjects()
        {
            Vector3 placedObjectLocation = FindObjectOfType<AutoPlacementOfObjectsInPlane>().PlacedObjectLocation;
            GameObject Obstscle = Instantiate(obstaclePrefab, placedObjectLocation + new Vector3(0, 0, 6), Quaternion.identity);
            Obstscle.transform.SetParent(gameObjectsContainer.transform);
 
        }

      
    }
}
