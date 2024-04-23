using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    public static UserData userdata;
    public static List<string> chosenMonsters = new List<string>();

    #region Constant
    public static int START_GOLD = 220;

    public static int VERTICAL_NUMBER = 5;
    public static int HORIZONTAL_NUMBER = 14;
    public static float PADDING = 2.5f;

    public static string USER_DATA_FILE_NAME = "on7PRwy5XiwWj3z6";

    public float GOLD_SPAWN_RATE = 1;
    public float SCREEN_WIDTH = 9f;
    public float SCREEN_HEIGHT = 5f;

    public const string PLAYSTORE_URL = "https://play.google.com/store/apps/details?id=com.rubycell.piano.tiles.neon.sonic.rhythm.cat.dancing.magic.beat.edm.hop";

    public const string FIREBASE_EVENT_REQUEST_INTERTISTIAL = "intertistial_request";
    public const string FIREBASE_EVENT_SHOW_INTERTISTIAL = "intertistial_show";
    public const string FIREBASE_EVENT_REQUEST_REWARD = "reward_request";
    public const string FIREBASE_EVENT_SHOW_REWARD = "reward_show";

    public const string FIREBASE_EVENT_PLAY_GAME = "play_game";
    public const string FIREBASE_EVENT_PLAY_TUTORIAL = "play_tutorial";
    public const string FIREBASE_EVENT_PLAY_COMPLETE = "play_completed";
    public const string FIREBASE_EVENT_GAME_OVER = "game_over";
    public const string FIREBASE_EVENT_WATCH_ADS_CONTINUE = "watch_ads_continue";
    public const string FIREBASE_EVENT_CONTINUE_GAME = "continue_game";
    public const string FIREBASE_EVENT_SHOW_GAME_RESULT = "show_game_result";

    public const string FIREBASE_EVENT_SHOW_SONG_SELECTOR = "show_song_selector";
    public const string FIREBASE_EVENT_GET_DAILY_REWARD = "get_daily_reward";
    public const string FIREBASE_EVENT_GET_LUCKY_SPIN_REWARD = "get_lucky_spin_reward";
    public const string FIREBASE_EVENT_PLAY_SONG_THUMBNAIL = "play_song_thumbnail";
    public const string FIREBASE_EVENT_UNLOCK_SONG = "unlock_song";
    public const string FIREBASE_EVENT_PLAY_NEXT_GAME_RANDOM = "play_next_game_random";
    public const string FIREBASE_EVENT_REPLAY_GAME = "replay_game";

    public const string PLAYER_PREF_NEXT_DAILY_REWARD_TIME = "next_daily_reward_time";
    public const string PLAYER_PREF_NEXT_DAILY_REWARD_DAY_INDEX = "next_daily_reward_day_index";
    public const string PLAYER_PREF_LAST_PLAY_TIME = "last_play_time";

    public const string PLAYER_PREF_LAST_FREE_AUTOMERGE = "last_free_automerge";
    public const string PLAYER_PREF_LAST_FREE_BUFFET_PARTY = "last_free_buffet";
    #endregion

    public List<GameObject> dragonSmalls;
    public List<GameObject> dragonMediums;
    public List<GameObject> dragonBigs;

    public List<Material> bodyMats;
    public List<Material> eyeMats;

    public static List<GameObject> poolObjects;
    public static List<string> poolNames;

    public static int gold;
    public static int levelId;

    public static string currentScene;

    public TextMeshProUGUI textGold;

    float count;

    void Awake() {
        poolObjects = new List<GameObject>();
        poolNames = new List<string>();

        gold = 0;
        LoadUserData();
        //Debug.Log(userdata.levelReward.Count);
        if(Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool SpendGold(int amount) {
        if (amount > gold)
            return false;
        gold -= amount;
        Instance.textGold.text = gold.ToString();
        return true;
    }

    public static GameObject LoadPool(string poolName, Vector3 position) {
        for (int i = 0; i < poolNames.Count; i++) {
            if (string.Compare(poolNames[i], poolName) == 0 && poolObjects[i].activeSelf == false) {
                poolObjects[i].SetActive(true);
                poolObjects[i].transform.position = position;
                return poolObjects[i];
            }
        }
        GameObject src = Resources.Load<GameObject>(poolName) as GameObject;
        
        GameObject obj = Instantiate(src, position, src.transform.rotation);
        poolNames.Add(poolName);
        poolObjects.Add(obj);
        return obj;
    }

    public void SpawnText(string text, Vector3 pos) {
        GameObject obj = LoadPool("BasicText", pos + new Vector3(0,1f));
        obj.GetComponent<TextMeshPro>().text = text;
        DoEffectAppear(obj, 0f, obj.transform.localScale.x);

        LeanTween.delayedCall(1f, () => {
            obj.SetActive(false);
        });
    }

    public void ShowHint(string text) {
        GameObject obj = LoadPool("BasicText", new Vector3(0.44f, -2.15f));
        obj.GetComponent<TextMeshPro>().text = text;
        LeanTween.move(obj, obj.transform.position + new Vector3(0f, 1f),1f);
        LeanTween.delayedCall(5f, () => {
            obj.SetActive(false);
        });
    }

    public static Vector3 ConvertToGridPosition(Vector3 pos) {
        Vector3 pivot = Instance.transform.position - new Vector3(PADDING / 2, PADDING / 2);

        int h_index = (int)((pos.x - pivot.x) / PADDING);
        int v_index = (int)((pos.y - pivot.y) / PADDING);

        if (h_index >= 0 && h_index < HORIZONTAL_NUMBER && v_index >= 0 && v_index < VERTICAL_NUMBER) {
            Vector3 gridPosition = new Vector3(Instance.transform.position.x + h_index * PADDING, Instance.transform.position.y + v_index * PADDING);
            return gridPosition;
        }

        return new Vector3(-9999f, -9999f);
    }

    public static Vector3 GoToTargetVector(Vector3 current, Vector3 target, float speed, bool isFlying = false) {
        float distanceToTarget = Vector3.Distance(current, target);
        if (distanceToTarget < 0.1f)
            return new Vector3(0,0);

        Vector3 vectorToTarget = target - current;

        vectorToTarget = vectorToTarget * speed / distanceToTarget;

        return vectorToTarget;
    }

    public void DoEffectAppear(GameObject obj, float starScale, float endScale, float speed = 0.2f, float maxScale = 1.1f) {
        obj.transform.localScale = new Vector3(starScale, starScale);
        LeanTween.scale(obj, new Vector3(maxScale, maxScale, maxScale) * endScale, speed).setOnComplete(() => {
            LeanTween.scale(obj, new Vector3(1f, 1f, 1f) * endScale, speed);
        });
    }

    public void DoEffectDisappear(GameObject obj, float starScale, float endScale, float speed = 0.2f, float maxScale = 1.1f) {
        obj.transform.localScale = new Vector3(starScale, starScale);
        LeanTween.scale(obj, new Vector3(maxScale, maxScale, maxScale) * starScale, speed).setOnComplete(() => {
            LeanTween.scale(obj, new Vector3(1f, 1f, 1f) * endScale, speed).setOnComplete(() => {
                obj.SetActive(false);
            });
        });
    }

    public static void ShowImageBanner(Image img) {
        img.gameObject.SetActive(true);
        img.color = new Color(1, 1, 1, 1);

        GameSystem.Instance.DoEffectAppear(img.gameObject, 0.5f, 1f, 0.5f);

        LeanTween.delayedCall(2f, () => {
            LeanTween.value(1f, 0f, 1f).setOnUpdate((float f) => {
                img.color = new Color(1, 1, 1, f);
            }).setOnComplete(() => {
                img.gameObject.SetActive(false);
            });
        });
    }

    public IEnumerator IncreaseNumberEffect(TextMeshProUGUI txtNumber, int startGold, int endGold, float effectTime) {
        int increase = (int)((endGold - startGold) / (effectTime / Time.deltaTime));
        if (increase == 0) {
            increase = endGold > startGold ? 1 : -1;
        }
        int gold = startGold;
        bool loop = true;
        while (loop) {
            gold += increase;

            if (startGold < endGold) {
                loop = gold < endGold;
            } else {
                loop = gold > endGold;
            }

            //txtNumber.text = gold.ToString();
            txtNumber.text = GameSystem.ShortenNumer(gold);

            yield return new WaitForEndOfFrame();
        }
    }

    public static void SaveUserDataToLocal() {
        string json = JsonConvert.SerializeObject(GameSystem.userdata);
        string path = FileUtilities.GetWritablePath(GameSystem.USER_DATA_FILE_NAME);

        FileUtilities.SaveFile(System.Text.Encoding.UTF8.GetBytes(json), path, true);
    }

    public static void LoadUserData() {
        if (!FileUtilities.IsFileExist(GameSystem.USER_DATA_FILE_NAME)) {
            GameSystem.userdata = new UserData();
            if (!userdata.unlockedHeros.Contains("E0"))
            {
                userdata.unlockedHeros.Add("E0");
                var monsterData = Resources.Load<MonsterAI>("E0").monsterData;
                monsterData.damage = monsterData.baseDamge;
                monsterData.maxhp = monsterData.baseHp;
                monsterData.level = 1;
                monsterData.rarity = monsterData.baseRarity;
                userdata.unlockHeroDatas.Add("E0", monsterData);
            }            
            //if (userdata.unlockedHeros == null || userdata.unlockedHeros.Count == 0) {
            //    userdata.unlockedHeros = new List<string>();
            //    userdata.unlockedHeroesLevel.Add("E2", 1);
            //    userdata.unlockedHeroesLevel.Add("E3", 1);
            //    userdata.unlockedHeros.Add("E2");
            //    userdata.unlockedHeros.Add("E3");
            //}
            GameSystem.SaveUserDataToLocal();
        } else {
            //GameSystem.userdata = FileUtilities.DeserializeObjectFromFile<UserData>(GameSystem.USER_DATA_FILE_NAME);
            GameSystem.userdata = FileUtilities.DeserializeObjectFromFile<UserData>(GameSystem.USER_DATA_FILE_NAME);
        }
    }

    public static T LoadFile<T>(string fileName)
    {
        if (!FileUtilities.IsFileExist(fileName))
        {
            return default(T);
        }
        else
        {
            return FileUtilities.DeserializeObjectFromFile<T>(fileName);
        }
    }

    public static void SaveFile(string fileName, object data)
    {
        string json = JsonConvert.SerializeObject(data);
        string path = FileUtilities.GetWritablePath(fileName);
        FileUtilities.SaveFile(System.Text.Encoding.UTF8.GetBytes(json), path, true);
    }

    public void DoEffectCakeAppear(Vector3 pos) {
        GameObject effect = GameSystem.LoadPool("StarEffect", pos);
        LeanTween.delayedCall(0.5f, () => {
            effect.SetActive(false);
        });
    }

    public void DoEffectBounce(GameObject go, float time) {
        LeanTween.scale(go, new Vector3(1.2f, 0.8f), time).setOnComplete(() => {
            LeanTween.scale(go, new Vector3(0.8f, 1.2f), time).setOnComplete(() => {
                LeanTween.scale(go, new Vector3(1f, 1f), time);
            });
        });
    }

    //public void ShowTextEffect(float dame, Vector3 pos) {
    //    GameObject text = GameSystem.LoadPool("TextEffect", pos);
    //    text.GetComponent<TextEffect>().DoEffect(dame);
    //}

    //public void ShowTextGold(float dame, Vector3 pos) {
    //    GameObject text = GameSystem.LoadPool("TextGold", pos);
    //    text.GetComponent<TextGold>().DoEffect(dame);
    //    Gameplay.Instance.AddGold(dame);
    //}

    public static int GetSortingOrder(Transform t) { 
        return -(int)(t.position.y * 10000);
    }

    public static int GetFallNapVungLevel() {
        //int maxLevel = -9999;

        //for (int i = 0; i < Gameplay.Instance.cells.Count; i++) {
        //    if (Gameplay.Instance.cells[i].level > maxLevel) {
        //        maxLevel = i;
        //    }
        //}

        int maxLevel = GameSystem.userdata.currentLevel;

        if (maxLevel < 7) {
            return 1;
        }
        else if(maxLevel < 10) {
            return 2;
        }
        else if(maxLevel < 13) {
            return 3;
        }
        else if(maxLevel < 15) {
            return 4;
        } else {
            return maxLevel - 10;
        }
    }

    public static int GetBuyCakeLevel() {
        //int maxLevel = -9999;

        //for (int i = 0; i < Gameplay.Instance.cells.Count; i++) {
        //    if (Gameplay.Instance.cells[i].level > maxLevel) {
        //        maxLevel = i;
        //    }
        //}

        int maxLevel = GameSystem.userdata.currentLevel;

        if (maxLevel < 7) {
            return 1;
        } else if (maxLevel < 9) {
            return 2;
        } else if (maxLevel < 11) {
            return 3;
        } else if (maxLevel < 14) {
            return 4;
        } else {
            return maxLevel - 10;
        }
    }

    public static string ShortenNumer(float moneyIn, int digitNumber = 2) {
        //quadrillion, quintrillion, sextillion, septillion
        const float THOUNDSAND = 1000;
        const float MILION = 1000000;
        const float BILLION = 1000000000;
        const float TRILLION = 1000000000000;
        const float TRILLION_1 = TRILLION * 1000;
        const float TRILLION_2 = TRILLION_1 * 1000;

        string FORMAT = "F" + digitNumber;

        if (moneyIn < THOUNDSAND) { 
            return ((int)moneyIn).ToString();
        }

        if (moneyIn < MILION) {
            return (moneyIn / THOUNDSAND).ToString(FORMAT) + "K";
        }

        if (moneyIn < BILLION) {
            return (moneyIn / MILION).ToString(FORMAT) + "M";
        }

        if (moneyIn < TRILLION) {
            return (moneyIn / BILLION).ToString(FORMAT) + "B";
        }

        if (moneyIn < TRILLION_1) {
            return (moneyIn / TRILLION).ToString(FORMAT) + "T";
        }

        if (moneyIn < TRILLION_2) {
            return (moneyIn / TRILLION_1).ToString(FORMAT) + "AA";
        }

        return "Infinity!";
    }

    //public static float GetGoldPrice(int level) {
    //    float dame = CakeData.Instance.cakeInfos[level].dame;

    //    if (!GameSystem.userdata.buyCakeCounts.ContainsKey(level)) {
    //        GameSystem.userdata.buyCakeCounts.Add(level, 0);
    //    }

    //    GameSystem.userdata.buyCakeCounts[level]++;

    //    float price = dame * 2 * Mathf.Pow(1.1f, GameSystem.userdata.buyCakeCounts[level]);

    //    return price;
    //}

    public static float GetDiamondPrice(int level) {

        float price = Mathf.Pow(1.2f, level);

        return price;
    }

    public static int GetDayCode(DateTime date) {
        string year = date.Year.ToString();

        string month = date.Month.ToString();
        if (date.Month < 10)
            month = "0" + month;

        string day = date.Day.ToString();
        if (date.Day < 10)
            day = "0" + day;

        return int.Parse(year + month + day);
    }
}