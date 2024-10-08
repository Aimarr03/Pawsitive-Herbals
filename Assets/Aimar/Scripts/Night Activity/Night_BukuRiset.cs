using AimarWork;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Night_BukuRiset : MonoBehaviour
{
    [SerializeField] private Canvas UI_BukuRiset;
    [SerializeField] private TextMeshProUGUI namaJamu;
    [SerializeField] private TextMeshProUGUI deskripsi;
    [SerializeField] private TextMeshProUGUI manfaat;
    [SerializeField] private TextMeshProUGUI bahan_bahan;
    [SerializeField] private TextMeshProUGUI metode;
    [SerializeField] private Button RisetButton;
    private int currentIndex = 0;
    [SerializeField, Header("Data Jamu")]private List<SO_Jamu> Jamus = new List<SO_Jamu>();
    private int maxIndex => Jamus.Count;
    private SO_Jamu DataJamu => Jamus[currentIndex];
    private void Awake()
    {
        UI_BukuRiset.gameObject.SetActive(false);
        Jamus.OrderBy(jamus => jamus.terbuka);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
    }
    private void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UI_BukuRiset.gameObject.SetActive(true);
            DisplayDataJamu();
        }
    }
    public void TutupBukuRiset()
    {
        UI_BukuRiset.gameObject.SetActive(false);
        currentIndex = 0;
    }
    public void DisplayDataJamu()
    {
        RisetButton.onClick.RemoveAllListeners();
        if (DataJamu.terbuka)
        {
            RisetButton.onClick.AddListener(RisetMeningkatkanJamu);
            namaJamu.text = DataJamu.nama;
            deskripsi.text = DataJamu.deskripsi;
            manfaat.text = DataJamu.manfaat;
        }
        else
        {
            RisetButton.onClick.AddListener(RisetMembukaJamu);
            manfaat.text = "???";
            deskripsi.text = "???";
            //bahan_bahan.text = "???";
            metode.text = "???";
        }
    }
    private void RisetMembukaJamu()
    {
        Debug.Log("Riset Membuka Jamu");
        DataJamu.terbuka = true;
        DataJamu.level++;
        DisplayDataJamu();
    }
    private void RisetMeningkatkanJamu()
    {
        Debug.Log("Riset Ningkatin Jamu");
        DataJamu.level++;
        DisplayDataJamu();
    }
}
