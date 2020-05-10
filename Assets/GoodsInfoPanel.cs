using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsInfoPanel : UIScene
{
    public static GoodsInfoPanel _instance;
    private UILabel GoodsPropertyLabel;
    private UILabel GoodsDescribeLabel;
    private void Awake()
    {
        _instance = this;
        GoodsPropertyLabel = Helper.GetChild<UILabel>(this.transform, "GoodsPropertyLabel");
        GoodsDescribeLabel = Helper.GetChild<UILabel>(this.transform, "GoodsDescribeLabel");

    }
    protected override void Start()
    {
        base.Start();
    }
}
