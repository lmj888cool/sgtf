using UnityEngine;
using System.Collections;
public class skillData {
	public string skillName;//技能名称
	public int skillId;//技能ID
	public string skillInfo;//技能描述
	public int skillType;//技能类型
	public string skillIcon;//技能图标
	public string skillEffectName;
	public int attackType;
	public int attackNum;
	public int attackDamage;//技能初始伤害
	public int attackRange = 0;//伤害范围
	public float attackInterval;//攻击间隔（多次攻击的技能）
	public string music = "";//技能音效
	public float effectPriotY;//技能Y轴的描点
	public int stateDuration;//状态持续时间（击晕，减速，逆行，击飞等）
	public float stateChance;//状态的概率(1为100%)
	public int startAttackIndex;//播放到第几帧开始伤害
	public float shakeScreenNum;//是否震动屏幕 小于或等于0为不振动，其他正值为震动的秒数
}
