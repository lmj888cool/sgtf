using UnityEngine;
using System.Collections;
using SimpleJson;
namespace TFSG {
	public class heroData {
		public int heroId;
		public string heroName;
		public string heroIcon;
		public string heroStyle;
		public string heroDescription;
		public int heroSkill;
		public int heroLv;
        public int attackRange;//攻击范围
        public float speed;//攻速
        public int damage;//伤害
        public bool isInFight = false;
		public string color;
		//public int weapon = 0;
		//public int armor = 0;
		//public int shoes = 0;
		//public int amulet = 0;
		public JsonObject jo;
	}
}
