using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public float CamSpeed = 10f;
    public float ZoomSpeed = 1f;
    public Camera Camera;

    public float xBoundaryMin = -5;
    public float xBoundaryMax = 5;
    public float yBoundaryMin = -5;
    public float yBoundaryMax = 5;

    public float orthoBoundaryMin = .25f;
    public float orthoBoundaryMax = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        float HorizontalMovement = Input.GetAxis("Horizontal") * (CamSpeed * (Camera.orthographicSize / 5));
        float VerticalMovement = Input.GetAxis("Vertical") * (CamSpeed * (Camera.orthographicSize / 5));
        float orthoMovement = Input.GetAxis("Zoom") * (ZoomSpeed);

        HorizontalMovement *= Time.deltaTime;
        VerticalMovement *= Time.deltaTime;
        orthoMovement *= Time.deltaTime;

        transform.Translate(HorizontalMovement, VerticalMovement, 0);
        Camera.orthographicSize += orthoMovement;

        Camera.transform.position = new Vector3(Mathf.Clamp(Camera.transform.position.x, xBoundaryMin, xBoundaryMax), Mathf.Clamp(Camera.transform.position.y, yBoundaryMin, yBoundaryMax), -10);
        Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, orthoBoundaryMin, orthoBoundaryMax);

        if (Input.GetKeyDown("r"))
        {
            Camera.orthographicSize = 1.5f;
            Camera.transform.position = new Vector3(0, 0, -10);
        }
    }
}
