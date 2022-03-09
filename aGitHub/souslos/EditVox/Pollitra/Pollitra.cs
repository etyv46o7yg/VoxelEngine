using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pollitra : MonoBehaviour
    {
    public RawImage mainImagePolitra, ImageIndicator;
    Texture2D palitreTex;
    public Color Color_1;
    Vector2 sizeTex;
    // Start is called before the first frame update
    void Start()
        {
        palitreTex = (Texture2D) mainImagePolitra.texture;
        sizeTex = new Vector2( palitreTex.width, palitreTex.height);
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public void CliqueOnPalitre(Vector2 _worldPos)
        {       
        Vector2 localPosTex = CanvasImageClique.GetLocalUICoordinates(mainImagePolitra.rectTransform, _worldPos) / mainImagePolitra.rectTransform.rect.size * sizeTex;


        Color_1 =  palitreTex.GetPixel( (int) localPosTex.x, (int) localPosTex.y );
        Color_1.a = EditorPrince.instance.sliderAlfa.val;
        ImageIndicator.color = new Color( Color_1.r, Color_1.g, Color_1.b, 1.0f);
        }

    public void AlphaChange(float _alfa)
        {
        Color_1.a = EditorPrince.instance.sliderAlfa.val;
        //ImageIndicator.color = Color_1;
        }

    }
