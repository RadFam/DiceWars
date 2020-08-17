using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControls
{
    public class ControlNotDestroyScript : MonoBehaviour
    {
        private static ControlNotDestroyScript instance;
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}