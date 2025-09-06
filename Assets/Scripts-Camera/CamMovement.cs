using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class CamMovement : MonoBehaviour
{
    #region Singleton
    public static CamMovement Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public float CamSpeed = 10f;
    public float ZoomSpeed = 1f;
    public Camera Camera;


    public float orthoBoundaryMin = .25f;
    public float orthoBoundaryMax = 10f;

    public bool _reset = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!MapManager.Instance._galaxy)
        {
            return;
        }

        Camera.orthographic = true;

        //if (!GMMenu.Instance._menuObjectsL2.activeSelf)
        //{
        float HorizontalMovement = Input.GetAxis("Horizontal") * (CamSpeed * (Camera.orthographicSize / 5));
        float VerticalMovement = Input.GetAxis("Vertical") * (CamSpeed * (Camera.orthographicSize / 5));
        float orthoMovement = Input.GetAxis("Zoom") * (ZoomSpeed);

        float MouseXMovement = 0;
        float MouseYMovement = 0;

        if (Input.GetMouseButton(0) && !OutlinerUIGalaxy.Instance._fleetIsOn)
        {
            MouseXMovement = Input.GetAxis("Mouse X") * -1 * (CamSpeed * Camera.orthographicSize / 2);
            MouseYMovement = Input.GetAxis("Mouse Y") * -1 * (CamSpeed * Camera.orthographicSize /2);
        }

        if (Input.mouseScrollDelta.y == 0)
        {
            orthoMovement += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * 2.5f;
        }
        else
        {
            orthoMovement += Input.mouseScrollDelta.y * ZoomSpeed * 2.5f * -1;
        }
                
                

        HorizontalMovement *= Time.deltaTime;
        VerticalMovement *= Time.deltaTime;
        MouseXMovement *= Time.deltaTime;
        MouseYMovement *= Time.deltaTime;
        orthoMovement *= Time.deltaTime;

        transform.Translate(HorizontalMovement + MouseXMovement, VerticalMovement + MouseYMovement, 0);
            
        Camera.orthographicSize += orthoMovement;

        Camera.transform.position = new Vector3(Mathf.Clamp(Camera.transform.position.x, MapManager.Instance._map.xBoundaryMin, MapManager.Instance._map.xBoundaryMax), Mathf.Clamp(Camera.transform.position.y, MapManager.Instance._map.yBoundaryMin, MapManager.Instance._map.yBoundaryMax), -10);
        Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, orthoBoundaryMin, orthoBoundaryMax);

        if ((Input.GetKeyDown("r") || _reset) && MapManager.Instance.CameraResetIncompatCheck)
        {
            _reset = false;
            Camera.orthographicSize = 1.5f;
            /* if (MapManager.Instance._map._playerFactionId < 0 || MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._spawnSectorID < 0)
            {
                Camera.transform.position = new Vector3(0, 0, -10);
            }
            else 
            {
                Sector S = MapManager.Instance._map._sectors[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._spawnSectorID];
                Vector2 _p = MapManager.Instance.TurnSectorIntoRealPos(new Vector2(S._posXInt, S._posYInt));
                Camera.transform.position = new Vector3(_p.x, _p.y, -10);

            } */

            if (MapManager.Instance._map._playerFactionId < 0)
            {
                Camera.transform.position = new Vector3(0, 0, -10);
            }
            else
            {
                Vector2 _p = MapManager.Instance.TurnSectorIntoRealPos(MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._spawnPosition);
                Camera.transform.position = new Vector3(_p.x, _p.y, -10);
            }
                 
        }
        //}
            
    }
}
