using AimarWork.GameManagerLogic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Money : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    int uangKini = 0;
    private void Start()
    {
        Manager_Game.instance.OnChangeUang += Instance_OnChangeUang;
        Text.text = Manager_Game.instance.uang_kini.ToString();
    }
    private void OnDisable()
    {
        Manager_Game.instance.OnChangeUang -= Instance_OnChangeUang;
    }

    private void Instance_OnChangeUang()
    {
        StartCoroutine(StartChanging(Manager_Game.instance.uang_kini));
    }
    private IEnumerator StartChanging(int UangTerbaru)
    {
        int currentValue = uangKini;
        float duration = 0.5f; 
        float elapsed = 0f; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int newValue = (int)Mathf.Lerp(currentValue, UangTerbaru, elapsed / duration);
            Text.text = "Rp. " + newValue.ToString("N0"); 
            yield return null; 
        }

        Text.text = "Rp. " + UangTerbaru.ToString("N0");
        uangKini += UangTerbaru;
    }
}
