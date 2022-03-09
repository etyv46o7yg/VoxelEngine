using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditeurAddLayer : MonoBehaviour
    {
    public PanelAvecSlider bas, haut;
    // Start is called before the first frame update
    void Awake()
        {
        bas.ValChager  += FaireQuelque;
        haut.ValChager += FaireQuelque;
        }

    // Update is called once per frame

    public void DrawLevel()
        {

        }

    public void FaireZero()
        {

        }

    public void FaireQuelque(float _param)
        {

        }
    }
