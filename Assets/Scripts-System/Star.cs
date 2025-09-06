using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemMap
{
    public class Star : MonoBehaviour
    {

        [Header("Star Light")]
        public Color _color;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        }

        // Update is called once per frame
        void Update()
        {
            ReassignColor();
        }


        void ReassignColor()
        {
            MeshRenderer _mR = GetComponent<MeshRenderer>();

            _mR.material.SetColor("_EmissionColor", _color);
            _mR.material.SetColor("_Color", _color);

        }
    }
}
    
