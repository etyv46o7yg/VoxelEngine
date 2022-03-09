using Mathan;
using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorPrince : MonoBehaviour
    {
    public MenedgerRouts menedgerRouts;

    public static EditorPrince instance = null;
    public RawImage mainImageEditor;
    public PanelAvecSlider   sliderAlfa, sliderSizeBrush, slideDureteBrush;
    int    courrDeptTex, courrLevel;
    Texture2D sliceTex;
    public Text   textCourrLevel;    //--------------------------------
    public RenderTexture rayMap;
    public Minimal_Render render;

    public Vector2 scale2Da3D;

    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public UIRayCast uiRayCast;
    public Pollitra pollitra;

    public RawImage   renderImage;
    public Toggle     toggleContin, toggleScreenGlobal, toggleEstAdd;
    CanvasImageClique imageClique;
    public UndoRedo<SauverMonde> undoRedo;
    public UndoRedo<PieceDeMonde> undoRedo_2;
    const int size = 256;

    [Range(0, 1)]
    public float f;

    public Predicatus predicatusContirDraw;
    public bool estScreen = true, estGlobal = true, estAdd = true;

    private void Awake()
        {
        if (instance == null)
            {
            instance = this;
            }
        else if (instance == this)
            {
            Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
    void Start()
        {
        predicatusContirDraw = new Predicatus();

        toggleContin.      onValueChanged.AddListener( delegate { ChangerToggleContiniusMode (toggleContin      ); } );
        toggleScreenGlobal.onValueChanged.AddListener( delegate { ChangerScreenWorld         (toggleScreenGlobal); } );
        toggleEstAdd.      onValueChanged.AddListener( delegate { ChangerEstAdd              (toggleEstAdd      ); } );
        
        toggleContin.isOn       = false;
        toggleScreenGlobal.isOn = false;
        toggleEstAdd.isOn       = true;

        slideDureteBrush.ValChager += MonHandler;
        sliderSizeBrush.ValChager  += MonHandler;
        sliderAlfa.ValChager       += AlphaChanger;

        imageClique = renderImage.GetComponent<CanvasImageClique>();
        imageClique.ACommoncerClique += CommonnceLaPaintire;
        imageClique.AFinirClique     += FinitLaPaintire;        

        undoRedo   = new UndoRedo<SauverMonde>(5);
        undoRedo_2 = new UndoRedo<PieceDeMonde>(50);
        
        menedgerRouts.directionPaint.VectorAEteChanger += ChanderVectorDirPaint;
        }

    // Использовать только для обработки непрерывных кликов!
    void Update()
        {
        Vector2 moucePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


        if ( Input.GetMouseButtonDown(0) )
            {
            var res = uiRayCast.RayCast( moucePos);
            foreach (var item in res)
                {              

                if (item.gameObject.CompareTag("Palitre"))
                    {
                    pollitra.CliqueOnPalitre(moucePos);
                    }

                }           

            }

        }


    public void CliqueOnRenderImage()
        {
        Vector2 _worlsPosCursor = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 imageSize = new Vector2(renderImage.texture.width, renderImage.texture.height);

        RectTransform rect = renderImage.rectTransform;
        Vector2 localPos =  imageClique.GetLocalUICoordinates_2(_worlsPosCursor);

        if (instance.predicatusContirDraw.GetValue())
            {
            render.Draw(localPos, pollitra.Color_1, sliderSizeBrush.val, slideDureteBrush.val, estScreen, estAdd);
            }
        //AltRayTraceTexture(render.gameObject.transform.position, dir, 500);
        }

    /// <summary>
    /// загрузка для обработки текстуры
    /// </summary>
    /// <param name="_tex"></param>
    public void Load(Texture3D _tex)
        {       
        render.monde.texture = _tex;

        mainImageEditor.texture = GetSlice(render.monde.texture, 50);
        courrDeptTex = render.monde.texture.depth;
        courrLevel = 0;
        sliceTex = (Texture2D) mainImageEditor.texture;

        scale2Da3D = mainImageEditor.rectTransform.sizeDelta / new Vector2(_tex.width, _tex.height);
        }

    void ChanderVectorDirPaint(Vector3 _dir)
        {
        Debug.Log("измеяю направение рисования " + _dir);
        render.ChangerDirection(_dir);
        }

    Texture2D GetSlice(Texture3D _tex, int slice)
        {
        Texture2D res = new Texture2D(_tex.width, _tex.height);
        for (int i = 0; i < res.width; i++)
            {
            for (int j = 0; j < res.height; j++)
                {
                res.SetPixel(i, j, _tex.GetPixel(i, j, slice) );
                }
            }

        res.Apply();
        return res;
        }

    /// <summary>
    /// обновить текстуру после изменения
    /// </summary>
    void RafraichirTexture3D()
        {
        for (int i = 0; i < render.monde.texture.width; i++)
            {
            for (int j = 0; j < render.monde.texture.height; j++)
                {
                render.monde.texture.SetPixel( i, j, courrLevel, sliceTex.GetPixel(i,j) );
                }
            }
        }

    public void CommonnceLaPaintire()
        {
        
        }

    public void FinitLaPaintire(Vector3 a, Vector3 b)
        { 

        }

    public void RayTraceTexture(Vector3 pos, Vector3 dir, float distanca)
        {
        float length = distanca;
        Vector3 courrPos = pos;

        int countR = 0, countV = 0;

        for (int i = 0; i < length; i++)
            {
            courrPos = pos - dir * i;
            int3 index = new int3( new Vector3( courrPos.x, courrPos.z, courrPos.y) );
            Color courrColor = render.monde.SafetyReadTex(index);

            if (courrColor.a > 0.1f)
                {  
                countR++;
                //render.monde.SafetyEcrir(index, Color.red);

                render.monde.DrawMarcos(index, 3, Color.green);
                return;
                }
            else
                {
                countV++;
                //render.monde.SafetyEcrir(index, Color.green);
                }
            
            }

        Debug.Log("красный = " + countR + "; зеленых = " + countV + ";");

        render.ChangeMonde();
        }

    Vector3 TransformRayDir(Vector3 pos)
        {
        Vector3 res = (pos -  new Vector3(0.5f, 0.5f, 0.5f)) * 2.0f;
        return res;
        }

    static public Texture2D GetRTPixels(RenderTexture rt)
        {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
        }
    
    /// <summary>
    /// Измениллось значение Тоггле
    /// </summary>
    /// <param name="_nom"></param>
    private void ChangerToggleContiniusMode(Toggle _toggle)
        {

        if (!_toggle.isOn)
            {
            predicatusContirDraw.condition = Predicatus.Condition.GetKey;
            }
        else
            {
            predicatusContirDraw.condition = Predicatus.Condition.GetKeyDonw;
            }  
        
        _toggle.gameObject.GetComponentInChildren<TextToggle>().Toggle();            
        }

    private void ChangerScreenWorld(Toggle _toggle)
        {
        estScreen = toggleScreenGlobal.isOn;
        _toggle.gameObject.GetComponentInChildren<TextToggle>().Toggle();
        }

    private void ChangerEstAdd(Toggle _toggle)
        {
        estAdd = toggleEstAdd.isOn;
        _toggle.gameObject.GetComponentInChildren<TextToggle>().Toggle();
        }
    
    public void MonHandler(float _param)
        {

        }

    void AlphaChanger( float _param)
        {
        pollitra.Color_1.a = sliderAlfa.val;
        }
    }

public class Predicatus
    {
    public enum Condition
        {
        GetKey = 0,
        GetKeyDonw = 1
        }

    public Condition condition = Condition.GetKey;

    /// <summary>
    /// Получить значение
    /// </summary>
    /// <returns></returns>
    public bool GetValue()
        {
        switch (condition)
            {
            case Condition.GetKey:     return Input.GetKey    (KeyCode.Mouse0);
            case Condition.GetKeyDonw: return Input.GetKeyDown(KeyCode.Mouse0);
            default: return false; 
            }

        }
    }
