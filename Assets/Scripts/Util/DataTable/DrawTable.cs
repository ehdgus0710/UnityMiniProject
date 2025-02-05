using System.Collections.Generic;
using UnityEngine;

public class CoinDrawData
{
    public float Normal {  get; set; }
    public float Rare { get; set; }
    public float Epic { get; set; }


    //public override string ToString()
    //{
    //    return $"Type : {Type}\nName : {Name}\nDesc : {Desc}\nValue : {Value}\nCost : {Cost}\nIcon : {Icon}\n";
    //}
}


public class CoinDrawTable : DataTable
{
    private List<CoinDrawData> list = new List<CoinDrawData>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        list.Clear();
        list = LoadCSV<CoinDrawData>(textAsset.text);


        foreach (var item in list)
        {
            if (item == null)
            {
                Debug.LogError($"Key Duplicated");
            }
        }
    }

    public CoinDrawData Get(int index)
    {
        if (list.Count <= index)
        {
            return default;
        }

        return list[index];
    }
}