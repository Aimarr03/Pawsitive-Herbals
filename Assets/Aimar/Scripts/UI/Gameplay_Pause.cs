using AimarWork.GameManagerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AimarWork
{
    public class Gameplay_Pause : MonoBehaviour
    {
        public RectTransform container;
        public RectTransform containerOptions;
        private void Start()
        {
            container.gameObject.SetActive(false);
            containerOptions.gameObject.SetActive(false);
            Manager_Game.instance.OnPauseInvoke += Paused;
        }
        private void OnDisable()
        {
            Manager_Game.instance.OnPauseInvoke -= Paused;
            Manager_Game.instance.OnPauseInvoke -= Resume;
            Manager_Game.instance.OnPauseInvoke -= CloseOptions;
        }
        public void Paused()
        {
            Manager_Game.instance.IsPaused = true;
            container.gameObject.SetActive(true);
            containerOptions.gameObject.SetActive(false);
            Manager_Game.instance.OnPauseInvoke -= Paused;
            Manager_Game.instance.OnPauseInvoke += Resume;
        }
        public void Resume()
        {
            Manager_Game.instance.IsPaused = false;
            Manager_Game.instance.OnPauseInvoke -= Resume;
            Manager_Game.instance.OnPauseInvoke += Paused;
            container.gameObject.SetActive(false);
            containerOptions.gameObject.SetActive(false);
        }
        public void OpenOptions()
        {
            containerOptions.gameObject.SetActive(true);    
            Manager_Game.instance.OnPauseInvoke -= Resume;
            Manager_Game.instance.OnPauseInvoke += CloseOptions;
        }
        public void CloseOptions()
        {
            containerOptions.gameObject.SetActive(false);
            Manager_Game.instance.OnPauseInvoke -= CloseOptions;
            Manager_Game.instance.OnPauseInvoke += Resume;
        }
        public void ExitToMainMenu() => Manager_Game.instance.ExitToMainMenu();
        public void ToggleMusic(bool value) => Manager_Audio.instance?.ToggleMusic(value);
        public void ToggleSFX(bool value) => Manager_Audio.instance?.ToggleSFX(value);
    }
}

