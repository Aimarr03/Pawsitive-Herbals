using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    namespace GameManagerLogic
    {
        public class Manager_Game : MonoBehaviour
        {
            public static Manager_Game instance;
            public int exp_kini;
            public int uang_kini;

            private void Awake()
            {
                if(instance == null)
                {
                    instance = this;
                    return;
                }
                else
                {
                    Destroy(this);
                }
            }
        }
    }
    
}

