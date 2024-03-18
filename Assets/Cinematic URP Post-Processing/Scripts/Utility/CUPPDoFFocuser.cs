using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using PRISM.Utils;

namespace PRISM.Utils
{



    public class CUPPDoFFocuser : MonoBehaviour
    {
        public Volume targetCUPPEffectsVolumeToChange;
        private PRISMDepthOfField targetCUPPEffectsToChange;

        private void Start()
        {
            if(targetCUPPEffectsVolumeToChange)
            {
                Volume volume = gameObject.GetComponent<Volume>();
                PRISMDepthOfField tmp;
                if (volume.profile.TryGet<PRISMDepthOfField>(out tmp))
                {
                    targetCUPPEffectsToChange = tmp;
                }
            } else
            {
                PRISMDepthOfField tmp;
                if (targetCUPPEffectsVolumeToChange.profile.TryGet<PRISMDepthOfField>(out tmp))
                {
                    targetCUPPEffectsToChange = tmp;
                }
            }


        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit rcHit;
            bool b = Physics.Raycast(transform.position, transform.forward * 1000f, out rcHit);

            if (b)
            {
                Debug.Log("Hit something");
                targetCUPPEffectsToChange.focusDistance.value = rcHit.distance;
                Debug.Log(rcHit.distance);
            }
        }
    }



}
