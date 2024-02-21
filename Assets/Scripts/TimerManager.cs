using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TimerManager : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI timerText;
    public Player player;
    private float curTime;

    public float maxTime;
    public float fillRate;
    // Modified by player script
    public bool isDraining;
    public bool isFilling;


    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxTime;
        curTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDraining) {
            curTime -= Time.deltaTime;
        }
        if(isFilling) {
            curTime += Time.deltaTime * fillRate;
        }
        if(curTime > maxTime) {
            curTime = maxTime;
        }

        timerText.text = Mathf.Floor(curTime).ToString("#");
        slider.value = curTime;

        if(curTime < 0.0f) {
            player.Die();
            curTime = 0.0f;
            isDraining = false;
        }
    }
}