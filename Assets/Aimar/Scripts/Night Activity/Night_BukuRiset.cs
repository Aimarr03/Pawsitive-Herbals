using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace AimarWork
{
    public class Night_BukuRiset : MonoBehaviour
    {
        [Header("Deskripsi Umum")]
        [SerializeField] private Canvas UI_BukuRiset;
        [SerializeField] private TextMeshProUGUI namaJamu;
        [SerializeField] private TextMeshProUGUI deskripsi;
        [SerializeField] private TextMeshProUGUI manfaat;

        [Header("Bahan Bahan")]
        [SerializeField] private RectTransform bahan_container;
        [SerializeField] private Image bahanFormat;

        [Header("Metode")]
        [SerializeField] private RectTransform metodeContainer;
        [SerializeField] private TextMeshProUGUI metodeFormat;

        [Header("Button")]
        [SerializeField] private Button RisetButton;
        [SerializeField] private Button SelanjutnyaButton;
        [SerializeField] private Button SebelumnyaButton;
        private int currentIndex = 0;
        [SerializeField, Header("Data Jamu")] private List<SO_Jamu> Jamus = new List<SO_Jamu>();
        private int maxIndex => Jamus.Count;
        private SO_Jamu DataJamu => Jamus[currentIndex];
        private void Awake()
        {
            UI_BukuRiset.gameObject.SetActive(false);
            Jamus.OrderBy(jamus => jamus.terbuka);
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Debug.Log("Mouse Enter");
        }
        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Debug.Log("Mouse Exit");
        }
        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Enter Buku Riset");
                BukaBukuRiset();
            }
        }
        public void BukaBukuRiset()
        {
            UI_BukuRiset.gameObject.SetActive(true);
            DisplayDataJamu();
        }
        public void TutupBukuRiset()
        {
            UI_BukuRiset.gameObject.SetActive(false);
            currentIndex = 0;
        }
        public void DisplayDataJamu()
        {
            SebelumnyaButton.gameObject.SetActive(currentIndex > 0);
            SelanjutnyaButton.gameObject.SetActive(currentIndex < maxIndex - 1);
            for (int index = 0; index < bahan_container.childCount; index++)
            {
                Transform bahanKini = bahan_container.transform.GetChild(index);
                if (bahanKini != bahanFormat.transform)
                {
                    Destroy(bahanKini.gameObject);
                }
            }
            for (int index = 0; index < metodeContainer.childCount; index++)
            {
                Transform metodeKini = metodeContainer.transform.GetChild(index);
                if (metodeKini != metodeFormat.transform)
                {
                    Destroy(metodeKini.gameObject);
                }
            }
            RisetButton.onClick.RemoveAllListeners();
            namaJamu.text = DataJamu.nama;
            if (DataJamu.terbuka)
            {
                RisetButton.onClick.AddListener(RisetMeningkatkanJamu);
                deskripsi.text = DataJamu.deskripsi;
                manfaat.text = DataJamu.manfaat;
                for (int index = 0; index < DataJamu.List_Bahan_Mentah.Count; index++)
                {
                    //Debug.Log("Bahan dibuat");
                    Image bahanKini = Instantiate(bahanFormat, bahan_container);
                    bahanKini.gameObject.SetActive(true);
                }
                for (int index = 0; index < DataJamu.List_Metode.Count; index++)
                {
                    TextMeshProUGUI metodeKini = Instantiate(metodeFormat, metodeContainer);
                    metodeKini.gameObject.SetActive(true);

                    SO_Jamu.Metode metode = DataJamu.List_Metode[index];
                    metodeKini.text = metode.langkah;
                }
            }
            else
            {
                RisetButton.onClick.AddListener(RisetMembukaJamu);
                manfaat.text = "???";
                deskripsi.text = "???";
                //bahan_bahan.text = "???";
                for (int index = 0; index < 3; index++)
                {
                    Image bahanKini = Instantiate(bahanFormat, bahan_container);
                    bahanKini.gameObject.SetActive(true);
                }
                for (int index = 0; index < 3; index++)
                {
                    TextMeshProUGUI metodeKini = Instantiate(metodeFormat, metodeContainer);
                    metodeKini.gameObject.SetActive(true);
                    metodeKini.text = "???";
                }
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
        public void HalamanSelanjutnya()
        {
            currentIndex++;
            DisplayDataJamu();
        }
        public void HalamanSebelumnya()
        {
            currentIndex--;
            DisplayDataJamu();
        }
    }
}

