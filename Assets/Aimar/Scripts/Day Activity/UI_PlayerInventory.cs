using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AimarWork
{
    public class UI_PlayerInventory : MonoBehaviour
    {
        public RectTransform Container;
        private void Start()
        {
            PlayerInventory.List_Berubah += PlayerInventory_List_Berubah;
        }
        private void OnDisable()
        {
            PlayerInventory.List_Berubah -= PlayerInventory_List_Berubah;
        }
        private void PlayerInventory_List_Berubah(List<SO_BahanBase> obj)
        {
            Bersihkan_Container();
            for(int index =  0; index < obj.Count; index++)
            {
                Transform UI_Image = Container.GetChild(index);

                Image Data_Image = UI_Image.GetChild(0).GetComponent<Image>();
                Data_Image.enabled = true;
                Data_Image.sprite = obj[index].ikon_gameplay;
            }
        }
        private void Bersihkan_Container()
        {
            for(int index = 0; index < Container.childCount; index++)
            {
                Transform UI_Image = Container.GetChild(index);

                Image Data_Image = UI_Image.GetChild(0).GetComponent<Image>();
                Data_Image.enabled = false;
                Data_Image.sprite = null;
                
            }
        }
    }
}

