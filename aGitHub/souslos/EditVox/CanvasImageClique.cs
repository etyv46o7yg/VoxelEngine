using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// обработка указателя над изображением
/// </summary>
public class CanvasImageClique : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,    IBeginDragHandler, IDragHandler, IEndDragHandler
    {
    RawImage cetteImage;
    public string nomElement = "";
    public bool estOverImage = false;
    public bool estPaintirProcces = false;
    RectTransform rect;
    public enum TypeUIImage
        {
        ImagetextureSlice = 0,
        Palithre          = 1,
        RenderVoxelImage  = 2
        }

    public delegate void CliqueHaut(Vector3 min, Vector3 max);
    public delegate void CliqueBas ();

    /// <summary>
    /// началось рисование
    /// </summary>
    public event CliqueBas ACommoncerClique;

    /// <summary>
    /// занончилось рисование
    /// </summary>
    public event CliqueHaut AFinirClique;   

    public TypeUIImage typeCetteElement = TypeUIImage.ImagetextureSlice;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
        {
        cetteImage = GetComponent<RawImage>();
        rect = GetComponent<RectTransform>();

        }

    // Update is called once per frame
    void Update()
        {

        }
   

    public void OnPointerOver(PointerEventData eventData)
        {       

        Debug.Log("я НАД " + gameObject.name);
        }

    public static Vector2 GetLocalUICoordinates(RectTransform _rect, Vector2 pos)
        {
        Vector3 worldPos = _rect.position;
        Vector2 beeAss   = _rect.rect.size;
        Vector2 localCoord = pos - new Vector2(worldPos.x, worldPos.y) + beeAss * 0.5f;
        return localCoord;
        }

    /// <summary>
    /// нахождение UV координат по мировым
    /// </summary>
    /// <param name="pos">экранные мировые координаты</param>
    /// <returns>значение </returns>
    public Vector2 GetLocalUICoordinates_2(Vector2 pos)
        {
        Vector3 [] corn = new Vector3 [4];
        rect.GetWorldCorners(corn);

        Vector2 minCorn = new Vector2( corn[0].x, corn[0].y);
        Vector2 maxCorn = new Vector2( corn[2].x, corn[2].y);
        Vector2 delta = maxCorn - minCorn;

        Vector2 relatifPosition = (pos - minCorn) / delta;
        relatifPosition = new Vector2(relatifPosition.x * cetteImage.texture.width, relatifPosition.y * cetteImage.texture.height );

        return relatifPosition;
        }


    public Vector2 TextureSpaceCoord(Vector3 worldPos)
        {
        
        float ppu = sprite.pixelsPerUnit;

        // Local position on the sprite in pixels.
        Vector2 localPos = transform.InverseTransformPoint(worldPos) * ppu;

        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(sprite.rect.x, sprite.rect.y) + sprite.pivot;
        Vector2 texSpaceCoord = texSpacePivot + localPos;

        return texSpaceCoord;
        

        //Physics.RaycastAll(worldPos, Ve)
        }
        

    /// <summary>
    /// мыжка отжата над изображением
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
        {
        if (eventData.button == PointerEventData.InputButton.Left)
            {
            AFinirClique(Vector3.zero, Vector3.zero);
            }
        }

    public void OnPointerDown(PointerEventData eventData)
        {

        if (eventData.button == PointerEventData.InputButton.Left)
            {
            ACommoncerClique();
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

    public void OnBeginDrag(PointerEventData eventData)
        {

        }

    public void OnDrag(PointerEventData eventData)
        {

        }

    public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
