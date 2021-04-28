using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SouverTexScript : MonoBehaviour
    {
    public static SouverTexScript instance;

    public RenderTexture rt;
    public string path;
    public int count = 0;

    // Start is called before the first frame update
    void Awake()
        {
        instance = this;

        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public int SouvetTexToFile(Texture2D _tex, string _path)
        {
        byte [] bytes = _tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(_path, bytes);
        return 0;
        }

    public static Texture2D toTexture2D(RenderTexture _rTex)
        {
        Texture2D tex = new Texture2D(_rTex.width, _rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = _rTex;
        tex.ReadPixels(new Rect(0, 0, _rTex.width, _rTex.height), 0, 0);
        tex.Apply();
        Destroy(tex);//prevents memory leak
        return tex;
        }

    public string GeterateName(int _count)
        {
        string res = Application.dataPath + "/tex/rayMap/image_" + _count.ToString() + ".png";
        Debug.Log(res);
        return res;
        }

   
    public void SauverInstance(string _nom)
        {
        SouvetTexToFile(toTexture2D(rt), _nom);
        }
    }

//[CustomEditor(typeof(SouverTexScript))]

public class SouverTexScriptEditor 
    {
    /*
    public override void OnInspectorGUI()
        {
        SouverTexScript sav = SouverTexScript.instance;

        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);

        if (GUILayout.Button("Your ButtonText")) 
            {
            sav.count++;
            int pos = sav.count;
            sav.SauverInstance( sav.GeterateName(pos) );
            }
        }
    */
    }