using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using TFSG;
using SimpleJson;
using Spine;
using Spine.Unity;
public class DataManager {

	private static DataManager _dataManager;
	public Dictionary<int,skillData> skillDic;
	public Dictionary<int,heroData> heroDic;
	public Dictionary<int,monsterData> monsterDic;
	public Dictionary<int,chapterData> chapterDic;
	public Dictionary<int,equipData> equipDic;//装备
	public Dictionary<int,itemData> itemDic;//物品


	public Dictionary<string,Dictionary<int, JsonObject>> dataDic;
    /// <summary>
    /// //////////////////////////////////
    /// </summary>
    /// 
	public Dictionary<int, JsonObject> skillDicJson;
	public Dictionary<int, JsonObject> heroDicJson;
	public Dictionary<int, JsonObject> chapterDicJson;
	public Dictionary<int, JsonObject> campaignDicJson;
	public Dictionary<int, JsonObject> monsterDicJson;
	public Dictionary<int, JsonObject> equipDicJson;
	public Dictionary<int, JsonObject> itemDicJson;
	public Dictionary<int, JsonObject> levelUpJson;
	public Dictionary<int, JsonObject> starUpJson;
	public Dictionary<int, JsonObject> suitJson;
	public Dictionary<int, JsonObject> soundJson;
	public Dictionary<int, JsonObject> languageJson;
    public Dictionary<string,ArrayList> chapterWaveData;
	public static JsonObject playerData;
	public static DataManager getInstance(){//获取单例
		if(_dataManager == null){
			_dataManager = new DataManager();
			//初始化配置表们
			_dataManager.initHeroData();
			_dataManager.initSkillData();
			_dataManager.initChapterData();
			_dataManager.initMonsterData();

            _dataManager.skillDicJson = _dataManager.getJson("data/skill", "\r\n");
            _dataManager.heroDicJson = _dataManager.getJson("data/hero", "\r\n");
            _dataManager.chapterDicJson = _dataManager.getJson("data/chapter", "\r\n");
            _dataManager.monsterDicJson = _dataManager.getJson("data/monster", "\r\n");
			_dataManager.equipDicJson = _dataManager.getJson("data/equipment", "\r\n");
            _dataManager.itemDicJson = _dataManager.getJson("data/item", "\r\n");
			_dataManager.campaignDicJson = _dataManager.getJson("data/campaign", "\r\n");
			_dataManager.levelUpJson = _dataManager.getJson("data/levelup", "\r\n");
			_dataManager.starUpJson = _dataManager.getJson("data/starup", "\r\n");
			_dataManager.suitJson = _dataManager.getJson("data/suit", "\r\n");
			_dataManager.soundJson = _dataManager.getJson("data/sound", "\r\n");
			_dataManager.languageJson = _dataManager.getJson("data/language", "\r\n");
			_dataManager.dataDic ["item"] = _dataManager.itemDicJson;
			_dataManager.dataDic ["equipment"] = _dataManager.equipDicJson;
			_dataManager.dataDic ["chapter"] = _dataManager.chapterDicJson;
			_dataManager.dataDic ["hero"] = _dataManager.heroDicJson;
			_dataManager.dataDic ["skill"] = _dataManager.skillDicJson;
			_dataManager.dataDic ["monster"] = _dataManager.monsterDicJson;
			_dataManager.dataDic ["campaign"] = _dataManager.campaignDicJson;
			_dataManager.dataDic ["levelUp"] = _dataManager.levelUpJson;
			_dataManager.dataDic ["starUp"] = _dataManager.starUpJson;
			_dataManager.dataDic ["suit"] = _dataManager.suitJson;



        }
		return _dataManager;
	}

	public DataManager(){
		skillDic = new Dictionary<int, skillData> ();
		heroDic = new Dictionary<int, heroData> ();
		monsterDic = new Dictionary<int, monsterData> ();
		chapterDic = new Dictionary<int, chapterData> ();
		chapterWaveData = new Dictionary<string,ArrayList> ();
		dataDic = new Dictionary<string, Dictionary<int, JsonObject>> ();

	}

	public string [][] getData (string path,string _split,int fromIndex = 2) {
		string [][] data;
		//读取csv二进制文件  
		TextAsset binAsset = Resources.Load (path, typeof(TextAsset)) as TextAsset;         

		if (binAsset != null) {
			//读取每一行的内容  
			string[] lineArray = binAsset.text.Split (_split.ToCharArray (), StringSplitOptions.RemoveEmptyEntries);

			//创建二维数组  
			data = new string [lineArray.Length - fromIndex][];  

			//把csv中的数据储存在二位数组中  
			int index = 0;
			for (int i = fromIndex; i < lineArray.Length; i++) {  
				data [index] = lineArray [i].Split (',');
				index++;
			} 
		} else {
			data = new string [0][]; 
		}

		return data;
	}
    public Dictionary<int, JsonObject> getJson(string path, string _split)
    {
        Dictionary<int, JsonObject> JsonObjects = new Dictionary<int, JsonObject>();
        //读取csv二进制文件  
        TextAsset binAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;

        if (binAsset != null)
        {
            //读取每一行的内容  
            string[] lineArray = binAsset.text.Split(_split.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            
            string[] key = lineArray[0].Split(',');
            //把csv中的数据储存在二位数组中  
            for (int i = 1; i < lineArray.Length; i++)
            {
				//创建二维数组  
				JsonObject data = new JsonObject();
                string[] values = lineArray[i].Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    data[key[j]] = values[j];

                }
				int id = int.Parse(data[0].ToString());
				JsonObjects[id] = data;
            }
            
        }
       

        return JsonObjects;
    }
    public void initChapterData(){
		string[][] arrAll = getData ("data/chapter","\r\n");
		for (int i = 0; i < arrAll.Length; i++) { 
			string[] arr = arrAll [i];
			int chapterId = int.Parse (arr[0]);
			chapterData chapter = new chapterData ();
			chapter.chapterId= chapterId;
			chapter.chapterName = arr[1];
			chapter.chapterMap = arr[2];
			chapter.chapterMonster = arr[3];
			chapter.chapterTower = arr[4];
			chapter.chapterLoveNum = int.Parse(arr[5]);
			string[] HpAdd = arr[6].Split('_');
			chapter.chapterHpAdd1 = int.Parse(HpAdd[0]);
			chapter.chapterHpAdd2 = int.Parse(HpAdd[1]);
			chapterDic [chapterId] = chapter;
		}
	}
	public void initHeroData(){
		string[][] arrAll = getData ("data/hero","\r\n");
		for (int i = 0; i < arrAll.Length; i++) {
			string[] arr = arrAll [i];
			int heroId = int.Parse (arr[0]);
			heroData hero = new heroData ();
			hero.heroId= heroId;
			hero.heroName = arr[1];
			hero.heroIcon = arr[2];
			hero.heroStyle = arr[3];
			hero.heroDescription = arr[4];
            hero.attackRange = int.Parse(arr[5]);
            hero.speed = float.Parse(arr[6]);
            hero.damage = int.Parse(arr[7]);
            hero.heroSkill = int.Parse(arr[8]);
			heroDic[heroId] = hero;
		}
	}
	public void initSkillData(){
		string[][] arrAll = getData ("data/skill","\r\n");
		for (int i = 0; i < arrAll.Length; i++) { 
			string[] arr = arrAll [i];
			int skillId = int.Parse (arr[0]);
			skillData skill = new skillData ();
			skill.skillId = skillId;
			skill.skillName = arr[1];
			skill.skillIcon = arr[2];
			skill.skillInfo = arr[5];
			skill.skillEffectName = arr[6];
			skill.attackType = int.Parse(arr[3]);
			skill.skillType = int.Parse(arr[4]);
			skill.attackNum = int.Parse(arr[7]);
			skill.attackInterval = float.Parse(arr[8]);
			skill.music = arr[9];
			skill.attackRange = int.Parse(arr[10]);
			skill.effectPriotY = float.Parse (arr[11]);
			skill.stateDuration = int.Parse (arr[12]);//1000开头的是击晕，2000开头的是减速，3000开头的是逆行
			skill.stateChance = float.Parse (arr[13]);
			skill.attackDamage = int.Parse (arr[14]);
			skill.startAttackIndex = int.Parse (arr[15]);
			skill.shakeScreenNum = float.Parse (arr[16]);
			//skill.converseTime = int.Parse (arr[19]);
			skillDic [skillId] = skill;
		}
	}
	public void initMonsterData(){
		string[][] arrAll = getData ("data/monster","\r\n");
		for (int i = 0; i < arrAll.Length; i++) { 
			string[] arr = arrAll [i];
			int monsterId = int.Parse (arr[0]);
			monsterData monster = new monsterData ();
			monster.monsterId= monsterId;
			monster.monsterName = arr[1];
			monster.monsterHp = int.Parse(arr[3]);
			monster.monsterStyle = arr[2];
			monster.monsterType = int.Parse(arr[4]);
			monster.defence = int.Parse(arr[5]);
			monster.magicDefence = int.Parse(arr[6]);
			monster.moveSpeed = float.Parse(arr[7]);
			monster.moveType = int.Parse(arr[8]);
            monster.monsterDescription = arr[9];
			monster.monsterDamage = int.Parse(arr[10]);

			monsterDic [monsterId] = monster;
		}
	}
    public void initEquipData()
    {
        string[][] arrAll = getData("data/equipment", "\r\n");
        for (int i = 0; i < arrAll.Length; i++)
        {
            string[] arr = arrAll[i];
            int equipId = int.Parse(arr[0]);
            equipData equip = new equipData();
            equip.equipId = equipId;
            equip.equipName = arr[1];
            equip.equipIcon = arr[10];
            equip.equipInfo = arr[3];
            equip.equipType = arr[4];
            equip.attackValue = int.Parse(arr[5]);
            equip.defenceValue = int.Parse(arr[6]);
            equip.needHeroLevel = int.Parse(arr[9]);
            equip.pinzhi= arr[8];
            //equip.converseTime = int.Parse (arr[19]);
            equipDic[equipId] = equip;
        }
    }
    public void initItemData()
    {
        string[][] arrAll = getData("data/item", "\r\n");
        for (int i = 0; i < arrAll.Length; i++)
        {
            string[] arr = arrAll[i];
            int itemId = int.Parse(arr[0]);
            itemData item = new itemData();
            item.itemId = itemId;
            item.itemName = arr[1];
            item.itemIcon = arr[2];
            item.itemInfo = arr[5];
            item.itemEffectName = arr[6];
            item.attackType = int.Parse(arr[3]);
            item.itemType = int.Parse(arr[4]);
            item.attackNum = int.Parse(arr[7]);
            item.attackInterval = float.Parse(arr[8]);
            item.music = arr[9];
            item.attackRange = int.Parse(arr[10]);
            item.effectPriotY = float.Parse(arr[11]);
            item.stateDuration = int.Parse(arr[12]);//1000开头的是击晕，2000开头的是减速，3000开头的是逆行
            item.stateChance = float.Parse(arr[13]);
            item.attackDamage = int.Parse(arr[14]);
            item.startAttackIndex = int.Parse(arr[15]);
            item.shakeScreenNum = float.Parse(arr[16]);
            //item.converseTime = int.Parse (arr[19]);
            itemDic[itemId] = item;
        }
    }
    public ArrayList initChapterWaveData(string wavePath){
		if (chapterWaveData.ContainsKey (wavePath)) {			
			return (ArrayList)chapterWaveData [wavePath];
		}
		if(wavePath == ""){
			return new ArrayList ();
		}
		ArrayList pathArr = new ArrayList ();
		string[][] waveArr = DataManager.getInstance ().getData (wavePath, "\r\n",0);


		for (int waveIndex = 0; waveIndex < waveArr.Length - 1; waveIndex++) { 
			//if (waveArr.Length > waveIndex) {
				Vector3 oldtWave = new Vector3 (int.Parse (waveArr [waveIndex] [0]), int.Parse (waveArr [waveIndex] [1]),int.Parse (waveArr [waveIndex] [2]));
				//Array pos = new float[3];
				//pos [0] = int.Parse (waveArr [0] [0]);
				//pos [1] = int.Parse (waveArr [0] [1]);
				//pos [2] = int.Parse (waveArr [0] [2]);
				pathArr.Add (oldtWave);
				Vector3 currentWave = new Vector3 (0, 0, 0);
				int nextIndex = waveIndex + 1;
				currentWave.x = int.Parse (waveArr [nextIndex] [0]);
				currentWave.y = int.Parse (waveArr [nextIndex] [1]);
				int currentDir = int.Parse (waveArr [nextIndex] [2]);
				double distance = GetDistance (currentWave, oldtWave);
				int moveNum = (int)System.Math.Abs(distance);
				float movex = (currentWave.x - oldtWave.x) / moveNum;
				float movey = (currentWave.y - oldtWave.y) / moveNum;
				
				for (int i = 0; i < moveNum; i++) {
					
					//Array pos2 = new float[3];
					oldtWave.x +=movex;
					oldtWave.y +=movey;
					Vector3 Wave = new Vector3 (oldtWave.x, oldtWave.y, currentDir);
					//Wave.z = oldtWave.z;
					pathArr.Add (Wave);
				}
				//oldtWave = currentWave;
				//waveIndex++;
				//setWalkDir ();
			//} 
		}
		chapterWaveData [wavePath] = pathArr;
		return pathArr;
	}
	public  double GetDistance(Vector3 startPoint, Vector3 endPoint)
	{
		float x = System.Math.Abs(endPoint.x - startPoint.x);
		float y = System.Math.Abs(endPoint.y - startPoint.y);
		return System.Math.Sqrt(x * x + y * y);
	} 

	public Color getColor(string color){
		if (color == "white") {
			return Color.white;
		} else if (color == "green") {
			return Color.green;
		} else if (color == "blue") {
			return new Color(0.15f,0.84f,1.0f,1.0f);
		} else if (color == "yellow") {
			return Color.yellow;
		} else if (color == "red") {
			return Color.red;
		} else if (color == "purple") {
			return new Color(207,0,229);
		} else {
			return Color.white;
		}

	}
	public JsonObject getItemDataById(int id){
		if (id > 8000) {//装备
			return equipDicJson[id];
		} else {
			return itemDicJson[id];
		}
		return null;
	}
	public JsonObject getSuitByEquip(JsonObject equip){
		foreach (KeyValuePair<int,JsonObject> kvp in suitJson) {
			JsonObject jo = kvp.Value;
			string kind = equip ["kind"].ToString ();
			if (jo[kind].ToString() == equip ["id"].ToString ()) {
				return jo;
			}
		}
		return null;
	}
	/// <summary>
	/// 加载一个spine的骨骼动画
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	/**public SkeletonDataAsset loadSpineAnim(string path)
	{
		//加载json,atlas文件;
		TextAsset jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(@"Assets\" + path + ".json");
		TextAsset atlasFile = AssetDatabase.LoadAssetAtPath<TextAsset>(@"Assets\" + path + ".atlas.txt");

		//解析图片个数;就是材质个数;
		string atlasStr = atlasFile.ToString();
		string[] atlasLines = atlasStr.Split('\n');
		List<string> _lsPng = new List<string>();
		for (int i = 0; i < atlasLines.Length - 1; i++)
		{
			if (atlasLines[i].Length == 0)
				_lsPng.Add(atlasLines[i + 1]);
		}
		//创建材质;
		Material[] maters = null;
		if (_lsPng != null)
		{
			maters = new Material[_lsPng.Count];
			string pngPath = path.Replace("/", @"\");
			int pos = pngPath.LastIndexOf(@"\");
			pos++;
			pngPath = pngPath.Remove(pos, pngPath.Length - pos);
			for (int i = 0; i < _lsPng.Count; i++)
			{
				maters[i] = new Material(Shader.Find("Spine/Skeleton"));
				string[] pngs = _lsPng[i].Split('.');
				string pngP = pngPath + pngs[0];
				//maters[i].mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(@"Assets\" + path + ".png");
				maters[i].mainTexture = (Texture)Resources.Load(@"Assets\" + path);
					//Resources.Load(pngPath + _lsPng[i],typeof(Texture)) as Texture;

			}
		}
		//创建一个atlas;
		AtlasAsset atlasAsset = ScriptableObject.CreateInstance<AtlasAsset>();
		atlasAsset.atlasFile = atlasFile;
		atlasAsset.materials = maters;
		//atlasAsset.Reset();
		//创建一个spine插件的动画文件,并初始化数据;
		SkeletonDataAsset skeletonDataAsset = SkeletonDataAsset.CreateInstance<SkeletonDataAsset>();
		skeletonDataAsset.atlasAssets = new AtlasAsset[1] { atlasAsset };
		skeletonDataAsset.skeletonJSON = jsonFile;
		skeletonDataAsset.fromAnimation = new string[0];
		skeletonDataAsset.toAnimation = new string[0];
		skeletonDataAsset.duration = new float[0];
		skeletonDataAsset.scale = 0.01f;            //创建出来动画的渲染大小;


		return skeletonDataAsset;
	}
	**/
	/// <summary>
	/// 加载一个spine的骨骼动画
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public SkeletonAnimation getSkeletonDataAsset(string path)
	{
		 //SkeletonAnimation playerAnim;
	        SkeletonAnimation playerAnim;
		   SkeletonDataAsset playerData;
		   AtlasAsset atlasdata;
		   string name = path;
		   atlasdata = ScriptableObject.CreateInstance<AtlasAsset> ();
		   playerData = ScriptableObject.CreateInstance<SkeletonDataAsset> ();
		   playerData.fromAnimation = new string[0];
		   playerData.toAnimation = new string[0];
		   playerData.duration = new float[0];
		   playerData.scale = 0.01f;
		   playerData.defaultMix = 0.15f;


		   atlasdata.atlasFile = (TextAsset)Resources.Load (name + ".atlas.txt", typeof(TextAsset));
		   Material[] materials = new Material[1];
		   materials [0] = new Material (Shader.Find ("Spine/Skeleton"));
		   Texture aa = (Texture)Resources.Load (name, typeof(Texture2D));
		   materials [0].mainTexture = aa;
		   
		   atlasdata.materials = materials;
		   
		   playerData.atlasAssets = new AtlasAsset[1] { atlasdata };
		   playerData.skeletonJSON = (TextAsset)Resources.Load (name, typeof(TextAsset));
		   
		   //GameObject player = new GameObject();
		   //player.transform.localPosition = Vector3.zero;
		   //player.transform.localScale = new Vector3 (1f, 1f, 1f);
		   
			playerAnim = SkeletonAnimation.NewSkeletonAnimationGameObject (playerData);
		   playerAnim.skeletonDataAsset = playerData;
		   playerAnim.AnimationName = "attack";
		   playerAnim.calculateNormals = true;

		   playerAnim.loop = true;
	     

		return playerAnim;
	}
}
