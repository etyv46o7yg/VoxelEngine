using UnityEngine;

namespace Render
    {
    /// <summary>
    /// класс растроек рендера - тип, освещение и т.д.
    /// </summary>
    public class RenderMetters
        {
        public enum Complexite
            {
            Simple = 0,
            Moyen = 1,
            Compose = 2
            }

        public Complexite complexite;
        public float luminosite;
        public Color colorLumiere;
        public int prefereFSP;
        }
    }