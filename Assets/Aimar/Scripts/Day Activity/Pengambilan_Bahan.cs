using FadlanWork;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    public class Pengambilan_Bahan : BaseInteractableObject
    {
        public List<SO_BahanMentah> List_BahanMentah;

        public List<SO_BahanMentah> pilihanKini;

        public Canvas canvas;

        public RectTransform Bahan_Container;
        public UI_BahanMentah format_PemilihanBahanMentah;

        PlayerInventory inventory;
        Dictionary<SO_BahanMentah,UI_BahanMentah> Dictionary_UI_BahanMentah;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
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
        }

        private void Start()
        {
            canvas.gameObject.SetActive(false);
            UI_BahanMentah.TambahBahanMentah += UI_BahanMentah_TambahBahanMentah;
            UI_BahanMentah.KurangBahanMentah += UI_BahanMentah_KurangBahanMentah;
            
            List_BahanMentah = Manager_Jamu.instance.GetSemuaBahanMentah();
            Dictionary_UI_BahanMentah = new Dictionary<SO_BahanMentah, UI_BahanMentah>();
            foreach(SO_BahanMentah bahanMentah in List_BahanMentah)
            {
                UI_BahanMentah UI_BahanMentah = Instantiate(format_PemilihanBahanMentah, Bahan_Container);
                UI_BahanMentah.gameObject.SetActive(true);
                UI_BahanMentah.SetUpBahanMentah(bahanMentah);
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
            if(inventory.ListBahan.Contains(obj))
            {
                inventory.ListBahan.Remove(obj);
            }
        }

        private void UI_BahanMentah_TambahBahanMentah(SO_BahanMentah obj)
        {
            pilihanKini.Add(obj);
        }

        public void KonfirmasiPilihan()
        {
            canvas.gameObject.SetActive(false);
            pilihanKini = pilihanKini.OrderBy(bahan => bahan.nama).ToList();

            inventory.PenambahanBahan(pilihanKini);
        }
    }
}

