

using System;
using UnityEngine;

namespace Derevo.UI
{
    //Gameobject of the script should be active self
    public class UILayer : MonoBehaviour
    {
        public static event Action<UILayer> SelectLayerEvent = delegate { };
        public static UILayer CurrentLayer_ { get; private set; } = null;

        [SerializeField] private bool IsActiveOnAwake = false;

        private void Awake()
        {
            SelectLayerEvent += SelectLayer;
            if (IsActiveOnAwake)
            {
                if (CurrentLayer_ == null)
                {
                    CurrentLayer_ = this;
                }
                else
                    gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            SelectLayerEvent -= SelectLayer;
        }
        private void SelectLayer(UILayer layer)
        {
            if (layer != this)
            {
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
            }
            else
                gameObject.SetActive(true);
        }

        public void SelectThis()
        {
            CurrentLayer_ = this;
            SelectLayerEvent(this);
        }
    }
}