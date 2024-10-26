using AimarWork;
using DG.Tweening;
using FadlanWork;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Penglihatan_Resep : BaseInteractableObject
{
    public List<SO_Jamu> List_Resep_Jamu;
    public Canvas canvas;
    public RectTransform containerTransform;

    [Header("UI Jamu")]
    public TextMeshProUGUI namaJamu;
    public Button HalamanSelanjutnya;
    public Button HalamanSebelumnya;
    public AudioClip PaperAudio;

    [Header("Bahan")] 
    public RectTransform containerGambarBahanBahan;
    public Image formatGambar;
    
    [Header("Metode")]
    public RectTransform containerMetode;
    public TextMeshProUGUI formatMetode;
    
    private int maxIndex => List_Resep_Jamu.Count;
    private int currentIndex = 0;
    private void Start()
    {
        List_Resep_Jamu = new List<SO_Jamu>();
        foreach(SO_Jamu jamu in Manager_TokoJamu.instance.List_Jamu)
        {
            if (!jamu.terbuka) continue;
            List_Resep_Jamu.Add(jamu);
        }
        canvas.gameObject.SetActive(false);
        List_Resep_Jamu = List_Resep_Jamu.OrderBy(jamu => jamu.nama).ToList();
    }
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        containerTransform.DOLocalMoveY(-1620, 0);
        currentIndex = 0;
        UpdateResep();
        canvas.gameObject.SetActive(true);
        containerTransform.DOLocalMoveY(-540, 0.8f).SetEase(Ease.OutExpo);
    }
    public void UpdateResep()
    {
        Manager_Audio.instance.PlaySFX(PaperAudio);
        
        Cek_Button();
        SO_Jamu jamu = List_Resep_Jamu[currentIndex];
        namaJamu.text = jamu.nama;
        for(int index = 0; index < containerGambarBahanBahan.childCount; index++)
        {
            Image currentImage = containerGambarBahanBahan.GetChild(index).GetComponent<Image>();
            if(currentImage.gameObject != formatGambar.gameObject)
            {
                Destroy(currentImage.gameObject);
            }
        }
        foreach(SO_BahanMentah bahanMentah in jamu.List_Bahan_Mentah)
        {
            Image currentBahan = Instantiate(formatGambar, containerGambarBahanBahan);
            currentBahan.gameObject.SetActive(true);
            if (bahanMentah.ikon_gameplay != null) 
            {
                currentBahan.sprite = bahanMentah.ikon_gameplay;
            }
        }
        for(int index = 0; index < containerMetode.childCount; index++)
        {
            TextMeshProUGUI currentMetode = containerMetode.GetChild(index).GetComponent<TextMeshProUGUI>();
            if(currentMetode.gameObject != formatMetode.gameObject)
            {
                Destroy(currentMetode.gameObject);
            }
        }
        foreach(SO_Jamu.Metode metode in jamu.List_Metode)
        {
            TextMeshProUGUI metodeKini = Instantiate(formatMetode, containerMetode);
            metodeKini.gameObject.SetActive(true);
            metodeKini.text = metode.langkah;
        }
    }
    public void Cek_Button()
    {
        HalamanSelanjutnya.gameObject.SetActive(currentIndex < maxIndex -1);
        HalamanSebelumnya.gameObject.SetActive(currentIndex > 0);
    }
    public void NextPage()
    {
        currentIndex++;
        UpdateResep();
    }
    public void PrevPage()
    {
        currentIndex--;
        UpdateResep();
    }
    public async void TutupResep()
    {
        Manager_Audio.instance.PlaySFX(PaperAudio);
        await containerTransform.DOLocalMoveY(-1620, 0.8f).SetEase(Ease.OutExpo).AsyncWaitForCompletion();
        canvas.gameObject.SetActive(false);
        currentIndex = 0;
    }
}
