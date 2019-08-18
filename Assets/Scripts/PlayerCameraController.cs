using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCameraController : MonoBehaviour
{
    public static PlayerCameraController instance; // Singleton for this script

    private Player _player;

    private Camera PlayerCamera;

    public GameObject TargetCameraRotatesAround; // Player's ship game object around which the camera rotates

    public Vector3 cameraLookingPoint; // Point on which camera is looking

    public Vector3 cameraOffset;

    public float cameraAimRayLength; // Ray length that based on camera far clip plane
    private float cameraDefaultFieldOfView;
    private float zoomedCameraFieldOfView = 10f; // Field of view when zooming

    private float defaultMouseRotateSensitivity = 3f;
    private float curentMouseRotateSensitivity;
    private float limitRotateY = 90f; // Rotation limit by Y

    [SerializeField] private float scrollWheelSensitivity = 1f;
    [SerializeField] private float scrollMax = 6f;
    [SerializeField] private float scrollMin = 0f;

    private float cameraRotateAngleX;
    private float cameraRotateAngleY;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        if (instance == null) // If instance not exist
        {
            instance = this; // Set up instance as this script
        }
        else //If instance already exists
        {
            Destroy(this); // Destroy this script
        }

        Manager.Instance.onPlayerAssigned += ActivateCamera; // Calls when player ship spawned
    }

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void ActivateCamera()
    {
        PlayerCamera = GetComponent<Camera>(); // Get camera component on this game object
        cameraDefaultFieldOfView = PlayerCamera.fieldOfView; // Set the current camera field of view as default
        cameraAimRayLength = PlayerCamera.farClipPlane; // Set ray length
        curentMouseRotateSensitivity = defaultMouseRotateSensitivity; // Set current mouse rotate sensitivity

        limitRotateY = Mathf.Abs(limitRotateY); // Get the absolute value of limit rotate Y
        limitRotateY = Mathf.Clamp(limitRotateY, -Mathf.Infinity, 90); // Clamp limit rotate Y up to 90

        cameraOffset = new Vector3(cameraOffset.x, cameraOffset.y, -Mathf.Abs(scrollMax) / 2); // Set camera offset for start camera position

        transform.position = TargetCameraRotatesAround.transform.position + cameraOffset; // Set start camera position
    }

    private void Update() // Update is called every frame
    {
        // If the cursor is visible set camera to default field of view and set default sensitivity
        //if (Cursor.visible)
        //{
        //    PlayerCamera.fieldOfView = cameraDefaultFieldOfView; // Set default camera field of view
        //    curentMouseRotateSensitivity = defaultMouseRotateSensitivity; // Set default mouse rotate sensitivity

        //    return;
        //}

        // If player ship has been destroyed set camera to default field of view, set default sensitivity and allow to rotate a camera
        if (!_player.IsAlive)
        {
            PlayerCamera.fieldOfView = cameraDefaultFieldOfView; // Set default camera field of view
            curentMouseRotateSensitivity = defaultMouseRotateSensitivity; // Set default mouse rotate sensitivity

            cameraRotateAngleX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * curentMouseRotateSensitivity; // Set new camera rotate angle by X
            cameraRotateAngleY += Input.GetAxis("Mouse Y") * curentMouseRotateSensitivity; // Set new camera rotate angle by Y
            cameraRotateAngleY = Mathf.Clamp(cameraRotateAngleY, -limitRotateY, limitRotateY); // Clamp rotation by Y

            transform.localEulerAngles = new Vector3(-cameraRotateAngleY, cameraRotateAngleX, 0); // Set new camera rotation
            transform.position = transform.localRotation * cameraOffset; // Set new camera position

            return;
        }

        // Camera rotating
        cameraOffset.z = Mathf.Clamp(cameraOffset.z, -Mathf.Abs(scrollMax), -Mathf.Abs(scrollMin)); // Clamp camera offset

        cameraRotateAngleX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * curentMouseRotateSensitivity; // Set new camera rotate angle by X
        cameraRotateAngleY += Input.GetAxis("Mouse Y") * curentMouseRotateSensitivity; // Set new camera rotate angle by Y
        cameraRotateAngleY = Mathf.Clamp(cameraRotateAngleY, -limitRotateY, limitRotateY); // Clamp rotation by Y

        transform.localEulerAngles = new Vector3(-cameraRotateAngleY, cameraRotateAngleX, 0); // Set new camera rotation
        transform.position = transform.localRotation * cameraOffset + TargetCameraRotatesAround.transform.position; // Set new camera position


        // Camera zooming
        //if (Input.GetKey(KeyCode.Mouse1)) // If right mouse button is pressed
        //{
        //    PlayerCamera.fieldOfView = zoomedCameraFieldOfView; // Set new camera field of view
        //    curentMouseRotateSensitivity = defaultMouseRotateSensitivity / 3; // Decrease mouse rotate sensitivity
        //}
        //else // If right mouse button is not pressed
        //{
        //    PlayerCamera.fieldOfView = cameraDefaultFieldOfView; // Set default camera field of view
        //    curentMouseRotateSensitivity = defaultMouseRotateSensitivity; // Set default mouse rotate sensitivity
        //}


        // Camera scrolling
        cameraOffset.z += Input.GetAxis("Mouse ScrollWheel") * scrollWheelSensitivity * 10;


        // Ray that comes out of the camera and determining contact point if it hit something
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, cameraAimRayLength)) // If this ray hits something
        {
            cameraLookingPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z); // Set hit coordinates

            Manager.Instance.CurrentSelectedTarget = hit.transform.gameObject; // Set current selected target as hitted game object
            Manager.Instance.LastSelectedTarget = hit.transform.gameObject; // Set last selected target as hitted game object
            Manager.Instance.onCurrentSelectedTargetAssigned?.Invoke();
        }
        else // If ray doesn't hit anything
        {
            // Set the point that is at the end of the ray coming out of the camera
            cameraLookingPoint = new Vector3(transform.position.x + transform.forward.x * cameraAimRayLength,
                transform.position.y + transform.forward.y * cameraAimRayLength,
                transform.position.z + transform.forward.z * cameraAimRayLength);

            Manager.Instance.CurrentSelectedTarget = null; // Clear current selected target
        }
    }
}
