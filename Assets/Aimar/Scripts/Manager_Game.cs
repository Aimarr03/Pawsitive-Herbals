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

