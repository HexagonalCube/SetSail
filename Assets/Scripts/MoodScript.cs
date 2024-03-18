using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodScript : MonoBehaviour
{
    [SerializeField] float distFromIslands;
    [SerializeField] int gameStage = 1;
    [SerializeField] float inputFrequency;

    [SerializeField] IslandsDistanceController distIslands;
    [SerializeField] float distanceWeight = 1f;
    [SerializeField] float gameStageWeight = 1f;
    [SerializeField] float inputWeight = 1f;

    [SerializeField] public float mood;

    [SerializeField] int frequency;
    [SerializeField] int count;
    [SerializeField] int[] frequencies;
    [SerializeField] float frequencyAvg;
    private void Start()
    {
        StartCoroutine(Frequency());
        frequencies = new int[3];
    }
    private void Update()
    {
        KeyFrequency();
        mood = CalculateMood();
    }
    float CalculateMood()
    {
        distFromIslands = distIslands.dist;
        float var1 = distFromIslands * distanceWeight;
        float var2 = gameStage * gameStageWeight;
        float var3 = inputFrequency * inputWeight;
        return var1 + var2 + var3;
    }
    void KeyFrequency()
    {
        if (Input.anyKeyDown)
        {
            frequency += 1;
        }
    }
    IEnumerator Frequency()//stores how many keys have been pressed in a second 
    {
        yield return new WaitForSeconds(0.5f);
        frequencyAvg = 0;
        frequencies[count] = frequency;
        count++;
        if (count == frequencies.Length) { count = 0; }
        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencyAvg += frequencies[i];
        }
        frequencyAvg /= frequencies.Length;
        inputFrequency = frequencyAvg;
        yield return new WaitForSeconds(0.5f);
        frequency = 1;
        StartCoroutine(Frequency());
    }
}
