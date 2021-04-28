using Mathan;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine;
using Debug = UnityEngine.Debug;

public class Voxel : MonoBehaviour
    {
    public LayerMask dispMask;

    public bool estMulLumiere = true;
    public Camera camera;
    public RawImage image_1, image_2;

    public Matrix4x4 matx;

    public int count = 100, layer = 10;
    public Mesh mesh;
    public Material mat;
    public Texture2D mapHight, noizeTex, SphereTex;
    RenderTexture    mainTex;

    public RenderTexture RayMap, expTex;
    public GameObject markObject, mark_2;

    [Range(0, 5.0f)]
    public float h;

    [Range(0, 5.0f)]
    public float h2;

    [Range(0, 500.0f)]
    public float h3;

    [Range(0, 2.0f)]
    public float h4;

    [Range(0, 2.0f)]
    public float h5;

    [Range(0, 20)]
    public int nomber_1;



    public Vector3 bias = Vector3.zero;

    const int size = 1024;
    int idMain, idClear;
    Dictionary<string, int> idKernel;
    ChaderSysteme chader, bloorShader;
    private RenderTexture tempTex;
    Monde monde;
    ComputeBuffer bufferLum;

    public Color colorA, colorB;
    public Vector4 vecNorm, vecBias;

    Entite ent;

    TempeteRender render;
    ChaderSysteme VecRend;
    // Start is called before the first frame update
    void Start()
        {        

        Application.targetFrameRate = 30;

        Prepare();
        }
    void Update()
        {       
        //return;
        //image_2.texture = Visualiseur.VisualiserCa(ent, Mathf.RoundToInt( nomber_1) );

        Monde.SourseDeLum lum_1 = new Monde.SourseDeLum(markObject.transform.position, new Vector3(colorA.r, colorA.g, colorA.b), colorA.a);
        Monde.SourseDeLum lum_2 = new Monde.SourseDeLum(mark_2.transform.position, new Vector3(colorB.r, colorB.g, colorB.b), colorB.a);
        Monde.SourseDeLum lum_3 = new Monde.SourseDeLum(new Vector3(100, 150, 80), new Vector3(1, 1, 1), 100.0f);
        Monde.SourseDeLum lum_4 = new Monde.SourseDeLum(new Vector3(150, 150, 80), new Vector3(1, 1, 1), 100.0f);
        Monde.SourseDeLum[] arrLum = new Monde.SourseDeLum[] { lum_1, lum_2, lum_3, lum_4 };
        bufferLum.SetData(arrLum);
        chader.SetBufferPourToutKernels(bufferLum, "LumSources");

        chader.shader.SetVector("camPos",  camera.transform.position);
        chader.shader.SetVector("viewDIr", markObject.transform.position);
        chader.shader.SetVector("vecNorm", vecNorm);
        chader.shader.SetFloat("h", h);
        chader.shader.SetFloat("h2", h2);
        chader.shader.SetFloat("h3", h3);
        chader.shader.SetFloat("h4", h4);
        chader.shader.SetFloat("h5", h5);
        chader.shader.SetFloat("kX", nomber_1);

        chader.shader.Dispatch(idClear, 32, 32, 32);
        chader.shader.Dispatch(idKernel ["Blitz"], 32, 32, 32);

        for (int i = 0; i < 1; i++)
            {
            chader.SetInt("numeroLumSousre", i);
            chader.shader.Dispatch(idKernel ["Lum"], 8, 8, 6);
            chader.shader.Dispatch(idKernel ["LumSum"], 32, 32, 32);
            }

        //chader.shader.Dispatch(idKernel ["Sol"], 16, 16, 1);
        chader.Dispatch("Mul");          
                                

        if (Input.GetKeyDown(KeyCode.R))
            {
            /*
            const int sizeCh = 128;

            Monde chonk = new Monde(sizeCh, sizeCh, sizeCh);
            chonk.Fillimon(new Color(0, 0, 1, 0.003f));
            chonk.AddSphere(new Vector3(50, 50, 50), 30, colorB);

            chader.SetTexture(chonk.texture, "Chunck");
            chader.shader.SetVector("chunckBias", vecBias);
            */
            chader.shader.Dispatch(idKernel["Add"], 32, 32, 32);
            }

        if (Input.GetKey(KeyCode.T) || Input.GetKeyDown(KeyCode.Y))
            {
            for (int i = 0; i <= nomber_1; i++)
                {
                chader.shader.Dispatch(idKernel ["Change"], 32, 32, 32);
                }
            //chader.shader.Dispatch(idKernel ["Change"], 32, 32, 32);
            }
        
        }

    void Prepare()
        {
        Debug.Log( "Размер = " + Monde.Particule.GetSize() );
        //ent = Entite.GetEntiteFromMesh(GameObject.Find("Forma"), "certg");

        idKernel = new Dictionary<string, int>();
        Caching.ClearCache();

        if (false)
            {
            monde = Monde.LoadMonde("мир_1");
            monde.AddSphere(new Vector3(100, 100, 35), 25, new Color(1, 1, 0, 0.02f));
            //monde.AddSphere(new Vector3(100, 100, 40), 30, new Color(1, 0, 0, 0.01f) );
            //monde.AddBox(new int3(100, 100, 0), new int3(200, 200, 20), new Color(1, 1, 1, 1) );
            //monde.AddBox(new int3(0, 0,20), new int3(500, 500, 100), new Color(0.5f, 0.5f, 0.5f, 0.002f));
            }
        else
            {
            monde = new Monde(256, 256, 190);
            monde.GenerateMapFromTexture(mapHight, new Color(1, 1, 1, 0.003f));
            //monde.AddBox(new int3(30, 30, 10), new int3(50, 50, 50), Color.blue);
            monde.AddBox(new int3(150, 50, 200), new int3(50, 5, 80), new Color(1, 0, 1, 0.05f));
            monde.AddBox(new int3(150, 70, 180), new int3(50, 15, 80), new Color(1, 1, 0, 1));
            monde.AddSphere(new Vector3(150, 150, 230), 25, Color.yellow);
            monde.AddSphere(new Vector3(100, 100, 35), 30, new Color(0, 1, 0, 0.1f));           
            }

        //monde.AddEntite(ent, new int3(10, 10, 10));

        const int size = 1024, sizeVoxelCount = 256 * 256 * 256;
        string [] nameMainKernel = new string [] { "CSMain", "Clear", "Experemental", "Changement", "Lumiere", "Multiplie", "BlooreH", "BlooreV", "Lumiere_2", "AddEntite", "Blitz", "Soleil", "BlitzLum" }; // 13 шт, от 0 до 12

        if (camera == null)
            {
            camera = GetComponent<Camera>();
            }

        mainTex = ChaderSysteme.GetRT(size, size);

        RenderTexture zBuffer        = ChaderSysteme.GetRT(size, size, RenderTextureFormat.RFloat);
        RenderTexture mondeInput     = ChaderSysteme.GetRt3D(256, 256, 256, RenderTextureFormat.ARGB4444);
        RenderTexture texLumData     = ChaderSysteme.GetRt3D(256, 256, 256, RenderTextureFormat.ARGB4444);
        RenderTexture texSumLum      = ChaderSysteme.GetRt3D(256, 256, 256, RenderTextureFormat.ARGB4444);
        RenderTexture texMondeNorm   = ChaderSysteme.GetRt3D(256, 256, 256, RenderTextureFormat.ARGB4444);
        tempTex                      = ChaderSysteme.GetRT(size, size);

        chader = new ChaderSysteme("Voxel", nameMainKernel, mainTex);
        chader.shader.SetVector("dim", new Vector4(256, 256, 256, 0));

        idMain = chader.GetKernel(nameMainKernel [0]);
        idClear = chader.GetKernel(nameMainKernel [1]);
        idKernel.Add("Main",   idMain );
        idKernel.Add("Clear",  idClear);
        idKernel.Add("Exp",    chader.GetKernel(nameMainKernel [2]));
        idKernel.Add("Change", chader.GetKernel(nameMainKernel [3]));
        idKernel.Add("Lum",    chader.GetKernel(nameMainKernel [4]));
        idKernel.Add("Mul",    chader.GetKernel(nameMainKernel [5]));
        idKernel.Add("BlurH",  chader.GetKernel(nameMainKernel [6]));
        idKernel.Add("BlurV",  chader.GetKernel(nameMainKernel [7]));
        idKernel.Add("Lum_2",  chader.GetKernel(nameMainKernel [8]));
        idKernel.Add("Add",    chader.GetKernel(nameMainKernel [9]));
        idKernel.Add("Blitz",  chader.GetKernel(nameMainKernel [10]));
        idKernel.Add("Sol",    chader.GetKernel(nameMainKernel [11]));
        idKernel.Add("LumSum", chader.GetKernel(nameMainKernel [12]));

        chader.SetNumeroThredes(32, 32, 1);
        chader.SetPublicTexture(mainTex,        "Result");
        chader.SetPublicTexture(tempTex,        "Tempore");
        chader.SetPublicTexture(zBuffer,        "zBuffer");
        chader.SetPublicTexture(texLumData,     "worldLum");
        chader.SetPublicTexture(mondeInput,     "mondeInput");
        chader.SetPublicTexture(texSumLum,      "sumLum");
        chader.SetPublicTexture(texMondeNorm,   "mondeNorm");
        chader.SetTexture      (noizeTex,       "Noize");
        chader.SetTexture      (SphereTex,      "SphereMap");

        //один float = 4 байта. 4*4 = 16 (float4)

        bufferLum = new ComputeBuffer(4, Monde.SourseDeLum.Lent);

        chader.CreerEtSetVideBuffer<Monde.Particule>(Monde.Particule.Lent, sizeVoxelCount, "Particl");
        chader.CreerEtSetVideBuffer<Monde.RefVoxel >(Monde.RefVoxel.Lent,  sizeVoxelCount, "BufferRefVoxel");

        chader.SetBufferPourToutKernels(bufferLum,   "LumSources");
        chader.SetTexture(monde.texture, "Input");

        image_1.texture = mainTex;

        chader.SetTexture(RayMap, "RayMap");

        chader.shader.Dispatch(idKernel ["Exp"],   32, 32, 32);
        chader.shader.Dispatch(idKernel ["Blitz"], 32, 32, 32);

        //bufferMonde.Release();
        }

    public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
        {
        byte [] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
        }

    Texture2D toTexture2D(RenderTexture rTex)
        {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
        }

    public void SauverMonde(string name = "мир")
        {
        monde.Souver(name);
        }

    public void ProcModel(string name)
        {

        }
    
    private void SetColor()
        {

        }
    
    void Exp()
        {
        RenderTexture map = ChaderSysteme.GetRT(2048, 2048);

        VecRend = new ChaderSysteme("RenderMapRay", new string [] { "CSMain" });
        VecRend.SetFloat("h", h);
        VecRend.SetPublicTexture(map, "Result");
        VecRend.SetNumeroThredes(64, 64, 1);
        int kernM = VecRend.GetKernel("CSMain");
        //VecRend.Dispatch(kernM);
        image_1.texture = map;

        Texture2D tex = toTexture2D(map);
        SaveTextureAsPNG(tex, Application.dataPath + "/rayMapSphere.png");

        return;
        }
    }

namespace VoxelEngine
    {
    /// <summary>
    /// оболочка для класса рендера
    /// </summary>
    public class TempeteRender
        {
        private Dictionary<string, int> idKernel;
        Monde monde;
        public RenderTexture mainTex;
        private RenderTexture tempTex;
        private ChaderSysteme chader;
        private ComputeBuffer bufferLum;
        private IEnumerator cycle = null;

        public List<ObjectMonde> objects;

        public TempeteRender(Texture2D mapHight, RenderTexture ReyMap)
            {
            Prepare(mapHight, ReyMap);
            }

        void Prepare(Texture2D mapHight, RenderTexture ReyMap)
            {
            //ent = Entite.GetEntiteFromMesh(GameObject.Find("Forma"), "certg");

            idKernel = new Dictionary<string, int>();
            Caching.ClearCache();

            if (false)
                {
                monde = Monde.LoadMonde("мир_1");
                monde.AddSphere(new Vector3(100, 100, 35), 25, new Color(1, 1, 0, 0.02f));
                //monde.AddSphere(new Vector3(100, 100, 40), 30, new Color(1, 0, 0, 0.01f) );
                //monde.AddBox(new int3(100, 100, 0), new int3(200, 200, 20), new Color(1, 1, 1, 1) );
                //monde.AddBox(new int3(0, 0,20), new int3(500, 500, 100), new Color(0.5f, 0.5f, 0.5f, 0.002f));
                }
            else
                {
                monde = new Monde(256, 256, 190);
                //monde.GenerateMapFromTexture(mapHight);
                //monde.AddBox(new int3(30, 30, 10), new int3(50, 50, 50), Color.blue);
                //monde.AddBox(new int3(100, 50, 200), new int3(50, 5, 50), new Color(1, 0, 1, 0.05f));
                //monde.AddBox(new int3(100, 70, 200), new int3(50, 5, 50), new Color(1, 1, 0, 0.05f));
                monde.AddSphere(new Vector3(150, 150, 230), 25, Color.yellow);
                //monde.AddSphere(new Vector3(100, 100, 35), 50, new Color(0, 1, 0, 0.1f));
                }

            //monde.AddEntite(ent, new int3(10, 10, 10));


            const int size = 1024;
            string [] nameMainKernel = new string [] { "CSMain", "Clear", "Experemental", "Changement", "Lumiere", "Multiplie", "BlooreH", "BlooreV", "Lumiere_2" }; // 9 шт, от 0 до 8
            string [] nameKernelBloor = new string [] { "BlureH" };


            mainTex = ChaderSysteme.GetRT(size, size);

            RenderTexture zBuffer = ChaderSysteme.GetRT(size, size, RenderTextureFormat.RFloat);
            RenderTexture screenWorldPos = ChaderSysteme.GetRT(size, size, RenderTextureFormat.Default);
            RenderTexture ombreCard = ChaderSysteme.GetRT(size, size, RenderTextureFormat.Default);
            RenderTexture mondeInput = ChaderSysteme.GetRt3D(256, 256, 256);
            RenderTexture texLumData = ChaderSysteme.GetRt3D(256, 256, 256);
            tempTex = ChaderSysteme.GetRT(size, size);

            chader = new ChaderSysteme("Voxel", nameMainKernel, mainTex);
            chader.shader.SetVector("dim", new Vector4(256, 256, 256, 0));

            int idMain = chader.GetKernel(nameMainKernel [0]);
            int idClear = chader.GetKernel(nameMainKernel [1]);
            idKernel.Add("Main", idMain);
            idKernel.Add("Clear", idClear);
            idKernel.Add("Exp", chader.GetKernel(nameMainKernel [2]));
            idKernel.Add("Change", chader.GetKernel(nameMainKernel [3]));
            idKernel.Add("Lum", chader.GetKernel(nameMainKernel [4]));
            idKernel.Add("Mul", chader.GetKernel(nameMainKernel [5]));
            idKernel.Add("BlurH", chader.GetKernel(nameMainKernel [6]));
            idKernel.Add("BlurV", chader.GetKernel(nameMainKernel [7]));
            idKernel.Add("Lum_2", chader.GetKernel(nameMainKernel [8]));

            chader.SetNumeroThredes(32, 32, 1);
            chader.SetPublicTexture(mainTex, "Result");
            chader.SetPublicTexture(tempTex, "Tempore");
            chader.SetPublicTexture(zBuffer, "zBuffer");
            chader.SetPublicTexture(screenWorldPos, "worldPosBuffer");
            chader.SetPublicTexture(ombreCard, "ombreCard");
            chader.SetPublicTexture(texLumData, "worldLum");
            chader.SetPublicTexture(mondeInput, "mondeInput");

            //один float = 4 байта. 4*4 = 16 (float4)

            ComputeBuffer bufferMonde = new ComputeBuffer(monde.sizeMonde, 16);
            Vector4 [] inData = new Vector4 [monde.sizeMonde];
            bufferMonde.SetData(inData);

            ComputeBuffer bufferTrans = new ComputeBuffer(monde.sizeMonde, 16);
            Vector4 [] inData_2 = new Vector4 [monde.sizeMonde];
            bufferTrans.SetData(inData_2);

            bufferLum = new ComputeBuffer(4, Monde.SourseDeLum.Lent);
            Monde.SourseDeLum lum_1 = new Monde.SourseDeLum(new Vector3(100, 100, 80), new Vector3(0, 1, 1), 200.0f);
            Monde.SourseDeLum lum_2 = new Monde.SourseDeLum(new Vector3(150, 100, 80), new Vector3(0, 1, 0), 100.0f);
            Monde.SourseDeLum lum_3 = new Monde.SourseDeLum(new Vector3(100, 150, 80), new Vector3(1, 1, 1), 100.0f);
            Monde.SourseDeLum lum_4 = new Monde.SourseDeLum(new Vector3(150, 150, 80), new Vector3(1, 1, 1), 100.0f);
            Monde.SourseDeLum [] arrLum = new Monde.SourseDeLum [] { lum_1, lum_2, lum_3, lum_4 };
            bufferLum.SetData(arrLum);

            //chader.shader.SetBuffer(idMain, "monde", bufferMonde);
            //chader.shader.SetBuffer(idKernel["Exp"], "monde", bufferMonde);

            chader.SetBufferPourToutKernels(bufferMonde, "monde");
            chader.SetBufferPourToutKernels(bufferTrans, "transLumData");
            chader.SetBufferPourToutKernels(bufferLum, "LumSources");
            //bufferMonde.Release();

            //chader.shader.SetTexture(idMain, "Input", monde.texture);
            chader.SetTexture(monde.texture, "Input");

            chader.SetTexture(ReyMap, "RayMap");

            chader.shader.Dispatch(idKernel ["Exp"], 32, 32, 32);

            bufferMonde.GetData(inData);
            //bufferMonde.Release();
            Debug.Log("норм");
            }

        public void Fillimon(Color col)
            {
            chader.shader.SetVector("tempColor", new Vector4(col.r, col.g, col.b, col.a) );
            }

        public RenderTexture Tick(Vector3 camPos, Vector3 lumPos, Vector3 colorLumA, Vector3 colorLumB, float [] parameters)
            {
            Monde.SourseDeLum lum_1 = new Monde.SourseDeLum(new Vector3(100, 100, 80), colorLumA, 200.0f);
            Monde.SourseDeLum lum_2 = new Monde.SourseDeLum(new Vector3(150, 100, 80), colorLumB, 100.0f);
            Monde.SourseDeLum lum_3 = new Monde.SourseDeLum(new Vector3(100, 150, 80), new Vector3(1, 1, 1), 100.0f);
            Monde.SourseDeLum lum_4 = new Monde.SourseDeLum(new Vector3(150, 150, 80), new Vector3(1, 1, 1), 100.0f);
            Monde.SourseDeLum [] arrLum = new Monde.SourseDeLum [] { lum_1, lum_2, lum_3, lum_4 };
            bufferLum.SetData(arrLum);
            chader.SetBufferPourToutKernels(bufferLum, "LumSources");

            chader.shader.SetVector("camPos", camPos);
            chader.shader.SetVector("viewDIr", lumPos);
            chader.shader.SetVector("vecNorm", new Vector3(-1, -1, 1));
            chader.shader.SetFloat("h", parameters [0]);
            chader.shader.SetFloat("h2", parameters [1]);
            chader.shader.SetFloat("h3", parameters [2]);
            chader.shader.SetFloat("h4", parameters [3]);
            chader.shader.SetFloat("h5", parameters [4]);
            chader.shader.SetFloat("kX", parameters [5]);
            //chader.SetTexture(RayMap, "RayMap");

            var data_1 = Time.realtimeSinceStartup;

            chader.shader.Dispatch(idKernel ["Lum_2"], 16, 32, 32);
            //chader.Dispatch(idKernel ["Mul"]);

            var data_2 = Time.realtimeSinceStartup;
            float delta = data_2 - data_1;

            //image.texture = mainTex;

            if (Input.GetKey(KeyCode.R))
                {
                chader.shader.Dispatch(idKernel ["Change"], 32, 32, 32);
                }
            return mainTex;
            }
        
        }

    public class Monde
        {
        [SerializeField]
        public Texture3D texture;

        public Texture3D tex_1;
        public Texture3D tex_2;

        public readonly int sizeMonde;
        public Monde(int sizeX, int sizeY, int sizeZ)
            {
            TextureFormat format = TextureFormat.RGBA32;
            texture = new Texture3D(sizeX, sizeY, sizeZ, format, false);
            int demiZ = sizeZ / 2;

            //Texture2D monde = new Texture2D(2048/2, 2048/2);

            //tex_1 = new Texture3D(1, 1, 128, format, false);
            //tex_2 = new Texture3D(sizeX, sizeY, demiZ, format, false);

            sizeMonde = sizeX * sizeY * sizeZ;
            }

        public void GenerateMapFromTexture(Texture2D _map, Color _fond)
            {
            //texture = new Texture3D(256, 256, 256, TextureFormat.RGBA32, false);

            if (_map.width != texture.width && _map.height != texture.height)
                {
                Debug.Log("несовпадает разрешение");
                }

            float stepDepth = 1.0f / texture.depth;

            for (int i = 0; i < texture.width; i++)
                {
                for (int j = 0; j < texture.height; j++)
                    {
                    for (int k = 0; k < texture.depth; k++)
                        {
                        if (_map.GetPixel(i, j).r > stepDepth * k)
                            {
                            texture.SetPixel(i, j, k, new Color( (float) i / texture.width, (float)j / texture.height, (float) k / texture.depth, 1.0f));
                            }

                        else
                            {
                            texture.SetPixel(i, j, k, _fond);
                            }

                        //texture.SetPixel(i, j, k, new Color(1.0f, 1.0f, k / texture.depth, 0.9f));
                        }
                    }
                }

            texture.Apply();
            }

        public void CreerVideMonde(Color _fond)
            {
            for (int i = 0; i < texture.width; i++)
                {
                for (int j = 0; j < texture.height; j++)
                    {
                    for (int k = 0; k < texture.depth; k++)
                        {
                        texture.SetPixel(i, j, k, _fond);
                        }
                    }
                }

            texture.Apply();
            }
        
        public void CreerTerre(Color _color, int epasseur)
            {
            for (int i = 0; i < texture.width; i++)
                {
                for (int j = 0; j < texture.height; j++)
                    {
                    for (int k = 0; k < epasseur; k++)
                        {
                        texture.SetPixel(i, j, k, _color);
                        }
                    }
                }

            texture.Apply();
            }
        public void Fillimon(Color col)
            {
            for (int i = 0; i < texture.width; i++)
                {
                for (int j = 0; j < texture.height; j++)
                    {
                    for (int k = 0; k < texture.depth; k++)
                        {
                        texture.SetPixel(i, j, k, col);
                        }
                    }
                }

            texture.Apply();
            }

        public void AddSphere(Vector3 position, int radius, Color color)
            {

            float stepDepth = 1.0f / texture.depth;

            for (int i = 0; i < texture.width; i++)
                {
                for (int j = 0; j < texture.height; j++)
                    {
                    for (int k = 0; k < texture.depth; k++)
                        {
                        Vector3 courrPos = new Vector3(i, j, k);

                        if (Vector3.Distance(courrPos, position) < radius)
                            {
                            texture.SetPixel(i, j, k, color);
                            }
                        else
                            {
                            //texture.SetPixel(i, j, k, new Color(0.0f, 0.0f, 0.0f, 0.0f));
                            }
                        }
                    }
                }

            texture.Apply();
            }

        public void AddBox(int3 pos, int3 size, Color color)
            {
            for (int i = pos.x; i < pos.x + size.x; i++)
                {
                for (int j = pos.y; j < pos.y + size.y; j++)
                    {
                    for (int k = pos.z; k < pos.z + size.z; k++)
                        {
                        texture.SetPixel(i, j, k, color);
                        }
                    }
                }
            texture.Apply();
            }

        public bool Souver(string _name)
            {
            BinaryFormatter binary = new BinaryFormatter();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(texture, "Assets/Resources/" + _name + ".asset");
            Debug.Log("схоронил");
#endif
            return false;
            }

        public void AddEntite(Entite ent, int3 pos)
            {
            for (int i = pos.x; i < pos.x + ent.forma.height; i++)
                {
                for (int j = pos.y; j < pos.y + ent.forma.width; j++)
                    {
                    for (int k = pos.z; k < pos.z + ent.forma.depth; k++)
                        {
                        texture.SetPixel(i, j, k, ent.forma.GetPixel(i, j, k));
                        }
                    }
                }

            texture.Apply();
            }

        public static Monde LoadMonde(string _name)
            {
            var tex = Resources.Load(_name) as Texture3D;
            if (tex == null)
                {
                Debug.Log("Текстура " + _name + " не загружена");
                return null;
                }

            Monde res = new Monde(tex.width, tex.width, tex.depth);
            res.texture = tex;

            RenderTexture tex3d = new RenderTexture(tex.width, tex.height, tex.depth, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            tex3d.enableRandomWrite = true;
            tex3d.format = (RenderTextureFormat)11;
            tex3d.Create();

            Graphics.SetRandomWriteTarget(0, tex3d);

            Graphics.Blit(res.texture, tex3d);

            //Debug.Log("загружю...");

            return res;
            }

        public Color SafetyReadTex(int3 pos)
            {
            Color res = new Color(0, 0, 0, 0);

            if( (pos.x < 0 || pos.x >= texture.width) || (pos.y < 0 || pos.y >= texture.height) || (pos.z < 0 || pos.z >= texture.depth) )
                {
                return res;
                }

            res = texture.GetPixel(pos.x, pos.y, pos.z);
            return res;
            }

        public void SafetyEcrir(int3 pos, Color _col)
            {
            if ((pos.x < 0 || pos.x > texture.width) || (pos.y < 0 || pos.y > texture.height) || (pos.z < 0 || pos.z > texture.depth))
                {
                return;
                }

            texture.SetPixel(pos.x, pos.y, pos.z, _col);
            }

        public void DrawMarcos(int3 pos, int rad, Color color)
            {
            for (int i = pos.x - rad; i < pos.x + rad; i++)
                {
                for (int j = pos.y - rad; j < pos.y + rad; j++)
                    {
                    for (int k = pos.z - rad; k < pos.z + rad; k++)
                        {
                        texture.SetPixel(i, j, k, color);
                        }
                    }
                }

            texture.Apply();

            }

        /// <summary>
        /// источник света
        /// </summary>
        public struct SourseDeLum
            {
            public SourseDeLum(Vector3 _pos, Vector3 _color, float _intens)
                {
                pos = _pos;
                color = _color;
                intensivity = _intens;
                }

            public Vector3 pos;
            public Vector3 color;
            public float intensivity;
            //длина = 4*3 + 4*3 + 4*1 = 4 * 7 = 28
            public const int Lent = 28;
            };

        public struct Particule
            {
            int   posX;
            int   posY;
            int   posZ;
            int   colorX;
            int   colorY;
            int   colorZ;
            int   colorA;
            float mass;
            int   refVoxel;

            int vitX;
            int vitY;
            int vitZ;
            int subPosX, subPosY, subPosZ;

            public const int Lent = 15 * 4;

            public static int GetSize()
                {
                int size;
                unsafe
                    {
                    size = sizeof(Particule);
                    }
                return size;
                }
            };

        public struct RefVoxel
            {
            int pos;
            public const int Lent = 4;
            }
        }

    /// <summary>
    /// 3д сущность
    /// </summary>
    public class Entite
        {
        public readonly string nom;

        /// <summary>
        /// форма сущности
        /// </summary>
        public Texture3D forma;

        public Entite(Texture3D _form, string _nom)
            {
            forma = _form;
            nom = _nom;
            }

        public static Entite GetEntiteFromMesh(GameObject obj, string _nom)
            {
            const int resolution = 20;
            Texture3D tex = new Texture3D(resolution, resolution, resolution, TextureFormat.RGBA32, true);

            if (obj.GetComponent<MeshCollider>())
                {
                obj.transform.position = Vector3.zero;
                MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
                var min = meshCollider.bounds.min;
                var max = meshCollider.bounds.max;
                Vector3 delta = max - min;
                Debug.Log(min + " " + max + " дельтаГраницы = " + delta);
                int maxDim = Mathf.RoundToInt(Mathf.Max(delta.x, delta.y, delta.z)) + 1;
                float step = delta.magnitude / resolution;
                Debug.Log(maxDim);
                int count = 0;
                for (int x = 0; x < resolution; x++)
                    {
                    for (int y = 0; y < resolution; y++)
                        {
                        for (int z = 0; z < resolution; z++)
                            {
                            Vector3 courrPos = min + (new Vector3(x, y, z)) * step;
                            bool est = Physics.CheckSphere(courrPos, 0.2f);
                            if (est)
                                {
                                count++;
                                tex.SetPixel(x, y, z, Color.red);
                                }
                            else
                                {
                                tex.SetPixel(x, y, z, new Color(0, 0, 0, 0));
                                }
                            }
                        }
                    }
                Debug.Log("число = " + count);
                tex.Apply();
                }
            else
                {
                Debug.Log("нет коллайдера");
                }

            Entite ent = new Entite(tex, _nom);
            return ent;
            }
        }

    public class ObjectMonde
        {
        public Entite entite;
        public Vector3 pos;

        public ObjectMonde(Entite _entite, Vector3 _pos )
            {
            entite = _entite;
            pos    = _pos;
            }
        }

    public class Visualiseur
        {
        public static Texture2D VisualiserCa(Entite entite, int layer)
            {
            Texture2D tex = new Texture2D(entite.forma.height, entite.forma.width );

            for (int i = 0; i < entite.forma.height; i++)
                {
                for (int j = 0; j < entite.forma.width; j++)
                    {
                    tex.SetPixel(i, j, entite.forma.GetPixel( i, j, layer) );
                    }
                }

            tex.Apply();
            return tex;
            }
        }
    }