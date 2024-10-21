using AimarWork.GameManagerLogic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Money : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;

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
    private IEnumerator StartChanging(int targetValue)
    {
        int currentValue = int.Parse(Text.text); 
        float duration = 0.5f; 
        float elapsed = 0f; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int newValue = (int)Mathf.Lerp(currentValue, targetValue, elapsed / duration);
            Text.text = newValue.ToString("N0"); 
            yield return null; 
        }

        Text.text = targetValue.ToString("N0");
    }
}
