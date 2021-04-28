using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAvecSlider : MonoBehaviour
    {
    public float defoultPositionSlider;

    public Slider slider;
    public float val;
    public Vector2 pointDInflection;
    Text courrValText;
    public float scale = 1.0f;

    public event Changer ValChager;
    // Start is called before the first frame update
    void Start()
        {
        courrValText = this.transform.GetChild(0).GetComponent<Text>();
        slider = GetComponentInChildren<Slider>();

        slider.onValueChanged.AddListener(delegate { SliderAEteChange(); });

        slider.value = defoultPositionSlider;
        ValChager();
        SliderAEteChange();
        }

    // Update is called once per frame
    void Update()
        {

        }

    void SliderAEteChange()
        {
        float a = pointDInflection.x;
        float b = pointDInflection.y;

        if (slider.value < pointDInflection.x)
            {
            val = pointDInflection.y * slider.value / pointDInflection.x;
            }
        else
            {
            val = b + (1.0f - b) * (slider.value - a) / (1.0f - a);
            }

        val *= scale;

        courrValText.text = val.ToString();
        
        ValChager();
        }

    public delegate void Changer();
    }
