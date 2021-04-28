using Mathan;
using System;
using UnityEngine;

/// <summary>
/// рендер без освещения
/// </summary>
namespace Render
    {
    [System.Serializable]
    public class SauverMonde
        {
        [SerializeField]
        public float [] r;
        public float [] g;
        public float [] b;
        public float [] a;

        public SauverMonde(Vector4[] _colors)
            {
            GC.Collect();

            r = new float[_colors.Length];
            g = new float [_colors.Length];
            b = new float [_colors.Length];
            a = new float [_colors.Length];
            
            
            for (int i = 0; i < r.Length; i++)
                {
                r [i] = _colors [i].x;
                g [i] = _colors [i].y;
                b [i] = _colors [i].z;
                a [i] = _colors [i].w;
                }
            
            }
        
        public Vector4[] GetArrayValue()
            {
            Vector4[] res = new Vector4[r.Length];

            for (int i = 0; i < res.Length; i++)
                {
                res[i] = new Vector4(r[i], g[i], b[i], a[i]);
                }

            return res;
            }


        }
    
    public class LightMondeCaptupe
        {

        }

    public class PieceDeMonde
        {
        public int3 posMin, posMax;
        public ColorSavePiece[] colors;

        public PieceDeMonde(ColorSavePiece[] _colors, Vector3 _minPos, Vector3 _maxPos)
            {
            colors = _colors;
            posMin = _minPos;
            posMax = _maxPos;
            }

        /// <summary>
        /// получить размер объекта в байтах
        /// </summary>
        /// <returns>размер в байтах</returns>
        public int GetSizeInByte()
            {
            int sizeElement = 0;
            unsafe
                {
                sizeElement = System.Runtime.InteropServices.Marshal.SizeOf(typeof(ColorSavePiece));
                }

            if (sizeElement == 0)
                {
                throw new Exception("Не получилось узнать тип структуры");
                }

            int res = colors.Length * sizeElement;

            //Debug.Log("размер элемента = " + sizeElement);

            return res;
            }

        public float GetSizeInMegaByte()
            {
            float res = (float) GetSizeInByte() / 1000000.0f;
            return res;
            }
        }
    }