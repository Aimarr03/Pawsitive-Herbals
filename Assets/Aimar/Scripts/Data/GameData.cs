using System;
public class GameData
{
    public DateTime Data_Tanggal;
    public int exp_kini;
    public int uang_kini;

    public SerializableDictionary<string, JamuSaveData> Dictionary_DataJamu;
    public SerializableDictionary<string, BahanMentahSaveData> Dictionary_DataBahanMentah;

    public GameData()
    {
        Data_Tanggal = new DateTime(2024, 10, 1);
        exp_kini = 0;
        uang_kini = 0;

        Dictionary_DataJamu = new SerializableDictionary<string, JamuSaveData>();
        Dictionary_DataJamu.Add("Kunyit Asam", new JamuSaveData(1, true));
        Dictionary_DataJamu.Add("Beras Kencur", new JamuSaveData(0, false));
        Dictionary_DataJamu.Add("Temulawak", new JamuSaveData(0, false));
        Dictionary_DataJamu.Add("Wedang Jahe", new JamuSaveData(0, false));

        Dictionary_DataBahanMentah = new SerializableDictionary<string, BahanMentahSaveData>();
        Dictionary_DataBahanMentah.Add("Air", new BahanMentahSaveData(15));
        Dictionary_DataBahanMentah.Add("Asam Jawa", new BahanMentahSaveData(15));
        Dictionary_DataBahanMentah.Add("Gula Merah", new BahanMentahSaveData(15));
        Dictionary_DataBahanMentah.Add("Kunyit", new BahanMentahSaveData(15));
        
        Dictionary_DataBahanMentah.Add("Beras", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Garam", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Gula Pasir", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Jahe", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Kencur", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Pandan", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Serai", new BahanMentahSaveData(0));
        Dictionary_DataBahanMentah.Add("Temulawak", new BahanMentahSaveData(0));
    }
}
public struct JamuSaveData
{
    public int level;
    public bool terbuka;
    public JamuSaveData(int level, bool terbuka)
    {
        this.level = level;
        this.terbuka = terbuka;
    }
}
public struct BahanMentahSaveData
{
    int kuantitas;
    public BahanMentahSaveData(int kuantitas)
    {
        this.kuantitas = kuantitas;
    }
}
