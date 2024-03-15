using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using PRISM.Utils;
using System.Linq;
using System;

namespace PRISM.Utils
{
    public static class EnumExtensions
    {
        public static string NumberString(this ShutterSpeedValue enVal)
        {
            return enVal.ToString("D");
        }
    }

    public static class Enums
    {
        public static T Next<T>(this T v) where T : struct
        {
            return Enum.GetValues(v.GetType()).Cast<T>().Concat(new[] { default(T) }).SkipWhile(e => !v.Equals(e)).Skip(1).First();
        }

        public static T Previous<T>(this T v) where T : struct
        {
            return Enum.GetValues(v.GetType()).Cast<T>().Concat(new[] { default(T) }).Reverse().SkipWhile(e => !v.Equals(e)).Skip(1).First();
        }
    }

    public enum ShutterSpeedValue
    {
        OneFourHundredth = 400,
        OneTwoHundredth = 200,
        OneOneSixtyth = 160,
        OneOneTwentyFifth = 125,
        OneOneHundredth = 100,
        OneEightyth = 80,
        OneSixtyth = 60,
        OneFiftyth = 50,
        OneFourtyth = 40,
        OneThirtyth = 30,
        OneTwentyFifth = 25,
        OneTwentieth = 20,
        OneFifteenth = 15,
        OneTenth = 10,
        OneFifth = 5,
        OneHalf = 2,
        One = 1,
    };

    public enum ISOValue { ISO100=0, ISO200=1, ISO400=2, ISO800=3, ISO1600=4, ISO3200=5, ISO6400=6, ISO12800=7 };

    public class CUPPISONoise : MonoBehaviour
    {
        private PRISMEffects targetCUPPEffectsToChange;
        private float t = 0f;
        private float lerpSpeed = 1f;

        public float originalExposure = 1f;
        public float originalColorTemp = 1f;

        public ISOValue setISOValue = ISOValue.ISO100;
        public ISOValue newISOValue = ISOValue.ISO100;

        public VolumeProfile[] isoProfiles;

        private void Start()
        {
            Volume volume = gameObject.GetComponent<Volume>();
            PRISMEffects tmp;
            if (volume.profile.TryGet<PRISMEffects>(out tmp))
            {
                targetCUPPEffectsToChange = tmp;
            }
        }

        [ContextMenu("TestSet")]
        public void TestSet()
        {
            Start();
            SetNewISOValue(newISOValue);
        }

        public void SetNewISOValue(ISOValue newISO)
        {
            PRISMEffects tmp;
            if (isoProfiles[(int)newISO].TryGet<PRISMEffects>(out tmp))
            {
                //Debug.Log(tmp.exposure);
                targetCUPPEffectsToChange.SetAllOverridesTo(true);

                targetCUPPEffectsToChange.exposure.value = tmp.exposure.value;
                targetCUPPEffectsToChange.useFilmicNoise.value = tmp.useFilmicNoise.value;
                targetCUPPEffectsToChange.sensorNoise.value = tmp.sensorNoise.value;
                
                setISOValue = newISO;
            }
        }

        // Update is called once per frame
        void Update()
        {
            t += Time.deltaTime * lerpSpeed;

            if (t > 1f)
            {
                lerpSpeed = -1f;
            }

            if (t <= 0f)
            {
                lerpSpeed = 1f;
            }

            /*if (propertyToChange == PostProcessPropertyToChangeType.ColorTemp)
            {
                targetCUPPEffectsToChange.colorTemperature.value = originalColorTemp * t;
            }
            else if (propertyToChange == PostProcessPropertyToChangeType.Exposure)
            {
                targetCUPPEffectsToChange.exposure.value = originalExposure * t;
            }
            else //Nothing
            {
                targetCUPPEffectsToChange.colorTemperature.value = originalColorTemp;
                targetCUPPEffectsToChange.exposure.value = originalExposure;
            }*/
        }
    }



}
