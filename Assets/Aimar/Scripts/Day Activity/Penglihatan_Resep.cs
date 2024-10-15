using AimarWork;
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

    [Header("UI Jamu")]
    public TextMeshProUGUI namaJamu;
    public Button HalamanSelanjutnya;
    public Button HalamanSebelumnya;

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
        foreach(SO_Jamu jamu in Manager_Jamu.instance.List_Jamu)
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

        currentIndex = 0;
        UpdateResep();
        canvas.gameObject.SetActive(true);
    }
    public void UpdateResep()
    {
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
            currentBahan.sprite = bahanMentah.ikon_gameplay;
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
    public void TutupResep()
    {
        canvas.gameObject.SetActive(false);
        currentIndex = 0;
    }
}
