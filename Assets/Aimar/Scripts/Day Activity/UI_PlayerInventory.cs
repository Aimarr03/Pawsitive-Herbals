using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AimarWork
{
    public class UI_PlayerInventory : MonoBehaviour
    {
        public Image UI_Image_Format;
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
                Image UI_Image = Instantiate(UI_Image_Format, Container);
                UI_Image.gameObject.SetActive(true);

                UI_Image.sprite = obj[index].ikon_gameplay;
            }
        }
        private void Bersihkan_Container()
        {
            for(int index = 0; index < Container.childCount; index++)
            {
                Image UI_Image = Container.GetChild(index).GetComponent<Image>();
                if(UI_Image.gameObject != UI_Image_Format.gameObject)
                {
                    Destroy(UI_Image.gameObject);
                }
            }
        }
    }
}

