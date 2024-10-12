using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewJamu", menuName = "Game/Jamu")]
public class Jamu : ScriptableObject
{
    public string namaJamu;
    public Sprite ikonGameplay;
    public Sprite ikonResep;
    public int level;
    public string deskripsi;
    public string manfaat;
    public List<Bahan> bahanBahan;
    public List<string> metodeLangkah;
    public int profitBase;
    public int expBase;

    public int TotalProfit => profitBase * level;
    public int TotalExp => expBase * level;
}
