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
        List<UI_BahanMentah> List_UI_BahanMentah;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            canvas.gameObject.SetActive(true);
            pilihanKini = new List<SO_BahanMentah>();
            inventory = player.inventory;
        }

        private void Start()
        {
            canvas.gameObject.SetActive(false);
            UI_BahanMentah.TambahBahanMentah += UI_BahanMentah_TambahBahanMentah;
            UI_BahanMentah.KurangBahanMentah += UI_BahanMentah_KurangBahanMentah;
            List_BahanMentah = Manager_Jamu.instance.GetSemuaBahanMentah();
            List_UI_BahanMentah = new List<UI_BahanMentah>();
            foreach(SO_BahanMentah bahanMentah in List_BahanMentah)
            {
                UI_BahanMentah UI_BahanMentah = Instantiate(format_PemilihanBahanMentah, Bahan_Container);
                UI_BahanMentah.gameObject.SetActive(true);
                UI_BahanMentah.SetUpBahanMentah(bahanMentah);
                List_UI_BahanMentah.Add(UI_BahanMentah);
            }
            List_UI_BahanMentah = List_UI_BahanMentah.OrderBy(ui_bahan_mentah => ui_bahan_mentah.so_BahanMentah.nama).ToList();
        }
        private void OnDisable()
        {
            UI_BahanMentah.TambahBahanMentah -= UI_BahanMentah_TambahBahanMentah;
            UI_BahanMentah.KurangBahanMentah -= UI_BahanMentah_KurangBahanMentah;
        }

        private void UI_BahanMentah_KurangBahanMentah(SO_BahanMentah obj)
        {
            pilihanKini.Remove(obj);
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
        public void BatalPilihan()
        {

        }
    }
}

