using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductIPAData
{
    public string productName;
    public string productId;
    public string priceString;
    public string currencyCode;
    
    
    public ProductIPAData() {}

    public ProductIPAData(string productName, string productId, string priceString,
        string currencyCode)
    {
        this.productName = productName;
        this.productId = productId;
        this.priceString = priceString;
        this.currencyCode = currencyCode;
    }
}

public static class MG_ProductData
{
    public static ProductIPAData NoAds_Pack = new ProductIPAData("Remove_Ads", "gm.removeads", "1.99", "USD");
    public static ProductIPAData Gem_1_Pack = new ProductIPAData("Gem_1", "gm.g100", "0.99", "USD");
    public static ProductIPAData Gem_2_Pack = new ProductIPAData("Gem_2", "gm.g250", "2.99", "USD");
    public static ProductIPAData Gem_3_Pack = new ProductIPAData("Gem_3", "gm.g600", "4.99", "USD");

    public static MG_RewardProduct NoAdsReward = new MG_RewardProduct(MG_ProductType.NoAds, 0, 20);
    public static MG_RewardProduct Gem_1_Reward = new MG_RewardProduct(MG_ProductType.Gem, 100);
    public static MG_RewardProduct Gem_2_Reward = new MG_RewardProduct(MG_ProductType.Gem, 350);
    public static MG_RewardProduct Gem_3_Reward = new MG_RewardProduct(MG_ProductType.Gem, 600);
    
}

public enum MG_ProductType
{
    NoAds,
    Gem
}

public class MG_RewardProduct
{
    public MG_ProductType type;
    public int amount;
    public int bonus;

    public MG_RewardProduct(MG_ProductType type, int amount, int bonus = 0)
    {
        this.type = type;
        this.amount = amount;
        this.bonus = bonus;
    }
}

