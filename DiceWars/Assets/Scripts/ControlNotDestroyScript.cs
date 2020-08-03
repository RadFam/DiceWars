using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControls
{
    public class ControlNotDestroyScript : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}