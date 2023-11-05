using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class AutoPlacementOfObjectsInPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private GameObject placedPrefab;

    private GameObject placedObject;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    void Awake() 
    {
        dismissButton.onClick.AddListener(Dismiss);
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;
    }

    public void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if(args.added != null && placedObject == null)
        {
            ARPlane arPlane = args.added[0];
            placedObject = Instantiate(placedPrefab, arPlane.transform.position + new Vector3(0, 0, 10), Quaternion.identity);
            Debug.Log("AR Plane is detected!");
        }

    }

    public Vector3 PlacedObjectLocation
    {
        get
        {
            if (placedObject != null)
            {
                return placedObject.transform.position;
            }
            else
            {
                return Vector3.zero; // Return a default value if the placedObject is null
            }
        }
    }


    private void Dismiss() => welcomePanel.SetActive(false);
}
