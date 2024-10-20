using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AimarWork
{
    namespace GameManagerLogic
    {
        public class Manager_Game : MonoBehaviour
        {
            public static Manager_Game instance;
            PlayerInput inputs;
            public int exp_kini;
            public int uang_kini;
            public bool IsPaused = false;
            public event Action OnPauseInvoke;
            public event Action OnChangeUang;
            private void Awake()
            {
                if(instance == null)
                {
                    instance = this;
                    inputs = new PlayerInput();
                    return;
                }
                else
                {
                    Destroy(this);
                }
            }
            private void Start()
            {
                inputs.UI.Enable();
                inputs.UI.Pause.performed += Pause_performed;
            }
            private void OnDisable()
            {
                inputs.UI.Disable();
                inputs.UI.Pause.performed -= Pause_performed;
            }
            private void Pause_performed(InputAction.CallbackContext obj)
            {
                OnPauseInvoke?.Invoke();
            }
            public void GetProfit(int profit)
            {
                uang_kini += profit;
                OnChangeUang?.Invoke();
            }
            public void GunakanUang(int biaya)
            {
                uang_kini -= biaya;
                OnChangeUang?.Invoke();
            }
        }
    }
    
}

