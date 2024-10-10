using AimarWork;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Night_DropdownBeliBahan : MonoBehaviour
{
    private TMP_Dropdown tmp_dropdown;
    private SO_BahanMentah pilihanBahanMentah;
    [SerializeField] private Night_BeliBahan logika_BeliBahan;
    private void Awake()
    {
        tmp_dropdown = GetComponent<TMP_Dropdown>();
    }
    private void Start()
    {
        Update_ListBahanData();
    }
    public void Update_ListBahanData()
    {
        tmp_dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (SO_BahanMentah bahanMentah in logika_BeliBahan.List_BahanMentah)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = bahanMentah.nama;
            dropdownOptions.Add(optionData);
        }
        tmp_dropdown.AddOptions(dropdownOptions);
    }
    public void Set_JamuSO(int index)
    {
        pilihanBahanMentah = logika_BeliBahan.List_BahanMentah[index];
        Debug.Log("Pilihan Bahan Mentah " + pilihanBahanMentah.nama);
    }
}
