

using System;
using UnityEngine;

namespace Derevo.UI
{
    public class Submenu : MonoBehaviour
    {
        public event Action ShowingEvent= delegate { };
        public event Action HidingEvent = delegate { };

        public void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                ShowingEvent();
            }
        }
        public void Hide()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                HidingEvent();
            }
        }
    }
}