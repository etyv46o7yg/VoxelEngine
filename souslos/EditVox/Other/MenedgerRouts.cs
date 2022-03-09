using Assets.souslos.EditVox;
using Render;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// объект для хранения путей к скриптам
/// </summary>
[CreateAssetMenu(fileName = "PrinceMenadger", menuName = "PrinceMenadger", order = 51)]
public class MenedgerRouts : ScriptableObject
    {
    public RenderSettingsFenetre fenetreSettingsRender;
    public Minimal_Render render;
    public GameObject fileBrowser;
    public PrinceMenadger princeMenadger;
    public DirectionPaintScript directionPaint;
    }
