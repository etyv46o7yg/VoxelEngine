using System;
using UnityEngine;

namespace Mathan
    {
    [System.Serializable]
    public class int3
        {
        [SerializeField]
        public int x, y, z;

        public int multilie { get { return x*y*z;} }

        public int3(int _x, int _y, int _z)
            {
            x = _x;
            y = _y;
            z = _z;
            }

        public int3(Vector3 val)
            {
            x = (int) val.x;
            y = (int) val.y;
            z = (int) val.z;
            }

        public static int3 operator + (int3 a, int3 b)
            {
            int3 res = new int3(a.x + b.x, a.y + b.y, a.z + b.z);
            return res;
            }

        public static int3 operator - (int3 a, int3 b)
            {
            int3 res = new int3(a.x - b.x, a.y - b.y, a.z - b.z);
            return res;
            }

        public static int3 operator * (int3 a, float f)
            {
            int3 res = new int3( Mathf.RoundToInt( a.x * f), Mathf.RoundToInt(a.y * f), Mathf.RoundToInt(a.z * f) );
            return res;
            }

        public static explicit operator Vector3(int3 val)
            {
            Vector3 res = new Vector3( Mathf.FloorToInt(val.x), Mathf.FloorToInt(val.y), Mathf.FloorToInt(val.z) );
            return res;
            }

        public static int3 operator / (int3 a, float f)
            {
            int3 res = new int3(Mathf.RoundToInt(a.x / f), Mathf.RoundToInt(a.y / f), Mathf.RoundToInt(a.z / f));
            return res;
            }

        public static implicit operator int3 (Vector3 v)
            {
            return new int3(v);
            }

        /// <summary>
        /// находится ли точка в куба, ограниченного двумя точками?
        /// </summary>
        /// <param name="min">первая точка куба</param>
        /// <param name="max">вторая точка куба</param>
        /// <returns></returns>
        public bool estEnCube(int3 min, int3 max)
            {
            bool estX = (x >= min.x && x <= max.x);
            bool estY = (y >= min.y && y <= max.y);
            bool estZ = (z >= min.z && z <= max.z);

            return estX && estY && estZ;
            }

        public int GetVaxDim()
            {
            int res = Mathf.Max(x, y, z);
            return res;
            }

        public static Values values;

        public int Mult()
            {
            return x*y*z;
            }

        public override string ToString()
            {
            string res = "(" + x + ";" + y + ";" + z + ")";
            return res;
            }

        public int3 Clamp(int3 min, int3 max)
            {
            int3 res = new int3(x, y, z);

            if(x < min.x)
                {
                res.x = min.x;
                }

            if (x > max.x)
                {
                res.x = max.x;
                }

            //------------------------------

            if (y < min.y)
                {
                res.y = min.y;
                }

            if (y > max.y)
                {
                res.y = max.y;
                }

            //-----------------------------

            if (z < min.z)
                {
                res.z = min.z;
                }

            if (z > max.z)
                {
                res.z = max.z;
                }

            return res;
            }
        
        public int3 Clamp(int min, int max)
            {
            int3 res = new int3(x, y, z);

            if (x < min)
                {
                res.x = min;
                }

            if (x > max)
                {
                res.x = max;
                }

            //------------------------------

            if (y < min)
                {
                res.y = min;
                }

            if (y > max)
                {
                res.y = max;
                }

            //-----------------------------

            if (z < min)
                {
                res.z = min;
                }

            if (z > max)
                {
                res.z = max;
                }

            return res;
            }

        public int3 Clamp(int min, int3 max)
            {
            int3 res = new int3(x, y, z);

            if (x < min)
                {
                res.x = min;
                }

            if (x > max.x)
                {
                res.x = max.x;
                }

            //------------------------------

            if (y < min)
                {
                res.y = min;
                }

            if (y > max.y)
                {
                res.y = max.y;
                }

            //-----------------------------

            if (z < min)
                {
                res.z = min;
                }

            if (z > max.z)
                {
                res.z = max.z;
                }

            return res;
            }

        public class Values
            {
            public int3 one = new int3(1, 1, 1);
            }
        }

    public class int2
        {
        public int x, y;

        public int2(int _x, int _y)
            {
            x = _x;
            y = _y;
            }

        public static int2 operator +(int2 a, int2 b)
            {
            int2 res = new int2(a.x + b.x, a.y + b.y);
            return res;
            }

        public static int2 operator -(int2 a, int2 b)
            {
            int2 res = new int2(a.x - b.x, a.y - b.y);
            return res;
            }

        public static int2 operator *(int2 a, float f)
            {
            int2 res = new int2(Mathf.FloorToInt(a.x * f), Mathf.FloorToInt(a.y * f));
            return res;
            }
        }

    public struct SourseDeLum
        {
        public Vector3 pos;
        public Vector3 color;
        public float   intensivity;
        };
    
    public class КакНазвать
        {
        public static int2 Transform3Dto2D(int3 pos)
            {
            const int u = 256; // текстура 4098х4098 состоит из кусков 256х256, по 16х16
            int z = pos.z;
            int xOffset = z % u;
            int yOffset = z / u;
            int2 offset = new int2(xOffset, yOffset);

            int2 res = new int2(pos.x + u * xOffset, pos.y + u * yOffset);
            return res;
            }

        public static int3 Transform2Dto3D(int2 pos)
            {
            const int u = 256; // текстура 4098х4098 состоит из кусков 256х256, по 16х16
            int3 res = new int3(0, 0, 0);

            int xChounck = pos.x % u;
            int yChounck = pos.y % u;

            return res;
            }

        public static int Transform3Dto1D(int3 id, int3 dim)
            {
            if (id.x < 0 || id.x > dim.x)
                {
                return -1;
                }

            if (id.y < 0 || id.y > dim.y)
                {
                return -1;
                }

            if (id.z < 0 || id.z > dim.z)
                {
                return -1;
                }

            int i = id.z * dim.x * dim.y + id.y * dim.y + id.x;
            return i;
            }

        public static int3 Transform1Dto3D(int pos, int3 dim)
            {
            if (pos >= dim.x * dim.y * dim.z || pos < 0)
                {
                return new int3(-1, -1, -1);
                }

            int idx = pos;

            int z = Mathf.FloorToInt( (float) idx / (float)(dim.x * dim.y));
            idx -= (z * dim.x * dim.y);
            int y = Mathf.FloorToInt((float)idx / (float) dim.x);
            int x = idx % dim.x;
            return new int3(x, y, z);
            }
        }
    }
