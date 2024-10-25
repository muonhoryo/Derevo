
using System;
using UnityEngine;

namespace Derevo.Level
{
    [Serializable]
    public class ValuableCell
    {
        public enum DiffusionDirection
        {

        }

        private int Value=0;
        public int Value_ 
        {
            get=>Value; 
            set
            {
                if (value < 0)
                    return;

                Value = value;
            } 
        }
        
    }
}