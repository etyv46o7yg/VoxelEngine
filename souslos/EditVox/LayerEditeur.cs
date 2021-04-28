using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerEditeur : MonoBehaviour
    {
    public PanelAvecSlider bas, haut;
    // Start is called before the first frame update
    void Start()
        {
        bas.ValChager  += FaireZero;
        haut.ValChager += FaireZero;
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public void FaireZero()
        {

        }
    }
