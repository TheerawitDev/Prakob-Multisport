using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class HardwareSkiController : MonoBehaviour
{
    [Header("Serial Port Settings")]
    [Tooltip("The COM port your microcontroller is connected to. (e.g. COM3)")]
    public string portName = "COM3";
    [Tooltip("Must match the baud rate in your Arduino/ESP32 code.")]
    public int baudRate = 9600;

    [Header("Movement Settings")]
    [Tooltip("Normal forward speed when standing.")]
    public float baseForwardSpeed = 10f;
    [Tooltip("Increased forward speed when tucking/squatting.")]
    public float sprintForwardSpeed = 20f;
    [Tooltip("Speed of sideways movement when leaning.")]
    public float lateralSpeed = 5f;
    
    [Header("Camera Settings")]
    [Tooltip("Reference to the First-Person Camera. Drag your Main Camera here.")]
    public Transform playerCamera;
    [Tooltip("Y-position of camera when standing.")]
    public float normalCameraY = 1.6f;
    [Tooltip("Y-position of camera when squatting/tucking.")]
    public float tuckCameraY = 1.0f;
    [Tooltip("How fast the camera transitions between standing and tucking.")]
    public float cameraTransitionSpeed = 5f;

    [Header("Squat Threshold")]
    [Tooltip("Thigh angle below which the player is considered squatting.")]
    public float squatAngleThreshold = 45f;

    // Parsed Hardware Input Variables
    private bool leftLean = false;
    private bool rightLean = false;
    private float thighAngle = 90f;

    // Serial Communication Objects
    private SerialPort serialPort;
    private Thread serialThread;
    private bool isRunning = false;

    // Thread-safe variables for passing data between the background thread and main thread
    private string lastReceivedString = "";
    private readonly object lockObject = new object();
    
    // Current smoothed variables
    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseForwardSpeed;
        InitializeSerial();
    }

    void InitializeSerial()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            // ReadTimeout ensures the read method doesn't block forever if no data comes
            serialPort.ReadTimeout = 50; 
            serialPort.Open();

            isRunning = true;
            // Start reading in a background thread so the Unity Editor/Game doesn't freeze
            serialThread = new Thread(ReadSerialData);
            serialThread.IsBackground = true;
            serialThread.Start();
            Debug.Log($"<color=green>Successfully opened Serial Port {portName}</color>");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"<color=red>Failed to open Serial Port {portName}: {e.Message}</color>\nEnsure the port is correct and not opened in another program like Arduino IDE.");
        }
    }

    // This runs continuously in a background thread
    void ReadSerialData()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0)
                {
                    string data = serialPort.ReadLine(); // Reads until a newline '\n' character
                    
                    // Use a lock to safely pass the string to the Unity main thread
                    lock (lockObject)
                    {
                        lastReceivedString = data;
                    }
                }
            }
            catch (System.TimeoutException)
            {
                // Expected timeout if no data is sent within the ReadTimeout window, just continue loop
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Serial Read Error: " + e.Message);
            }
            
            // Small sleep to prevent this thread from eating 100% of a CPU core
            Thread.Sleep(10); 
        }
    }

    void Update()
    {
        ParseLatestData();
        HandleCameraAndSpeed();
        HandleMovement();
    }

    void ParseLatestData()
    {
        string dataToParse = "";
        
        // Safely extract the latest string from the background thread
        lock (lockObject)
        {
            dataToParse = lastReceivedString;
        }

        if (!string.IsNullOrEmpty(dataToParse))
        {
            // Expected Format: LeftPadTouched,RightPadTouched,ThighAngle
            // Example: "1,0,85"
            string[] values = dataToParse.Split(',');
            
            if (values.Length >= 3)
            {
                if (int.TryParse(values[0], out int leftVal)) 
                    leftLean = (leftVal == 1);
                    
                if (int.TryParse(values[1], out int rightVal)) 
                    rightLean = (rightVal == 1);
                    
                if (float.TryParse(values[2], out float angleVal)) 
                    thighAngle = angleVal;
            }
        }
    }

    void HandleCameraAndSpeed()
    {
        // 1. Determine Stance
        bool isSquatting = thighAngle < squatAngleThreshold;

        // 2. Set Target Variables Based on Stance
        float targetSpeed = isSquatting ? sprintForwardSpeed : baseForwardSpeed;
        float targetHeight = isSquatting ? tuckCameraY : normalCameraY;

        // 3. Smoothly adjust speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * cameraTransitionSpeed);

        // 4. Smoothly adjust camera height
        if (playerCamera != null)
        {
            Vector3 camPos = playerCamera.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, targetHeight, Time.deltaTime * cameraTransitionSpeed);
            playerCamera.localPosition = camPos;
        }
    }

    void HandleMovement()
    {
        // Calculate forward movement
        Vector3 movement = transform.forward * currentSpeed;

        // Add lateral (left/right) movement based on capacitive pads
        if (leftLean && !rightLean)
        {
            movement -= transform.right * lateralSpeed;
        }
        else if (rightLean && !leftLean)
        {
            movement += transform.right * lateralSpeed;
        }

        // Note: Using transform.Translate ignores physics. 
        // If your game requires physical collisions (like crashing into trees), 
        // you would assign this velocity to a Rigidbody instead: 
        // rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        
        transform.position += movement * Time.deltaTime;
    }

    // Crucial: Clean up the thread and port when the game stops playing
    void OnDestroy()
    {
        isRunning = false;
        
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join(500); // Wait briefly for the thread to finish
        }

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial Port Closed.");
        }
    }
}
