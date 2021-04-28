using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIRayCast : MonoBehaviour
    {    
    public List<UICorner> uICorners;
    // Start is called before the first frame update
    void Start()
        {
        var rects = FindObjectsOfType<RectTransform>().ToList();

        uICorners = new List<UICorner>();
        foreach (var item in rects)
            {
            Vector3 [] corn = new Vector3 [4];
            item.GetWorldCorners(corn);
            uICorners.Add( new UICorner(corn[0], corn [2], item) );
            }
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    [System.Serializable]
    public struct UICorner
        {
        Vector2 pos_min, pos_max;
        public RectTransform tr;

        public UICorner(Vector2 _posMin, Vector2 _posMax, RectTransform _tr)
            {
            pos_min = _posMin;
            pos_max = _posMax;
            tr = _tr;
            }

        public bool estDedand(Vector2 screenPos)
            {
            if(screenPos.x > pos_min.x && screenPos.x < pos_max.x && screenPos.y > pos_min.y && screenPos.y < pos_max.y )
                {
                return true;
                }
            return false;
            }
        }
    
    public struct UIRayCastResult
        {
        public RectTransform rt;
        public Vector2 localPos;
        }

    public List<RectTransform> RayCast(Vector2 _worldPos)
        {
        List<RectTransform> res = new List<RectTransform>();

        foreach (var item in uICorners)
            {
            if (item.estDedand(_worldPos) && item.tr.gameObject.activeSelf)
                {
                res.Add(item.tr);
                }
            }

        return res;
        }
    }
