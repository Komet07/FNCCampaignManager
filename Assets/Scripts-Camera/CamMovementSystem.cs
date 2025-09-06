using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovementSystem : MonoBehaviour
{
    public float CamSpeed = 10f;
    public float ZoomSpeed = 1f;
    public Camera Camera;

    public float _dragSpeed = 1;
    public Vector3 _basePosition = new Vector3(0,0,0);
    public float _scale = 1;


    public float _xAngle = 0;
    public float _yAngle = 0;
    public float _zAngle = 0;

    public GameObject _focusObject;

    [Tooltip("Position of mouse in last frame")]
    private Vector3 lastMousePos;
    [Tooltip("Pose of camera in last frame")]
    private Pose lastCamPose;

    public List<GameObject> _uiObjs = new List<GameObject>(); // Objects that will trigger 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!MapManager.Instance._system)
        {
            return;
        }

        Camera.orthographic = false;

        
        float HorizontalMovement = Input.GetAxis("Horizontal") * (CamSpeed * (Camera.orthographicSize / 5));
        float VerticalMovement = Input.GetAxis("Vertical") * (CamSpeed * (Camera.orthographicSize / 5));

        HorizontalMovement *= Time.deltaTime;
        VerticalMovement *= Time.deltaTime;



        _basePosition += (transform.forward * VerticalMovement) + (transform.right * HorizontalMovement);
        transform.position += (transform.forward * VerticalMovement) + (transform.right * HorizontalMovement);

        transform.position -= new Vector3(0, _basePosition.y, 0);
        _basePosition -= new Vector3(0, _basePosition.y, 0);


        Camera.transform.LookAt(_basePosition);


        if (Input.GetKeyDown("r"))
        {
            _scale = 1;
            Vector3 _a = _basePosition;
            _basePosition = new Vector3(0, 0, -0);
            transform.position -= _a;
        }

        // On mouse button down initially
        if (Input.GetMouseButtonDown(0))
        {
            // Start with current mouse position
            lastMousePos = Input.mousePosition;
        }

        bool _uiActive = false;

        for (int i = 0; i < _uiObjs.Count; i++)
        {
            if (_uiObjs[i].activeSelf)
            {
                _uiActive = true;
            }
        }

        ScaleDistance();

        // On mouse button pressed down continuously
        if (Input.GetMouseButton(0) && !_uiActive)
        {
            // Rotate camera about object center
            RotateCamera();
        }

        transform.LookAt(_basePosition);
        


    }

    void RotateCamera()
    {
        // Calculate mouse movement delta & update last mouse position to current
        Vector3 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        // Calculate rotation amount based on mouse movement delta
        float mouseX = mouseDelta.x * _dragSpeed;
        float mouseY = mouseDelta.y * _dragSpeed;

        // Store previous camera pose
        lastCamPose = new Pose(transform.position, transform.rotation);

        // Rotate camera about object
        transform.RotateAround(_basePosition, Vector3.up, mouseX);
        transform.RotateAround(_basePosition, transform.right, -mouseY);

        // If camera upside down -> Revert to previous rotation
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            transform.SetPositionAndRotation(lastCamPose.position, lastCamPose.rotation);
        }
    }

    void ScaleDistance()
    {
        float _scaleMovement = 0;

        if (Input.GetKey(KeyCode.E))
        {
            _scaleMovement = -1 * ZoomSpeed;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _scaleMovement = 1 * ZoomSpeed;
        }

        if (Input.mouseScrollDelta.y == 0)
        {
            _scaleMovement += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * 2.5f;
        }
        else
        {
            _scaleMovement += Input.mouseScrollDelta.y * ZoomSpeed * 2.5f * -1;
        }


        _scale += _scaleMovement * Time.deltaTime;
        _scale = Mathf.Clamp(_scale, .5f, 10f);



        transform.position = _basePosition + _scale * Vector3.Normalize(transform.position - _basePosition);

    }
}
