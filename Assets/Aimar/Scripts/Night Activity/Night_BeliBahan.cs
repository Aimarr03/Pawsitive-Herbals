using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AimarWork
{
    public class Night_BeliBahan : MonoBehaviour
    {
        public List<SO_BahanMentah> List_BahanMentah;
        [SerializeField] private List<SO_Jamu> List_Jamu;
        private void Awake()
        {
            List_BahanMentah = new List<SO_BahanMentah>();
            MemasukkanDataBahanTerbuka();
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
                Debug.Log("Enter Beli Bahan");
            }
        }
        private void MemasukkanDataBahanTerbuka()
        {
            foreach(SO_Jamu jamu in List_Jamu)
            {
                if (!jamu.terbuka) continue;
                foreach(SO_BahanMentah BahanMentah_jamu in jamu.List_Bahan_Mentah)
                {
                    if (List_BahanMentah.Contains(BahanMentah_jamu)) continue;
                    List_BahanMentah.Add(BahanMentah_jamu);
                }
            }
            List_BahanMentah = List_BahanMentah.OrderBy(jamu => jamu.nama).ToList();
        }
    }
}

