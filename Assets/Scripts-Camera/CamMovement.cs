using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public float CamSpeed = 10f;
    public float ZoomSpeed = 1f;
    public Camera Camera;


    public float orthoBoundaryMin = .25f;
    public float orthoBoundaryMax = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!GMMenu.Instance._menuObjectsL2.activeSelf)
        {
            float HorizontalMovement = Input.GetAxis("Horizontal") * (CamSpeed * (Camera.orthographicSize / 5));
            float VerticalMovement = Input.GetAxis("Vertical") * (CamSpeed * (Camera.orthographicSize / 5));
            float orthoMovement = Input.GetAxis("Zoom") * (ZoomSpeed);

            float MouseXMovement = 0;
            float MouseYMovement = 0;

            if (Input.GetMouseButton(0))
            {
                MouseXMovement = Input.GetAxis("Mouse X") * -1 * (CamSpeed * Camera.orthographicSize / 2);
                MouseYMovement = Input.GetAxis("Mouse Y") * -1 * (CamSpeed * Camera.orthographicSize /2);
            }

            orthoMovement += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * 2.5f;
                

            HorizontalMovement *= Time.deltaTime;
            VerticalMovement *= Time.deltaTime;
            MouseXMovement *= Time.deltaTime;
            MouseYMovement *= Time.deltaTime;
            orthoMovement *= Time.deltaTime;

            transform.Translate(HorizontalMovement + MouseXMovement, VerticalMovement + MouseYMovement, 0);
            
            Camera.orthographicSize += orthoMovement;

            Camera.transform.position = new Vector3(Mathf.Clamp(Camera.transform.position.x, MapManager.Instance._map.xBoundaryMin, MapManager.Instance._map.xBoundaryMax), Mathf.Clamp(Camera.transform.position.y, MapManager.Instance._map.yBoundaryMin, MapManager.Instance._map.yBoundaryMax), -10);
            Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, orthoBoundaryMin, orthoBoundaryMax);

            if (Input.GetKeyDown("r"))
            {
                Camera.orthographicSize = 1.5f;
                Camera.transform.position = new Vector3(0, 0, -10);
            }
        }
            
    }
}
