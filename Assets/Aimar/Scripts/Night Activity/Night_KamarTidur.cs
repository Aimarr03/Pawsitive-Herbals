using DG.Tweening;
using AimarWork;
using AimarWork.GameManagerLogic;
using FadlanWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using TMPro;

public class Night_KamarTidur : BaseInteractableObject
{
    public AudioClip bgm_Night;
    public Canvas containerCanvas;
    public Canvas transisiMalamCanvas;
    public Image backgroundLoadingScreen;
    public GameObject calenderNew;
    public GameObject calenderOld;
    public TextMeshProUGUI tanggalBaru;
    public TextMeshProUGUI tanggalLama;
    public TextMeshProUGUI sisaHari;

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

    public async void JadiTidur()
    {
        Debug.Log("Jadi Tidur");
        containerCanvas.gameObject.SetActive(false);


        DateTime hariSebelumTidur = Manager_Waktu.instance.TanggalKini;

        Manager_Waktu.instance.GantiHari();
        Manager_Waktu.instance.GantiStatusHari();
        
        calenderNew.SetActive(false);
        calenderOld.SetActive(false);
        transisiMalamCanvas.gameObject.SetActive(true);

        tanggalLama.text = hariSebelumTidur.ToString("dd / MM");
        tanggalBaru.text = Manager_Waktu.instance.TanggalKini.ToString("dd / MM");
        sisaHari.text = "sisa " + Manager_Waktu.instance.Sisa_Hari.ToString() + " hari lagi";

        await backgroundLoadingScreen.DOFade(1, 1.3f).AsyncWaitForCompletion();

        calenderNew.SetActive(true);
        calenderOld.SetActive(true);

        await Task.Delay(3000);

        Manager_Game.instance.LoadSceneWithSave(Manager_Game.instance.SCENE_STORE);
    }
    public void TidakJadiTidur()
    {
        Debug.Log("Tidak Jadi Tidur");
        containerCanvas.gameObject.SetActive(false);
    }
}
