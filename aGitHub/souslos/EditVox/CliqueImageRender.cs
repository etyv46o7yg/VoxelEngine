using Mathan;
using Render;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CliqueImageRender : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
    private Vector3 minPosPaint;
    private Vector3 maxPosPaint;
    Minimal_Render render;
    public bool estOverImage = false;

    RawImage cetteImage;
    RectTransform rect;

    public delegate void PaintStatus(int3 min, int3 max);
    public delegate void Neunral();

    public event PaintStatus FinirPaint;
    public event Neunral     CommorcerPaint;

    public Minimal_Render.CoursorInfo3D coursorInfo3d;

    // Start is called before the first frame update
    void Start()
        {
        render = EditorPrince.instance.render;
        cetteImage = GetComponent<RawImage>();
        rect       = GetComponent<RectTransform>();

        Reset();
        }

    // Update is called once per frame
    void Update()
        {
        if (estOverImage)
            {
            Vector2 texPosCoursor = GetLocalUICoordinates_2(Input.mousePosition);
            coursorInfo3d = EditorPrince.instance.render.GetPosMouse3d( texPosCoursor );
            render.posMouse3D.isHaseInfo = true;
            coursorInfo3d.pos2d = texPosCoursor;

            if (Input.GetMouseButtonDown(0))
                {
                CommorcerPaint();
                }

            if ( Input.GetMouseButton(0) )
                {
                EditorPrince.instance.CliqueOnRenderImage();
                }
            }
        else
            {
            render.posMouse3D.isHaseInfo = false;
            }

        if (render.posMouse3D.pos3d.x > -1.0f && estOverImage && Input.GetMouseButton(0) )
            {
            minPosPaint = Vector3.Min(minPosPaint, render.posMouse3D.pos3d);
            maxPosPaint = Vector3.Max(maxPosPaint, render.posMouse3D.pos3d);
            }
        
        }

    public Vector2 GetLocalUICoordinates_2(Vector2 pos)
        {
        Vector3 [] corn = new Vector3 [4];
        rect.GetWorldCorners(corn);

        Vector2 minCorn = new Vector2(corn [0].x, corn [0].y);
        Vector2 maxCorn = new Vector2(corn [2].x, corn [2].y);
        Vector2 delta = maxCorn - minCorn;

        Vector2 relatifPosition = (pos - minCorn) / delta;
        relatifPosition = new Vector2(relatifPosition.x * cetteImage.texture.width, relatifPosition.y * cetteImage.texture.height);

        return relatifPosition;
        }

    private void Reset()
        {
        minPosPaint = Vector3.positiveInfinity;
        maxPosPaint = Vector3.negativeInfinity;
        }

    public void OnPointerDown(PointerEventData eventData)
        {
        if (eventData.button == PointerEventData.InputButton.Left)
            {
            
            }
        }

    public void OnPointerUp(PointerEventData eventData)
        {
        if (eventData.button == PointerEventData.InputButton.Left)
            {
            Vector3 radVec = new Vector3(EditorPrince.instance.sliderSizeBrush.val, EditorPrince.instance.sliderSizeBrush.val, EditorPrince.instance.sliderSizeBrush.val) + Vector3.one * 5;
            minPosPaint -= radVec;
            maxPosPaint += radVec;

            //Debug.Log(minPosPaint + " " + maxPosPaint);
            //minPosPaint = (Vector3) ( (int3) minPosPaint).Clamp(0, 255);
            //maxPosPaint += new  Vector3(0, 10, 0);
            //Debug.Log(maxPosPaint);

            int3 delta = maxPosPaint - minPosPaint;
            //render.DrawBox(minPosPaint, maxPosPaint, EditorPrince.instance.pollitra.Color_1);
            FinirPaint( minPosPaint, maxPosPaint );
            Reset();                        
            
            }
        }

    public void OnPointerExit(PointerEventData eventData)
        {
        estOverImage = false;
        }

    public void OnPointerEnter(PointerEventData eventData)
        {
        estOverImage = true;
        }
    }
