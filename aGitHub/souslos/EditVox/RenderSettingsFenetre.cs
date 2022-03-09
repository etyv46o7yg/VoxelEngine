using Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.souslos.EditVox
    {
    public class RenderSettingsFenetre : MonoBehaviour
        {
        public RadioButtonChangerPanel radioButton;
        public RenderMetters metters;
        public MenedgerRouts menedger;
        public PanelAvecSlider panelLumiesiti;
        public Slider sliderFSP;
        public Text   textValueFSP;

        private void Start()
            {
            metters = new RenderMetters();

            menedger.fenetreSettingsRender = this;

            radioButton = GetComponentInChildren<RadioButtonChangerPanel>();
            radioButton.PanelAEteChanhe += ChangerRegime;
            panelLumiesiti.ValChager    += LumChage;

            sliderFSP.onValueChanged.AddListener(SliderFSPAEteChager);
            SliderFSPAEteChager(1);
            }

        private int ChangerRegime(int _param)
            {
            metters.complexite = (RenderMetters.Complexite) _param;
            return 0;
            }

        private void LumChage(float _param)
            {
            metters.luminosite = _param;
            }

        private void SliderFSPAEteChager(float _val)
            {
            int value = Mathf.RoundToInt( _val);
            int res = 0;

            switch (value)
                {
                case 0: res = 1;  break;
                case 1: res = 30; break;
                case 2: res = 45; break;
                case 3: res = 60; break;
                default:    break;
                }

            metters.prefereFSP = res;
            textValueFSP.text = res.ToString();
            }
        }
    }
