using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSortByY : IComparer {
	int IComparer.Compare(object a,object b){
		Monster a1 = (Monster)a;
		Monster b1 = (Monster)b;
		if(a1.transform.localPosition.y > b1.transform.localPosition.y){
			int index = a1.transform.GetSiblingIndex ();
			int index2 = b1.transform.GetSiblingIndex ();
			a1.transform.SetSiblingIndex (index2);
			b1.transform.SetSiblingIndex (index);
			return -1;
		}
		return 1;
	}
}
