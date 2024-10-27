using AimarWork.GameManagerLogic;
using DG.Tweening;
using FadlanWork;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace AimarWork
{
    public class Night_BukuRiset : BaseInteractableObject
    {
        [TitleGroup("Deskripsi Umum")]
        [SerializeField] private Canvas UI_Canvas;
        [SerializeField] private RectTransform UI_BukuRiset;
        [SerializeField] private TextMeshProUGUI namaJamu;
        [SerializeField] private TextMeshProUGUI deskripsi;
        [SerializeField] private TextMeshProUGUI manfaat;

        [TitleGroup("Bahan Bahan")]
        [SerializeField] private RectTransform bahan_container;
        [SerializeField] private Image bahanFormat;

        [TitleGroup("Metode")]
        [SerializeField] private RectTransform metodeContainer;
        [SerializeField] private TextMeshProUGUI metodeFormat;

        [TitleGroup("Button")]
        [SerializeField] private Button RisetButton;
        [SerializeField] private Button SelanjutnyaButton;
        [SerializeField] private Button SebelumnyaButton;
        private int currentIndex = 0;

        [TitleGroup("Container Bintang dan Profit")]
        [SerializeField] private TextMeshProUGUI text_profit;
        [SerializeField] private RectTransform bintangContainer;
        [SerializeField] private Image bintangFormat;

        [TitleGroup("Container Kebutuhan Untuk Riset")]
        [TitleGroup("Container Kebutuhan Untuk Riset/Buka Jamu")]
        [SerializeField] private RectTransform Buka_Jamu;
        [SerializeField] private TextMeshProUGUI text_BukaExp;
        
        [TitleGroup("Container Kebutuhan Untuk Riset/Ningkat Jamu")]
        [SerializeField] private RectTransform Ningkat_Jamu;
        [SerializeField] private TextMeshProUGUI text_NingkatExp;
        [SerializeField] private RectTransform Container_BahanYangDiperlukan;
        [SerializeField] private RectTransform formatBahanYangDiperlukan;
        [SerializeField] private Color color_tidakAda;
        [SerializeField] private float alpha_tidakAda;

        public AudioClip PaperFlip;

        [SerializeField, Header("Data Jamu")] private List<SO_Jamu> Jamus = new List<SO_Jamu>();
        private int maxIndex => Jamus.Count;
        private SO_Jamu DataJamu => Jamus[currentIndex];
        private void Awake()
        {
            UI_Canvas.gameObject.SetActive(false);
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
        /*private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                
            }
        }*/
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            BukaBukuRiset();
        }
        public void BukaBukuRiset()
        {
            UI_Canvas.gameObject.SetActive(true);
            Manager_Audio.instance.PlaySFX(PaperFlip);
            UI_BukuRiset.transform.DOLocalMoveY(-1620, 0).SetEase(Ease.InOutQuad);
            UI_BukuRiset.transform.DOLocalMoveY(-540, 0.8f).SetEase(Ease.InOutQuad);
            DisplayDataJamu();
        }
        public async void TutupBukuRiset()
        {
            Manager_Audio.instance.PlaySFX(PaperFlip);
            await UI_BukuRiset.transform.DOLocalMoveY(-1620, 0.8f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
            UI_Canvas.gameObject.SetActive(false);
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
            for(int index = 0; index < Container_BahanYangDiperlukan.childCount; index++)
            {
                Transform BahanRisetKini = Container_BahanYangDiperlukan.transform.GetChild(index);
                if (BahanRisetKini != formatBahanYangDiperlukan.transform)
                {
                    Destroy(BahanRisetKini.gameObject);
                }
            }
            RisetButton.onClick.RemoveAllListeners();
            namaJamu.text = DataJamu.nama;
            
            
            Buka_Jamu.gameObject.SetActive(false);
            Ningkat_Jamu.gameObject.SetActive(false);

            int indexBintang = 0;
            int expDibutuhkan = DataJamu.GetExpDiperlukan();
            foreach (RectTransform bintangKini in bintangContainer)
            {
                bintangKini.gameObject.SetActive(indexBintang < DataJamu.level);
                indexBintang++;
            }

            if (DataJamu.terbuka)
            {
                RisetButton.onClick.AddListener(RisetMeningkatkanJamu);
                deskripsi.text = DataJamu.deskripsi;
                manfaat.text = "";
                foreach(string textManfaat in DataJamu.manfaat)
                {
                    manfaat.text += textManfaat + "\n";
                }
                
                for (int index = 0; index < DataJamu.List_Bahan_Mentah.Count; index++)
                {
                    //Debug.Log("Bahan dibuat");
                    Image bahanKini = Instantiate(bahanFormat, bahan_container);
                    bahanKini.gameObject.SetActive(true);
                    bahanKini.sprite = DataJamu.List_Bahan_Mentah[index].ikon_gameplay;
                }
                for (int index = 0; index < DataJamu.List_Metode.Count; index++)
                {
                    TextMeshProUGUI metodeKini = Instantiate(metodeFormat, metodeContainer);
                    metodeKini.gameObject.SetActive(true);

                    SO_Jamu.Metode metode = DataJamu.List_Metode[index];
                    metodeKini.text = "/u2022 "+metode.langkah;
                }
                text_profit.text = "Profit: " + DataJamu.GetBaseKeuntungan();
                Ningkat_Jamu.gameObject.SetActive(true);

                text_BukaExp.text = $"EXP: {expDibutuhkan}/{Manager_Game.instance.exp_kini}";
                bool resepLengkap = true;
                bool exp_cukup = Manager_Game.instance.exp_kini >= expDibutuhkan;
                foreach (SO_BahanMentah bahanMentah in DataJamu.List_Bahan_Mentah)
                {
                    RectTransform BahanYangDibutuhkan = Instantiate(formatBahanYangDiperlukan, Container_BahanYangDiperlukan);
                    BahanYangDibutuhkan.gameObject.SetActive(true);
                    Image komponenGambar = BahanYangDibutuhkan.GetChild(0).GetComponent<Image>();
                    if (bahanMentah.ikon_gameplay != null)
                    {
                        komponenGambar.sprite = bahanMentah.ikon_gameplay;
                    }
                    if (bahanMentah.kuantitasKini == 0)
                    {
                        komponenGambar.color = color_tidakAda;
                        resepLengkap = false;
                    }
                }
                RisetButton.interactable = resepLengkap && exp_cukup;
            }
            else
            {
                RisetButton.onClick.AddListener(RisetMembukaJamu);
                manfaat.text = "???";
                deskripsi.text = "???";
                text_profit.text = "Profit: ???";
                //bahan_bahan.text = "???";
                /*for (int index = 0; index < 3; index++)
                {
                    Image bahanKini = Instantiate(bahanFormat, bahan_container);
                    bahanKini.color = color_tidakAda;
                    bahanKini.gameObject.SetActive(true);
                }*/
                for (int index = 0; index < 3; index++)
                {
                    TextMeshProUGUI metodeKini = Instantiate(metodeFormat, metodeContainer);
                    metodeKini.gameObject.SetActive(true);
                    metodeKini.text = "???";
                }

                Buka_Jamu.gameObject.SetActive(true);
                
                text_BukaExp.text = $"EXP: {expDibutuhkan}/{Manager_Game.instance.exp_kini}";
                RisetButton.interactable = Manager_Game.instance.exp_kini >= expDibutuhkan;
            }
        }
        private void RisetMembukaJamu()
        {
            Debug.Log("Riset Membuka Jamu");
            DataJamu.terbuka = true;
            DataJamu.level++;
            DisplayDataJamu();
            Manager_Game.instance.exp_kini -= DataJamu.GetExpDiperlukan();
        }
        private void RisetMeningkatkanJamu()
        {
            Debug.Log("Riset Ningkatin Jamu");
            DataJamu.level++;
            DisplayDataJamu();
            foreach(SO_BahanMentah bahanMentah in DataJamu.List_Bahan_Mentah)
            {
                bahanMentah.kuantitasKini--;
            }
            Manager_Game.instance.exp_kini -= DataJamu.GetExpDiperlukan();
        }
        public void HalamanSelanjutnya()
        {
            currentIndex++;
            Manager_Audio.instance.PlaySFX(PaperFlip);
            DisplayDataJamu();
        }
        public void HalamanSebelumnya()
        {
            currentIndex--;
            Manager_Audio.instance.PlaySFX(PaperFlip);
            DisplayDataJamu();
        }
    }
}

