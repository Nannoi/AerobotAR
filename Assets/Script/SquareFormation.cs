using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNamespace
{
    public class SquareFormation : MonoBehaviour
    {
        public string robotId;
        public Vector3 centerLocation;
        public double halfLength;
        public int numIterations;
        public List<GameObject> neighbours;

        private double radius;
        private int totalRobots;
        private int robotIndex;
        private float targetAngle;
        private double posX;
        private double posY;
        private float angle = 180;
        private int numberOfRays = 36;

        private string json;
        private bool isCompleted;

        public bool IsCompleted
        {
            get { return isCompleted; }
        }
        public void Initialize(string robotId, Vector3 centerLocation, double halfLength, int numIterations, List<GameObject> neighbours)
        {
            this.robotId = robotId;
            this.centerLocation = centerLocation;
            this.halfLength = halfLength;
            this.numIterations = numIterations;
            this.neighbours = neighbours;
        }

        public void Start()
        {
            radius = halfLength * Mathf.Sqrt(2.0f);
            totalRobots = neighbours.Count + 1;
            // robotIndex = (robotId.GetHashCode() % totalRobots);
            robotIndex = int.Parse(robotId);
            targetAngle = robotIndex * (2 * Mathf.PI / totalRobots);
            float cornerAngle = Mathf.PI / 4;

            // Initialize position
            posX = centerLocation.x + radius * Mathf.Cos(targetAngle+ cornerAngle);
            posY = centerLocation.z + radius * Mathf.Sin(targetAngle+ cornerAngle);

            Clamp(halfLength);

            StartCoroutine(Execute());
            StartCoroutine(RunCircleFormation());

        }

        private void FixedUpdate()
        {
            AvoidObstacle();
        }

        public System.Collections.IEnumerator Execute()
        {
            // Perform the square formation logic here
            Debug.Log($"CircleFormation executing for robot {robotId}");

            // Simulate the formation process by waiting for a few seconds
            yield return new WaitForSeconds(2.0f);

            // Set the completion flag
            isCompleted = true;

            // Send the square formation completion message
            Debug.Log("SQUARE_FORMATION_COMPLETED");
        }
        private System.Collections.IEnumerator RunCircleFormation()
        {
            // Move to the initial position
            yield return StartCoroutine(MoveToPosition(posX, posY));

            // Share position with neighbors
            foreach (GameObject neighbour in neighbours)
            {
                SendMessageToNeighbour("POSITION", robotId, posX, posY, neighbour);
            }

            // Retrieve positions of all neighbors
            Dictionary<string, Vector2> positions = new Dictionary<string, Vector2>();
            foreach (GameObject neighbour in neighbours)
            {
                PositionData positionData = neighbour.GetComponent<SquareFormation>().ReceivePositionFromNeighbour(neighbour);
                positions.Add(positionData.senderId, new Vector2((float)positionData.x, (float)positionData.y));
            }

            // Calculate the average position of the robot and its neighbors
            Vector2 avgPosition = new Vector2((float)posX, (float)posY);

            foreach (Vector2 position in positions.Values)
            {
                avgPosition += position;
            }

            // Point the object at the world origin (0,0,0)
            //transform.LookAt(Vector3.zero);


            //avgPosition /= (neighbours.Count + 1);

            // Update the position based on the average position and the desired square formation
            //double newX = centerX + radius * Mathf.Cos((float)targetAngle) + 0.5f * (avgPosition.x - posX);
            //double newY = centerY + radius * Mathf.Sin((float)targetAngle) + 0.5f * (avgPosition.y - posY);


            // Move to the updated position
            //yield return StartCoroutine(MoveToPosition(newX, newY));

            // Update the current position


            //posX = newX;
            //posY = newY;

            // Wait before the next iteration
            //yield return new WaitForSecondsRealtime((float)(syncInterval / 1000));
        }


        private void Clamp(double halfLength)
        {
            if (posX > halfLength)
            {
                posX = halfLength;
            }

            if (posX < -halfLength)
            {
                posX = -halfLength;
            }
            if (posY > halfLength)
            {
                posY = halfLength;
            }
            if (posY < -halfLength)
            {
                posY = -halfLength;
            }
        }

        // Replace out() with your Unity-specific communication method
        // out(SQUARE_FORMATION_COMPLETED)@self;

        private IEnumerator MoveToPosition(double targetX, double targetY)
        {
            for (int i = 1; i <= numIterations; i++)
            {
                Vector3 targetPosition = new Vector3((float)targetX, 0, (float)targetY);

                GetComponent<RobotControllerA>().destination.position = targetPosition;
                yield return StartCoroutine(GetComponent<RobotControllerA>().MoveCoroutine());
            }
        }

        private void AvoidObstacle()
        {
            float rayRange = 4f; // Set the desired range to 4 meters
            float targetVelocity = 10f;
            float avoidanceDistance = 1.0f; // Set the distance at which avoidance behavior is triggered
            float ObavoidanceDistance = 1.0f;
            float rayOffset = 0.15f; // Offset to move the rays out of the object's body

            var deltaPosition = Vector3.zero;
            for (int i = 0; i < numberOfRays; ++i)
            {
                var rotation = this.transform.rotation;
                var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle * 2 - angle, this.transform.up);
                var direction = rotation * rotationMod * Vector3.forward * rayRange;

                // Apply the ray offset to the starting position
                var rayStartPosition = this.transform.position + rayOffset * direction;

                var ray = new Ray(rayStartPosition, direction);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, rayRange, LayerMask.GetMask("Robot")))
                {
                    if (hitInfo.distance <= avoidanceDistance)
                    {
                        //var otherRobot = hitInfo.collider.gameObject;

                        // Check if the other robot has reached its destination
                        //var otherFormation = otherRobot.GetComponent<CircleFormation>();
                        if (IsCompleted == false)
                        {
                            // Calculate the avoidance direction based on the difference between the robot's position and the obstacle's position
                            Vector3 avoidanceDirection = (transform.position - hitInfo.point).normalized - (transform.forward);

                            //Vector3 desiredDirection = avoidanceDirection.normalized + transform.forward;
                            deltaPosition += (10.0f / numberOfRays) * targetVelocity * avoidanceDirection;

                            // Apply the avoidance direction to the delta position
                            Debug.DrawRay(ray.origin, direction, Color.red);

                        }
                    }
                }
                else if (Physics.Raycast(ray, out hitInfo, rayRange, LayerMask.GetMask("Obstacles")))
                {
                    if (hitInfo.distance <= ObavoidanceDistance)
                    {
                        // Calculate the avoidance direction based on the difference between the robot's position and the obstacle's position
                        Vector3 avoidanceDirection = (transform.position - hitInfo.point).normalized;

                        // Apply the avoidance direction to the delta position
                        deltaPosition += (1.0f / numberOfRays) * targetVelocity * avoidanceDirection;



                        if (avoidanceDirection != Vector3.zero)
                        {
                            // Calculate the desired direction by combining the avoidance direction and the robot's current direction
                            Vector3 desiredDirection = avoidanceDirection.normalized + transform.forward;

                            // Apply the desired direction to the delta position
                            deltaPosition = desiredDirection.normalized * targetVelocity;

                        }
                        Debug.DrawRay(ray.origin, direction, Color.cyan);

                    }
                }
                else
                {
                    Debug.DrawRay(ray.origin, direction, Color.white);
                }
            }

            this.transform.position += deltaPosition * Time.deltaTime;
        }


        private void SendMessageToNeighbour(string message, string senderId, double x, double y, GameObject neighbour)
        {
            // Construct the message data
            PositionData positionData = new PositionData(senderId, x, y);

            // Convert the position data to a JSON string
            json = JsonUtility.ToJson(positionData);

            // Send the message to the neighbour using your Unity-specific communication method
            //Debug.Log($"Message Sent: {positionData.senderId}: {positionData.x}, {positionData.y}");
            // Replace the code below with your actual communication method
            //neighbour.GetComponent<SquareFormation>().ReceivePositionFromNeighbour(message, senderId, x, y);
        }

        private PositionData ReceivePositionFromNeighbour(GameObject neighbour)
        {
            // Convert the JSON string to position data
            //PositionData positionData = JsonUtility.FromJson<PositionData>(json);

            // Process the received position data here

            PositionData neighbourData = new PositionData(robotId, neighbour.transform.position.x, neighbour.transform.position.z);
            //Debug.Log($"Received Position from {neighbourData.senderId}: {neighbourData.x}, {neighbourData.y}");

            // Return the position data
            return neighbourData;
        }

    }
}
    