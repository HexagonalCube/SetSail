using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRISM.Utils { 

    public class CUPPCanvasFadein : MonoBehaviour
    {

        public CanvasGroup thisCanvas;
        // Start is called before the first frame update
        void Start()
        {
            thisCanvas = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            if(thisCanvas.alpha < 1.0f)
            {
                thisCanvas.alpha += Time.deltaTime;
            }
        }
    }
}