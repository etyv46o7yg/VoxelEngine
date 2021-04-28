using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopologieClasses;
using UnityEngine;
using UnityEngine.UI;

public class DistrExperemental : MonoBehaviour
    {
    public LayerMask mask;
    public GameObject markos;
    public GameObject courrObj;

    public RawImage rawImage;
    RenderTexture rt;

    public UnityEngine.Object prefabMark;
    List<GameObject> listMarkos;

    ChaderSysteme chader;
    // Start is called before the first frame update
    void Start()
        {
        listMarkos = new List<GameObject>();

        Vector3 pl_a, pl_b, pl_c;
        pl_a = Vector3.zero;
        pl_b = Vector3.up + Vector3.left;
        pl_c = Vector3.forward;
        PlaneTroisPoint pl = new PlaneTroisPoint(pl_a, pl_b, pl_c);


        AddMark(pl_a, Color.black);
        AddMark(pl_b, Color.black);
        AddMark(pl_c, Color.black);


        VertexMesh A = new VertexMesh(new Vector3(2, -1, -3), 0);
        VertexMesh B = new VertexMesh(new Vector3(0, -1, 2), 1);
        VertexMesh C = new VertexMesh(new Vector3(-2, 4, 1), 2);
        TringleMesh tr = new TringleMesh(A, B, C);
        //fairQuelque_4(pl, tr);
        }

    void DestrieMarks()
        {
        foreach (var item in listMarkos)
            {
            Destroy(item);
            }

        listMarkos.Clear();
        }

    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyDown(KeyCode.Space) )
            {
            StepPointMark(courrObj.GetComponent<MeshFilter>().mesh, courrObj.transform.position);
            }

        if (Input.GetMouseButtonDown(0))
            {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, mask))
                {
                Vector3 localPos = hit.point - hit.collider.gameObject.transform.position;

                PlaneTroisPoint pl = new PlaneTroisPoint(localPos - Vector3.forward , localPos + Vector3.right , localPos - Vector3.up* 1.0f + Vector3.forward);

                FairQuelque_3(hit.collider.gameObject, hit.point, pl );
                //FairQuelque_2(hit.collider.gameObject, hit.point, pl );
                }
            }

        if (Input.GetKeyDown(KeyCode.R) )
            {
            DestrieMarks();
            }
        }

    void FairQuelque(GameObject obj, Vector3 point)
        {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

        Vector3 localPos =  point - obj.transform.position;

        courrObj = obj;

        int count = 0;

        List<Vector3> nouveauVerts = new List<Vector3>();

        var vertexes = mesh.vertices;

        for (int i = 0; i < vertexes.Length; i++)
            {
            Vector3 courr = vertexes[i];

            if (courr.x < localPos.x)
                {
                count++;
                vertexes[i] = localPos;
                }
            }

        Vector3 posB = localPos + Vector3.forward;
        Vector3 posC = localPos + Vector3.left;

        meshFilter.mesh.SetVertices(vertexes.ToList() );
        meshCollider.sharedMesh = meshFilter.mesh;

        Debug.Log(count);
        }

    void FairQuelque_2(GameObject obj, Vector3 point, PlaneTroisPoint _plane)
        {       
        MeshFilter   meshFilter   = obj.GetComponent<MeshFilter>();
        Mesh         mesh         = meshFilter.mesh;
        Mesh         nouveauMesh  = new Mesh();
        MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

        Vector3 localPos = point - obj.transform.position;

        courrObj = obj;

        List<TringleMesh> tringles = TringleMesh.GetTringleMeshListFromArrayIndexes(mesh.triangles, mesh);
        List<int> inCorrectIndex = new List<int>();

        PlaneTroisPoint plane = _plane;

        for (int i = 0; i < mesh.triangles.Length; i++)
            {
            /*
            if (mesh.vertices[ mesh.triangles[i] ].x > localPos.x)
                {
                inCorrectIndex.Add(mesh.triangles[i]);
                }
            */
            if (plane.estHautPoint(mesh.vertices[mesh.triangles[i]]))
                {
                inCorrectIndex.Add(mesh.triangles[i]);
                }
            }

        List<Vector3> nouveauVertxes  = new List<Vector3>();
        List<Vector3> nouveauNormals  = new List<Vector3>();
        List<Vector2> nouveauUV       = new List<Vector2>();     
        List<int> nouveauVertxesIndex = new List<int>();
        List<VertexMesh> vertexMeshes = new List<VertexMesh>();

        List<int> vertexIndexRing = new List<int>();

        int count = 0;
        for(int i = 0; i < mesh.vertices.Length; i++)
            {
            vertexMeshes.Add(new VertexMesh(mesh.vertices[i], mesh.uv[i], i) );

            if (!plane.estHautPoint(mesh.vertices[i]))
                {
                nouveauVertxes.Add(mesh.vertices[i]);
                nouveauUV.Add(mesh.uv[i]);
                nouveauNormals.Add(mesh.normals[i]);
                nouveauVertxesIndex.Add(count);                
                count++;
                }
            else
                {
                nouveauVertxesIndex.Add(-1);
                }
            }

        Debug.Log("всего вершин = " + mesh.vertices.Length + " оставнш. вершины = " + nouveauVertxes.Count);
        Debug.Log("корр. инд вершин = " + Topologie.GetStringFromArray(nouveauVertxesIndex) );

        //получиил список некорректных индектос треугольников
        List<TringleMesh> nouveauTring = new List<TringleMesh>();

        for (int i = 0; i < tringles.Count; i++)
            {
            bool avoirIncorrIndex = false;
            for (int j = 0; j < inCorrectIndex.Count; j++)
                {
                if (tringles[i].EstAvoirIndex(inCorrectIndex[j]))
                    {
                    avoirIncorrIndex = true;
                    }
                
                }

            if (!avoirIncorrIndex)
                {
                nouveauTring.Add(tringles[i]);
                }            
            }


        var trings = TringleMesh.GetTringlesFromListTringles(nouveauTring);

        for (int i = 0; i < trings.Length; i++)
            {
            trings[i] = nouveauVertxesIndex[trings[i]];
            }
        /*
        nouveauMesh.vertices  = nouveauVertxes.ToArray();
        nouveauMesh.triangles = trings;
        nouveauMesh.uv        = mesh.uv;
        nouveauMesh.normals   = mesh.normals;
        */
        //meshFilter.mesh.Clear();

        //Debug.Log("Нов. треуг. = " + Topologie.GetStringFromArray(trings) );
        //Debug.Log("всего новых вершин = " + nouveauVertxes.Count);

        meshFilter.mesh.SetTriangles(trings, 0);
        meshFilter.mesh.SetVertices(nouveauVertxes);
        meshFilter.mesh.SetUVs(0, nouveauUV);
        meshFilter.mesh.SetNormals(nouveauNormals);
        //meshFilter.mesh = nouveauMesh;

        Destroy(meshCollider);
        var coll = obj.AddComponent<MeshCollider>();
        coll.convex = true;
        }

    void FairQuelque_3(GameObject obj, Vector3 point, PlaneTroisPoint _plane)
        {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        Vector3 localPos = point - obj.transform.position;

        courrObj = obj;

        List<TringleMesh> tringles = TringleMesh.GetTringleMeshListFromArrayIndexes(mesh.triangles, mesh);

        List<TringleMesh> nouveauTringList  = new List<TringleMesh>();
        nouveauTringList.AddRange(tringles);

        List<Vector3>     nouveauVertexList = new List<Vector3>();
        nouveauVertexList.AddRange(mesh.vertices);

        List<Vector2> nouveauUVList = new List<Vector2>();
        nouveauUVList.AddRange(mesh.uv);

        List<Vector3> nouveauNormList = new List<Vector3>();
        nouveauNormList.AddRange(mesh.normals);

        List<int> inCorrectIndex = new List<int>();

        Debug.Log("станые ЮВ = " + Topologie.GetStringFromArray(mesh.uv) );

        //PlaneTroisPoint plane = new PlaneTroisPoint(localPos + Vector3.forward, localPos + Vector3.right, localPos + Vector3.up * 0.5f);

        List<Vector3> points = new List<Vector3>();

        int count = 0;

        List<Color> listColor = new List<Color>();
        listColor.Add(Color.blue);
        listColor.Add(Color.black);
        listColor.Add(Color.cyan);
        listColor.Add(Color.red);
        listColor.Add(Color.green);
        listColor.Add(Color.grey);
        listColor.Add(Color.white);

        int bias = mesh.vertices.Length, x = 0, counter_01 = 0;

        foreach (var item in tringles)
            {
            counter_01++;
            //var res = item.PointInterreption(plane);

            var res = item.DivideCa(_plane);

            Debug.Log(item.uvs[0] + " " + item.uvs[1] + " " + item.uvs[2]);

            if (res.ps.Count == 2) //если есть пересечение, т.е число пересеченных ребер треугольника равно 2
                {
                //чистое ребро, не пересеченное плоскостью
                EdgeTringle divEdge = new EdgeTringle(0, 0, Vector3.zero, Vector3.zero, Vector2.zero, Vector2.zero);
                
                var nouveauTring = item.GetDividedTringles(_plane, localPos, bias, ref x, ref divEdge);

                //Debug.Log( "uvA = " + divEdge.A.uv + " uvB = " + divEdge.B.uv);

                bias += x;
                Color currCol = listColor[count % listColor.Count];

                nouveauVertexList.Add(divEdge.A.vertex);
                nouveauVertexList.Add(divEdge.B.vertex);

                nouveauUVList.Add(divEdge.A.uv);
                nouveauUVList.Add(divEdge.B.uv);

                nouveauNormList.Add(divEdge.A.normal);
                nouveauNormList.Add(divEdge.B.normal);

                foreach (var atis in nouveauTring)
                    {
                    Debug.Log( "A " + atis.verABC[0].uv + " B " + atis.verABC[1].uv + " C " + atis.verABC[2].uv );
                    //Debug.Log("полож. пл. 1 = " + _plane.posPointSurPlane(atis.vertex[0]) + " полож. пл. 2 = " + _plane.posPointSurPlane(atis.vertex[1]) + " полож. пл. 3 = " + _plane.posPointSurPlane(atis.vertex[2])  );

                    nouveauTringList.Add(atis);
                    
                    //atis.ShowCa(obj.transform.position, currCol);
                    count++;
                    }
                    
                
                }            
            
            foreach (var piece in res.ps)
                {
                AddMark(obj.transform.position + piece.Item2, Color.blue);
                }
            }


        meshFilter.mesh.SetVertices  (nouveauVertexList);
        meshFilter.mesh.SetUVs       (0, nouveauUVList);
        meshFilter.mesh.SetNormals   (nouveauNormList);
        meshFilter.mesh.SetTriangles (TopologieClasses.TringleMesh.GetTringlesFromListTringles(nouveauTringList), 0);
        

        Debug.Log("UVs " + Topologie.GetStringFromArray(mesh.uv));

        List<int> mouveIndexTringle = new List<int>();

        List<TringleMesh> postDeleteTrings = new List<TringleMesh>();

        //plane.MovePlaneVerticale(-0.05f);

        Debug.Log("кооррд. = " + localPos);

        foreach (var item in nouveauTringList)            
            {
            int countPoint = _plane.numeroVertexTringleHautPlane(item);

            Debug.Log(item.ToString());

            if (countPoint > 0)
                {
                postDeleteTrings.Add(item);
                item.ShowCa(obj.transform.position, Color.white);
                }

            
            }

        foreach (var item in postDeleteTrings)
            {
            Debug.Log("новые треуг. " + item.ToString() );
            }


        meshFilter.mesh.SetTriangles(TringleMesh.GetTringlesFromListTringles(postDeleteTrings), 0);
        }

    void fairQuelque_4(PlaneTroisPoint plane, TringleMesh tringleMesh)
        {
        Vector3 localPos = Vector3.zero;

        EdgeTringle edge = tringleMesh.edge[0];
        int count = 0;

        var res = tringleMesh.GetDividedTringles(plane, localPos, 0, ref count, ref edge );

        tringleMesh.ShowCa(Vector3.zero, Color.green);

        Debug.Log(res.Count);
        Debug.Log(edge.A.vertex + " " + edge.B.vertex);
        Debug.Log(" дист. А = " + plane.DistanseToPoint(edge.A.vertex) + " дист. B = " + plane.DistanseToPoint(edge.B.vertex) );

        foreach (var item in res)
            {
            string result = "";

            item.ShowCa(Vector3.zero, Color.red);

            foreach (var atis in item.vertex)
                {
                result += plane.posPointSurPlane(atis) + " ";
                }

            Debug.Log(result);
            }
        }

    public void AddMark(Vector3 pos, Color color)
        {
        var go = Instantiate(prefabMark, pos, Quaternion.identity) as GameObject;
        go.GetComponent<MeshRenderer>().material.color = color;
        listMarkos.Add(go);        
        }   

    List<Vector3> GetRing(Vector3[] vert, float x)
        {
        List<Vector3> res = new List<Vector3>();

        return res;
        }

    (Vector3, Vector3) GetMinMaxPoint(Mesh mesh)
        {
        return (Vector3.zero, Vector3.zero);
        }

    void StepPointMark(Mesh mesh, Vector3 local)
        {
        StartCoroutine(StepMark(mesh, local));
        }

    public IEnumerator StepMark(Mesh mesh, Vector3 loco)
        {
        var vert = mesh.vertices;
        int i = 0;
        while (i < vert.Length)
            {
            markos.transform.position = vert[i] + loco;
            i++;
            yield return new WaitForSeconds(0.5f);
            }
        }
    }

namespace TopologieClasses
    {
    /// <summary>
    /// класс полных данные вершины меша
    /// </summary>
    public class VertexMesh : IEquatable<VertexMesh>
        {
        public Vector3 vertex;
        public Vector3 normal;
        public Vector2 uv;
        public int index;

        /// <summary>
        /// индексы соседних ребер
        /// </summary>
        public int[] voisinesEdges;

        public VertexMesh(Vector3 _vertex, Vector2 _uv, int _index)
            {
            vertex = _vertex;
            uv = _uv;
            index = _index;
            }

        public VertexMesh(Vector3 _vertex, int _index)
            {
            vertex = _vertex;
            index = _index;
            }

        public bool Equals(VertexMesh other)
            {
            if (other.index == index)
                {
                return true;
                }
            else
                {
                return false;
                }
            }
        }


    /// <summary>
    /// класс для задания секущей плоскости
    /// </summary>
    public class PlaneTroisPoint
        {
        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }
        public Vector3 C { get; private set; }
        public Vector3 D { get; private set; }

        /// <summary>
        /// нормаль к плоскости
        /// </summary>
        public Vector3 normal { get; private set; }

        float koeffA, koeffB, koeffC, koeffD;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_A">первая точка плоскости</param>
        /// <param name="_B">вторая точка плоскости</param>
        /// <param name="_C">третья точка плоскости</param>
        public PlaneTroisPoint(Vector3 _A, Vector3 _B, Vector3 _C)
            {
            Vector3 t1 = _A;
            Vector3 t2 = _B;
            Vector3 t3 = _C;

            //вычисляются коэффициенты для уравнения плоскости
            koeffA = t1.y * (t2.z - t3.z);// + t2.y* (t3.z - t1.z) + t3.y* (t1.z - t2.z); 
            koeffB = t1.z * (t2.x - t3.x) + t2.z * (t3.x - t1.x) + t3.z * (t1.x - t2.x);
            koeffC = t1.x * (t2.y - t3.y) + t2.x * (t3.y - t1.y) + t2.x * (t1.y - t2.y);
            koeffD = -(t1.x * (t2.y * t3.z - t3.y * t2.z) + t2.x * (t3.y * t1.z - t1.y * t3.z) + t3.x * (t1.y * t2.z - t2.y * t1.z));

            normal = new Vector3(koeffA, koeffB, koeffC);
            }

        /// <summary>
        /// находтся ли точка выше плоскоти?
        /// </summary>
        /// <param name="point">точка для которой вычисляется</param>
        /// <returns></returns>
        public bool estHautPoint(Vector3 point)
            {
            float f = koeffA * point.x + koeffB * point.y + koeffC * point.z + koeffD;

            if (f > 0.0f)
                {
                return false;
                }
            return true;
            }

        public float AndleBitwin(Vector3 vector)
            {
            return Vector3.AngleBetween(normal, vector);
            }

        public float DistanseToPoint(Vector3 point)
            {
            float a = Mathf.Abs(koeffA * point.x + koeffB * point.y + koeffC * point.z + koeffD);
            float b = normal.magnitude;

            return a / b;
            }

        /// <summary>
        /// подвинуть плоскость вверх
        /// </summary>
        /// <param name="f"></param>
        public void MovePlaneVerticale(float f)
            {
            koeffD += f;
            }

        /// <summary>
        /// точке пересечения отрезка и плоскости
        /// </summary>
        /// <param name="_A">первая точка отрезка</param>
        /// <param name="_B">вторая точка отрезка</param>
        /// <param name="res">точке пересечения</param>
        /// <returns>есть ли пересечение</returns>
        public bool InterectionPointLine(Vector3 _A, Vector3 _B, ref Vector3 res)
            {
            bool estHautA, estHautB;

            estHautA = estHautPoint(_A);
            estHautB = estHautPoint(_B);

            if (estHautA == estHautB)
                {
                return false;
                }

            float distA = DistanseToPoint(_A);
            float distB = DistanseToPoint(_B);

            Vector3 dir = _B - _A;

            Vector3 result = _A + dir * distA / (distB + distA);
            res = result;

            return true;
            }

        /// <summary>
        /// точка пересечения данной плосоксти и ребра
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool InterectionPointLine(EdgeTringle edge, ref Vector3 res, ref Vector2 _uv)
            {
            bool estHautA, estHautB;

            Vector3 A = edge.A.vertex;
            Vector3 B = edge.B.vertex;

            Vector2 A_uv = edge.A.uv;
            Vector2 B_uv = edge.B.uv;

            estHautA = estHautPoint(A);
            estHautB = estHautPoint(B);

            if (estHautA == estHautB)
                {
                return false;
                }

            float distA = DistanseToPoint(A);
            float distB = DistanseToPoint(B);

            Vector3 dir = B - A;
            Vector2 dir_uv = B_uv - A_uv;

            float kratos = distA / (distB + distA);

            Vector3 result = A + dir * kratos;
            res = result;
            _uv = A_uv + dir_uv * kratos;

            return true;
            }
        
        public bool estTringleHautPlane(TringleMesh tringle, Vector3 bias)
            {
            //Debug.Log( (tringle.vertex[0] + bias) + " " + (tringle.vertex[1] + bias) + " " + (tringle.vertex[2] + bias) );

            if (estHautPoint(tringle.vertex[0] + bias) && estHautPoint(tringle.vertex[1] + bias) && estHautPoint(tringle.vertex[2] + bias))
                {
                return true;
                }

            return false;
            }
        
        public int numeroVertexTringleHautPlane(TringleMesh tringle)
            {
            int resA = posPointSurPlane(tringle.vertex[0] );
            int resB = posPointSurPlane(tringle.vertex[1] );
            int resC = posPointSurPlane(tringle.vertex[2] );

            if (resA < 0 || resB < 0 || resC < 0)
                {
                return -1;
                }
            else
                {
                return 1;
                }
            //Debug.Log(resA.ToString() + " " + resB.ToString() + " " + resC.ToString() );

            return resA + resB + resC;
            }

        public int posPointSurPlane(Vector3 point)
            {
            float fA = koeffA * point.x + koeffB * point.y + koeffC * point.z + koeffD;

            if (Mathf.Abs(fA) < 0.1f)
                {
                return 0;
                }

            if (fA > 0)
                {
                return 1;
                }
            else
                {
                return -1;
                }
            }
        }

    public static class Topologie
        {
        public static string GetStringFromArray<T>(List<T> list)
            {
            string res = "";
            for (int i = 0; i < list.Count; i++)
                {
                res += list[i].ToString() + " ";
                }
            return res;
            }

        public static string GetStringFromArray<T>( T[] list)
            {
            string res = "";
            for (int i = 0; i < list.Length; i++)
                {
                res += list[i].ToString() + " ";
                }
            return res;
            }

        public static void SetMesh(GameObject go, Mesh mesh)
            {
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.GetComponent<MeshCollider>().sharedMesh = mesh;
            }

        public static Mesh Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
            {
            var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
            var mesh = new Mesh
                {
                vertices = new[] { vertex0, vertex1, vertex2 },
                normals = new[] { normal, normal, normal },
                uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) },
                triangles = new[] { 0, 1, 2 }
                };
            return mesh;
            }

        public static Mesh Quad(Vector3 origin, Vector3 width, Vector3 length)
        {
            var normal = Vector3.Cross(length, width).normalized;
            var mesh = new Mesh
            {
                vertices = new[] { origin, origin + length, origin + length + width, origin + width },
                normals = new[] { normal, normal, normal, normal },
                uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
                triangles = new[] { 0, 1, 2, 0, 2, 3 }
            };
            return mesh;
        }
    }

    /// <summary>
    /// треугольник меша
    /// </summary>
    public class TringleMesh
        {
        public int[] index          { get; private set; }
        public EdgeTringle[] edge   { get; private set; }
        public Vector3[] vertex     { get; private set; }
        public Vector2[] uvs        { get; private set; }
        public VertexMesh[] verABC  { get; private set; }

        public TringleMesh(int a, int b, int c, Mesh mesh)
            {
            index = new int[3];
            index[0] = a;
            index[1] = b;
            index[2] = c;

            vertex = new Vector3[3];
            vertex[0] = mesh.vertices[a];
            vertex[1] = mesh.vertices[b];
            vertex[2] = mesh.vertices[c];

            uvs    = new Vector2[3];
            uvs[0] = mesh.uv[0];
            uvs[1] = mesh.uv[1];
            uvs[2] = mesh.uv[2];

            edge = new EdgeTringle[3];
            edge[0] = new EdgeTringle(a, b, vertex[0], vertex[1], uvs[0], uvs[1]);
            edge[1] = new EdgeTringle(b, c, vertex[1], vertex[2], uvs[1], uvs[2]);
            edge[2] = new EdgeTringle(c, a, vertex[2], vertex[0], uvs[2], uvs[0]);

            verABC = new VertexMesh[3];
            verABC[0] = new VertexMesh(vertex[0], uvs[0], index[0] );
            verABC[1] = new VertexMesh(vertex[1], uvs[1], index[1]);
            verABC[2] = new VertexMesh(vertex[2], uvs[2], index[2]);
            }

        public bool estHaut { get; private set; }

        public TringleMesh(VertexMesh _A, VertexMesh _B, VertexMesh _C)
            {
            index = new int[3];
            index[0] = _A.index;
            index[1] = _B.index;
            index[2] = _C.index;

            vertex = new Vector3[3];
            vertex[0] = _A.vertex;
            vertex[1] = _B.vertex;
            vertex[2] = _C.vertex;

            edge = new EdgeTringle[3];
            edge[0] = new EdgeTringle(_A, _B);
            edge[1] = new EdgeTringle(_B, _C);
            edge[2] = new EdgeTringle(_C, _A);

            verABC = new VertexMesh[3];
            verABC[0] = _A;
            verABC[1] = _B;
            verABC[2] = _C;

            estHaut = true;
            }

        /// <summary>
        /// разразать сей треуголник и вернуть результат
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public DivideInfo DivideCa(PlaneTroisPoint plane)
            {
            Vector3 res = Vector3.zero;
            Vector2 res_uv = Vector2.zero;

            DivideInfo info = new DivideInfo();

            if (plane.InterectionPointLine(edge[0], ref res, ref res_uv))
                {
                info.ps.Add( (edge[0], res, res_uv) );
                }

            if (plane.InterectionPointLine(edge[1], ref res, ref res_uv))
                {
                info.ps.Add((edge[1], res, res_uv));
                }

            if (plane.InterectionPointLine(edge[2], ref res, ref res_uv))
                {
                info.ps.Add((edge[2], res, res_uv));
                }

            return info;
            }

        /// <summary>
        /// подразделить треугольник плоскостью и получить список новых треугольников
        /// </summary>
        /// <param name="plane">плоскость деления</param>
        /// <param name="biasVertexIndex">смещение вершин меша</param>
        /// <returns></returns>
        public List<TringleMesh> GetDividedTringles(PlaneTroisPoint plane, Vector3 localPos, int biasVertexIndex, ref int countAjouteVertex, ref EdgeTringle _edge)
            {
            List<TringleMesh> tringles = new List<TringleMesh>();
            var res = DivideCa(plane);

            if (res.ps.Count != 2)
                {
                Debug.Log("нулевой разрез");
                return tringles;
                }

            VertexMesh divA = new VertexMesh(res.ps[0].Item2, res.ps[0].Item3, biasVertexIndex);
            VertexMesh divB = new VertexMesh(res.ps[1].Item2, res.ps[1].Item3, biasVertexIndex + 1);

            

            //Debug.Log(res.ps[0].Item3 + " " + res.ps[1].Item3);

            EdgeTringle clearEdge        = (from item in edge   where !item.Equals(res.ps[0].Item1) && !item.Equals(res.ps[1].Item1) select item).First() ;            
            //EdgeTringle dividerEdge      = new EdgeTringle(biasVertexIndex, biasVertexIndex + 1, res.ps[0].Item2, res.ps[1].Item2);  //это новое ребро в треугольнике
            EdgeTringle firstEdgeDiriver = (from item in edge   where !item.Equals(clearEdge) select item).First();
            EdgeTringle secndEdgeDiriver = (from item in edge   where !item.Equals(clearEdge) && !item.Equals(firstEdgeDiriver) select item).First();
            VertexMesh opposite =          (from item in verABC where !item.Equals(clearEdge.A) && !item.Equals(clearEdge.B) select item).First();
            VertexMesh firstDivVertex, secondDivVertex, usedVertexClerEdge, notUserVertexClerEdge;            
            countAjouteVertex = 2;

            Vector3 norm = Vector3.Cross(clearEdge.B.vertex - clearEdge.A.vertex, divB.vertex - divA.vertex);
            divA.normal = norm;
            divB.normal = norm;

            EdgeTringle dividerEdge = new EdgeTringle(divA, divB);
            _edge = dividerEdge;

            usedVertexClerEdge = clearEdge.A;

            firstDivVertex = dividerEdge.A;

            if (secndEdgeDiriver.A.index == clearEdge.A.index || secndEdgeDiriver.A.index == clearEdge.B.index)
                {
                usedVertexClerEdge = secndEdgeDiriver.A;
                }
            else
                {
                usedVertexClerEdge = secndEdgeDiriver.B;
                }


            Vector3 ostatniVertex = Vector3.zero;


            int indexCommonVertex = FindCommonVertexIndex(res.ps[0].Item1, res.ps[1].Item1);
            if (indexCommonVertex != -1)
                {
                ostatniVertex = vertex[indexCommonVertex];
                }
            
            tringles.Add(new TringleMesh(clearEdge.A, clearEdge.B, firstDivVertex ));
            tringles.Add(new TringleMesh(dividerEdge.A, dividerEdge.B, usedVertexClerEdge ) );
            tringles.Add(new TringleMesh(dividerEdge.A, dividerEdge.B, opposite) );

            Debug.Log(tringles[0].ToString());
            Debug.Log(tringles[1].ToString());
            Debug.Log(tringles[2].ToString());

            return tringles;
            }

        /// <summary>
        /// содержит ли треугольник этот индекс
        /// </summary>
        /// <param name="_index">номер</param>
        /// <returns></returns>
        public bool EstAvoirIndex(int _index)
            {
            for (int i = 0; i < 3; i++)
                {
                if (index[i] == _index)
                    {
                    return true;
                    }
                }
            return false;
            }
        public bool estVoisine(TringleMesh tringle)
            {
            bool res = index[0] == tringle.index[0];

            return res;
            }
        public override string ToString()
            {
            string res = index[0] + " " + index[1] + " " + index[2];
            return res;
            }
        public static int[] GetTringlesFromListTringles(List<TringleMesh> list)
            {
            int[] res = new int[list.Count * 3];

            for (int i = 0; i < list.Count; i++)
                {
                res[i*3   ] = list[i].index[0];
                res[i*3 +1] = list[i].index[1];
                res[i*3 +2] = list[i].index[2];
                }
            return res;
            }
        public static List<TringleMesh> GetTringleMeshListFromArrayIndexes(int[] array, Mesh mesh)
            {
            int lent = array.Length / 3;
            List<TringleMesh> res = new List<TringleMesh>();

            for (int i = 0; i < lent; i++)
                {
                res.Add(new TringleMesh(array[i * 3], array[i * 3 + 1], array[i * 3 + 2], mesh ) );
                }

            return res;
            }       
        public List<Vector3> PointInterreption(PlaneTroisPoint plane)
            {
            List<Vector3> res = new List<Vector3>();

            Vector3 pos = new Vector3();

            if(plane.InterectionPointLine(vertex[0], vertex[1], ref pos))
                {
                res.Add(pos);
                }

            if (plane.InterectionPointLine(vertex[2], vertex[1], ref pos))
                {
                res.Add(pos);
                }

            if (plane.InterectionPointLine(vertex[0], vertex[2], ref pos))
                {
                res.Add(pos);
                }

            return res;
            }
        /// <summary>
        /// класс содержащий информацию о результате деления треугольника
        /// </summary>
        public class DivideInfo
            {
            /// <summary>
            /// разделенное ребро, точка разделения ребра
            /// </summary>
            public List<(EdgeTringle, Vector3, Vector2)> ps = new List<(EdgeTringle, Vector3, Vector2)>();
            }
        
        public void ShowCa(Vector3 bias, Color col)
            {
            Debug.DrawLine(bias + vertex[0], bias + vertex[1], col, 100.0f, true);
            Debug.DrawLine(bias + vertex[1], bias + vertex[2], col, 100.0f, true);
            Debug.DrawLine(bias + vertex[2], bias + vertex[0], col, 100.0f, true);
            }

        /// <summary>
        /// нати номер вершины треугольника, котораая общая с двумя ребрами
        /// </summary>
        /// <param name="AB"></param>
        /// <param name="BC"></param>
        /// <returns>от 0 до 2. Если ошибка -1</returns>
        public int FindCommonVertexIndex(EdgeTringle AB, EdgeTringle BC)
            {
            if(AB.indexA == BC.indexA || AB.indexA == BC.indexB)
                {
                return Array.FindIndex(index, x => x == AB.indexA);
                }
            else
                {
                return Array.FindIndex(index, x => x == AB.indexB);
                }

            }
        }
    
    /// <summary>
    /// класс ребра треугольника
    /// </summary>
    public class EdgeTringle : IEquatable<EdgeTringle>
        {
        public int indexA, indexB;
        public Vector3 posA, posB;

        public VertexMesh A, B;
     
        public EdgeTringle(int a, int b, Vector3 _posA, Vector3 _posB, Vector3 _uvA, Vector3 _uvB)
            {
            indexA = a;
            indexB = b;

            posA = _posA;
            posB = _posB;

            A = new VertexMesh(_posA, _uvA, a);
            B = new VertexMesh(_posB, _uvB, b);
            }

        public EdgeTringle(VertexMesh _A, VertexMesh _B)
            {
            A = _A;
            B = _B;

            indexA = _A.index;
            indexB = _B.index;
            posA = _A.vertex;
            posB = _B.vertex;
            }

        public bool Equals(EdgeTringle other)
            {
            if ((indexA == other.indexA && indexB == other.indexB) || (indexA == other.indexB && indexB == other.indexA))
                {
                return true;
                }
            return false;
            }

        public override int GetHashCode()
            {
            return base.GetHashCode();
            }
        
        public void ShowCa(Color col, Vector3 pos)
            {            
            Debug.DrawLine(pos + A.vertex, pos + B.vertex, col, 100.0f, true);
            }
        }
    
    public class Visialisator
        {
        List<Color> listColor;
        int count = 0;
         public Visialisator()
            {
            listColor = new List<Color>();
            listColor.Add(Color.blue);
            listColor.Add(Color.black);
            listColor.Add(Color.cyan);
            listColor.Add(Color.red);
            listColor.Add(Color.green);
            listColor.Add(Color.grey);
            listColor.Add(Color.white);
            }

        public void Show(System.Object _object)
            {
            if (_object.GetType() == typeof( TringleMesh) )
                {
                count++;
                int res = count % listColor.Count;

                TringleMesh _tr = _object as TringleMesh;
                _tr.ShowCa( Vector3.zero, listColor[res]);
                }
            }
        }
    }