/**
* 命名空间: TFSG
*
* 功 能： N/A
* 类 名： equipData
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 $time$ 君仔 初版
*
* Copyright (c) 2015 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小君仔工作室 　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using UnityEngine;
using System.Collections;
namespace TFSG
{
    public class equipData
    {
        public string equipName;//装备名称
        public int equipId;//装备ID
        public string equipInfo;//装备描述
        public string equipType;//装备类型
        public string equipIcon;//装备图标
        public int attackValue;//装备伤害
        public int defenceValue;//装备防御
        public int HPBonus = 0;//血量加成
        public int AttackSpeedBonus = 0;//攻速加成
        public string pinzhi = "";//装备品质
        public int needHeroLevel = 0;
    }
}