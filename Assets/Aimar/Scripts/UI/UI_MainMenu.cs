using AimarWork;
using AimarWork.GameManagerLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    public Button ContinueButton;
    public RectTransform OptionsContainer;
    public RectTransform CreditsContainer;
    public AudioClip BGM;
    private void Start()
    {
        Manager_Data.instance.LoadGame();
        OptionsContainer.gameObject.SetActive(false);
        CreditsContainer.gameObject.SetActive(false);
        ContinueButton.interactable = Manager_Data.instance.HasGameData();
        Manager_Audio.instance.PlayMusic(BGM);
    }
    public void NewGame() => Manager_Game.instance.NewGame();
    public void LoadGame() => Manager_Game.instance.LoadGame();
    public void ExitGame() => Manager_Game.instance.ExitGame();
    public void ToggleSFX(bool toggle) => Manager_Audio.instance.ToggleSFX(toggle);

    public void ToggleMusic(bool toggle) => Manager_Audio.instance.ToggleMusic(toggle);
    public void OpenOptions()
    {
        OptionsContainer.gameObject.SetActive(true);
    }
    public void CloseOptions()
    {
        OptionsContainer.gameObject.SetActive(false);
    }
    public void OpenCredits()
    {
        CreditsContainer.gameObject.SetActive(true);
    }
    public void CloseCredits()
    {
        CreditsContainer.gameObject.SetActive(false);
    }
}
