using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace AimarWork
{
    namespace GameManagerLogic
    {
        public class Manager_Game : MonoBehaviour
        {
            public readonly string SCENE_STORE= "Store";
            public readonly string SCENE_BEDROOM = "Bedroom";
            public readonly string SCENE_MAINMENU = "MainMenu";
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
            public void TambahProfit(int profit)
            {
                uang_kini += profit;
                OnChangeUang?.Invoke();
            }
            public void GunakanUang(int biaya)
            {
                uang_kini -= biaya;
                OnChangeUang?.Invoke();
            }
            public void TambahExp(int exp)
            {
                exp_kini += exp;
            }
            public void GunakanExp(int exp)
            {
                exp_kini -= exp;
            }
            public void LoadSceneWithSave(string scene)
            {
                Manager_Data.instance.SaveGame();
                SceneManager.LoadSceneAsync(scene);
            }
            public void LoadSceneWithoutSave(string scene)
            {
                SceneManager.LoadSceneAsync(scene);
            }
            public void NewGame()
            {
                Debug.Log("New Game");
                Manager_Data.instance.NewGame();
                LoadSceneWithSave(SCENE_STORE);
            }
            public void LoadGame()
            {
                Debug.Log("Load Game");

            }
            public void ExitToMainMenu()
            {
                Debug.Log("Exit To Main Menu Game");
                LoadSceneWithoutSave(SCENE_MAINMENU);
            }
            public void ExitGame()
            {
                Debug.Log("Exit Game");
                Application.Quit();
            }
        }
    }
    
}

