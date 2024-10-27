using DG.Tweening;
using FadlanWork;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace AimarWork
{
    public class Pengambilan_Bahan : BaseInteractableObject
    {
        public List<SO_BahanMentah> List_BahanMentah;

        public List<SO_BahanMentah> pilihanKini;

        public Canvas canvas;
        public RectTransform background;

        public RectTransform Bahan_Container;
        public UI_BahanMentah format_PemilihanBahanMentah;

        public int maximum = 6;
        public TextMeshProUGUI TextMaksimalIndikasi;

        public AudioClip OpenDrawer;

        PlayerInventory inventory;
        Dictionary<SO_BahanMentah,UI_BahanMentah> Dictionary_UI_BahanMentah;

        public static event Action UpdateBahan;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            TextMaksimalIndikasi.text = $"Maksimal ambil bahan {pilihanKini.Count}/{maximum}";
            
            Manager_Audio.instance.PlaySFX(OpenDrawer);
            background.transform.DOMoveY(1540, 0);
            background.transform.DOMoveY(540, 0.8f).SetEase(Ease.InOutQuad);
            
            canvas.gameObject.SetActive(true);
            inventory = player.inventory;
            pilihanKini = new List<SO_BahanMentah>();
            if (inventory.ListBahan.Count > 0)
            {
                foreach(SO_BahanBase so_bahanMentah in inventory.ListBahan)
                {
                    if(so_bahanMentah is SO_BahanMentah)
                    {
                        pilihanKini.Add(so_bahanMentah as SO_BahanMentah);
                        bool ada = Dictionary_UI_BahanMentah.ContainsKey(so_bahanMentah as SO_BahanMentah);
                        Dictionary_UI_BahanMentah[so_bahanMentah as SO_BahanMentah].UpdateBahanMentah(ada);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                foreach(UI_BahanMentah UI_bahanMentah in Dictionary_UI_BahanMentah.Values)
                {
                    UI_bahanMentah.UpdateBahanMentah(false);
                }
            }
        }
        
        private void Start()
        {
            canvas.gameObject.SetActive(false);
            UI_BahanMentah.TambahBahanMentah += UI_BahanMentah_TambahBahanMentah;
            UI_BahanMentah.KurangBahanMentah += UI_BahanMentah_KurangBahanMentah;
            
            List_BahanMentah = Manager_TokoJamu.instance.GetSemuaBahanMentah();
            Dictionary_UI_BahanMentah = new Dictionary<SO_BahanMentah, UI_BahanMentah>();
            foreach(SO_BahanMentah bahanMentah in List_BahanMentah)
            {
                UI_BahanMentah UI_BahanMentah = Instantiate(format_PemilihanBahanMentah, Bahan_Container);
                UI_BahanMentah.gameObject.SetActive(true);
                UI_BahanMentah.SetUpBahanMentah(bahanMentah);
                UI_BahanMentah.pengambilan_bahan = this;
                Dictionary_UI_BahanMentah.Add(bahanMentah,UI_BahanMentah);
            }
            
            Dictionary_UI_BahanMentah = Dictionary_UI_BahanMentah.OrderBy(ui_bahan_mentah => ui_bahan_mentah.Value.so_BahanMentah.nama).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        private void OnDisable()
        {
            UI_BahanMentah.TambahBahanMentah -= UI_BahanMentah_TambahBahanMentah;
            UI_BahanMentah.KurangBahanMentah -= UI_BahanMentah_KurangBahanMentah;
        }

        private void UI_BahanMentah_KurangBahanMentah(SO_BahanMentah obj)
        {
            pilihanKini.Remove(obj);
            inventory.PenguranganBahan(obj);
            
            TextMaksimalIndikasi.text = $"Maksimal ambil bahan {pilihanKini.Count}/{maximum}";
            UpdateBahan?.Invoke();
        }

        private void UI_BahanMentah_TambahBahanMentah(SO_BahanMentah obj)
        {
            if(pilihanKini.Count >= maximum)
            {
                return;
            }
            pilihanKini.Add(obj);
            inventory.PenambahanBahan(obj);
            
            TextMaksimalIndikasi.text = $"Maksimal ambil bahan {pilihanKini.Count}/{maximum}";
            UpdateBahan?.Invoke();
        }

        public async void KonfirmasiPilihan()
        {
            Manager_Audio.instance.PlaySFX(OpenDrawer);
            await background.transform.DOMoveY(1540, 0.8f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
            canvas.gameObject.SetActive(false);
            inventory.PembarisanInventory();
        }
    }
}

