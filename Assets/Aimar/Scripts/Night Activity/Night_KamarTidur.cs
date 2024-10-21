using AimarWork;
using AimarWork.GameManagerLogic;
using FadlanWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night_KamarTidur : BaseInteractableObject
{
    public AudioClip bgm_Night;
    public Canvas containerCanvas;
    private void Start()
    {
        Manager_Audio.instance.PlayMusic(bgm_Night);
        containerCanvas.gameObject.SetActive(false);
    }
    
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        containerCanvas.gameObject.SetActive(true);
    }

    public void JadiTidur()
    {
        Debug.Log("Jadi Tidur");
        containerCanvas.gameObject.SetActive(false);
        Manager_Waktu.instance.GantiHari();
        Manager_Waktu.instance.GantiStatusHari();
        Manager_Game.instance.LoadSceneWithSave(Manager_Game.instance.SCENE_STORE);
    }
    public void TidakJadiTidur()
    {
        Debug.Log("Tidak Jadi Tidur");
        containerCanvas.gameObject.SetActive(false);
    }
}
