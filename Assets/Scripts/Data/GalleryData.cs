using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class GalleryData
{
    public int ID;
    public string Name;
    public int Capacity;
    public int[] IDRelics;
}

public class GalleryRelicData
{
    public int IDRelic;
    public int IDGallery;
    public int Position; // Position in Gallery
    public string LastTimer;

    public GalleryRelicData()
    {
    }

    public GalleryRelicData(int iDGallery, int iDRelic, int position, string lastTimer)
    {
        IDGallery = iDGallery;
        IDRelic = iDRelic;
        Position = position; //[1 - Capacity]
        LastTimer = lastTimer;
    }
}

public class RelicData
{
    public int ID;
    public string Name;
    public string Description;
    public string ArtPath;
    public int Material = 10; //Material need to play Challenge
    public int Coin; // Coin Reward after Timer
    public int Timer; //Minute
}
