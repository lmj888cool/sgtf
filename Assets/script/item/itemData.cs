using UnityEngine;
using System.Collections;
public class itemData {
	public string itemName;//装备名称
	public int itemId;//装备ID
	public string itemInfo;//装备描述
	public int itemType;//装备类型
	public string itemIcon;//装备图标
	public string itemEffectName;
	public int attackType;
	public int attackNum;
	public int attackDamage;//装备初始伤害
	public int attackRange = 0;//伤害范围
	public float attackInterval;//攻击间隔（多次攻击的装备）
	public string music = "";//装备音效
	public float effectPriotY;//装备Y轴的描点
	public int stateDuration;//状态持续时间（击晕，减速，逆行，击飞等）
	public float stateChance;//状态的概率(1为100%)
	public int startAttackIndex;//播放到第几帧开始伤害
	public float shakeScreenNum;//是否震动屏幕 小于或等于0为不振动，其他正值为震动的秒数
}
