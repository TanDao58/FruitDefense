using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using System;
using TMPro;
using Newtonsoft.Json;
using Firebase;
using Firebase.Database;
using System.Text.RegularExpressions;

public enum PlacementRewarded
{
    reward_skip_level, reward_extra_coin, reward_hint, reward_skin
}

public enum PlacementIntertistial
{
    inter_click_button_setting, inter_next_level, inter_click_button_back, inter_select_level_in_home
}

public enum PlacementBanner
{
    banner_bottom
}

public enum RemoteConfigKey
{
    min_level_to_show_ads, min_seconds_between_intertistial, min_seconds_analytics
}

public enum UserProperty
{
    level, level_max, last_level, last_placement, total_interstitial_ads, total_rewarded_ads, retention_type, total_play_day
}

[System.Serializable]
public class FirebaseUserData
{
    public string id;
    public double startPlayDay;
    public double lastPlayDay;
    public string startPlayCode;
    public int totalPlayDay;
    public float totalPlayTime;
    public int retentionType;
    public int lastInterShow;
    public int level;
    public int interAdsCount;
    public int rewardAdsCount;
    public string language;
    public bool internetDisable;
    public string appVersion;
    public int sessionCount;
    public int exceptionCount;
    public string installerName;
}

[System.Serializable]
public class FirebaseSessionData
{
    public string id;
    public float sessionTime;
    public int totalPlayDay;
    public float totalPlayTime;
    public int retentionType;
    public string language;
    public List<string> actions;
}

[System.Serializable]
public class FirebaseExceptionData
{
    public int sessionId;
    public string userId;
    public string detail;
    public string version;
    public string time;
    public string platform;
    public string deviceModel;
    public string deviceName;
    public int systemMemorySize;
    public float batteryLevel;
    public BatteryStatus batteryStatus;
    public int graphicsMemorySize;
}

public class FirebaseManager : MonoBehaviour
{
    public const string FIREBASE_USER_DATA_FILE_NAME = "firebase_userdata";
    public static FirebaseManager Instance;

    public static FirebaseUserData firebaseUserData;
    public static int minsSecondsBetweenIntertistial;
    public static int minLevelToShowAds;
    public static int minSecondAnalytics;

    List<string> actions;
    DatabaseReference databaseRef;
    bool isFirebaseReady;
    float lastAnalytics;
    float previousPlayTime; //tổng thời gian chơi ở các lần chơi trước đó
    string appVersion;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        actions = new List<string>();
        appVersion = Application.version.Replace(".", "_");
        Application.logMessageReceived += LogException;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                isFirebaseReady = true;
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;

                LoadUserData();
                firebaseUserData.appVersion = Application.version;
                firebaseUserData.language = Application.systemLanguage.ToString();
                firebaseUserData.sessionCount++;
                firebaseUserData.installerName = Application.installerName;

                previousPlayTime = firebaseUserData.totalPlayTime;

                TrackRetention();
                AnalyticsUserProperty();
                SetUpRemoteConfig();
                SaveLocal();
                SaveServer();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
    #region FIREBASE_EVENTS
    public void LogLevelStart(int level, bool isRestart)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("level", level);
        var param2 = new Parameter("restart", isRestart ? "true" : "false");
        FirebaseAnalytics.LogEvent("level_start", param1, param2);
        if (isRestart) actions.Add("restart level " + level);
        else actions.Add("play level " + level);
    }

    public void LogLevelPassed(int level, float timeSpent)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("level", level);
        var param2 = new Parameter("time_spent", timeSpent);
        FirebaseAnalytics.LogEvent("level_passed", param1, param2);
        actions.Add("pass level " + level + " time = " + (int)(timeSpent));
    }

    public void LogLevelFail(int level, float timeSpent)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("level", level);
        var param2 = new Parameter("time_spent", timeSpent);
        FirebaseAnalytics.LogEvent("level_failed", param1, param2);
        actions.Add("fail level " + level + " time = " + (int)(timeSpent));
    }

    public void LogShowIntertistial(PlacementIntertistial placement)
    {
        if (isFirebaseReady == false) return;
        bool no_internet = Application.internetReachability == NetworkReachability.NotReachable;
        var param1 = new Parameter("internet_available", no_internet ? "false" : "true");
        var param2 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("show_interstitial_ads", param1, param2);

        SetUserProperty(UserProperty.last_placement, placement.ToString());
        actions.Add("show inter");
    }

    public void LogFailedShowIntertistial(PlacementIntertistial placement)
    {
        if (isFirebaseReady == false) return;
        bool no_internet = Application.internetReachability == NetworkReachability.NotReachable;
        var param1 = new Parameter("internet_available", no_internet ? "false" : "true");
        var param2 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("failed_show_interstitial_ads", param1, param2);
        actions.Add($"failed inter (internet = {!no_internet})");
    }

    public void LogIntertistialSuccess(PlacementIntertistial placement)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("interstitial_ads_success", param1);

        GameSystem.userdata.totalIntertistialAds++;
        GameSystem.SaveUserDataToLocal();
        SetUserProperty(UserProperty.total_interstitial_ads, GameSystem.userdata.totalIntertistialAds.ToString());
        firebaseUserData.interAdsCount++;
        actions.Add($"success inter");
    }

    public void LogIntertistialRequest()
    {
        FirebaseAnalytics.LogEvent("intertistial_request");
    }

    public void LogIntertistialReady()
    {
        FirebaseAnalytics.LogEvent("intertistial_ready");
    }

    public void LogRewardedRequest()
    {
        FirebaseAnalytics.LogEvent("rewarded_request");
    }

    public void LogRewardedReady()
    {
        FirebaseAnalytics.LogEvent("rewarded_ready");
    }

    public void LogIntertistialClick(PlacementIntertistial placement)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("interstitial_ads_click", param1);
        actions.Add($"click inter");
    }

    public void LogShowRewarded(PlacementRewarded placement)
    {
        if (isFirebaseReady == false) return;
        bool no_internet = Application.internetReachability == NetworkReachability.NotReachable;
        var param1 = new Parameter("internet_available", no_internet ? "false" : "true");
        var param2 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("show_rewarded_ads", param1, param2);
        actions.Add($"show reward ({placement.ToString()})");
        SetUserProperty(UserProperty.last_placement, placement.ToString());
    }

    public void LogFailedShowReward(PlacementRewarded placement)
    {
        if (isFirebaseReady == false) return;
        bool no_internet = Application.internetReachability == NetworkReachability.NotReachable;
        var param1 = new Parameter("internet_available", no_internet ? "false" : "true");
        var param2 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("failed_show_reward_ads", param1, param2);
        actions.Add($"failed inter (internet = {!no_internet})");
    }

    public void LogRewardedSuccess(PlacementRewarded placement)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("rewarded_ads_success", param1);

        GameSystem.userdata.totalRewardeddAds++;
        GameSystem.SaveUserDataToLocal();
        SetUserProperty(UserProperty.total_rewarded_ads, GameSystem.userdata.totalRewardeddAds.ToString());
        actions.Add("success reward");
    }

    public void LogRewardedClick(PlacementRewarded placement)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("rewarded_ads_click", param1);
        actions.Add($"click reward");
    }

    public void LogShowBanner(PlacementBanner placement)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("show_banner_ads", param1);
    }

    public void LogBannerClick(PlacementBanner placement)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("placement", placement.ToString());
        FirebaseAnalytics.LogEvent("banner_ads_click", param1);
        actions.Add($"click banner");
    }

    public void LogShowAppOpenAds()
    {
        if (isFirebaseReady == false) return;
        FirebaseAnalytics.LogEvent("show_app_open_ads");
        actions.Add($"show app open");
    }

    public void LogPopupAppear(string popupName)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("screen_name", SceneManager.GetActiveScene().name);
        var param2 = new Parameter("name", popupName);
        FirebaseAnalytics.LogEvent("ui_appear", param1, param2);
        actions.Add("popup " + popupName);
    }

    public void LogButtonClick(string buttonName)
    {
        if (isFirebaseReady == false) return;
        var param1 = new Parameter("screen_name", SceneManager.GetActiveScene().name);
        var param2 = new Parameter("name", buttonName);
        FirebaseAnalytics.LogEvent("button_click", param1, param2);
        actions.Add("button " + buttonName);
    }

    public void LogAction(string action)
    {
        actions.Add(action);
    }

    public void LogException(string condition, string stackTrace, LogType type)
    {
        if (isFirebaseReady == false) return;
        if (type == LogType.Exception)
        {
            actions.Add("exception " + condition + " " + stackTrace);
            FirebaseExceptionData exception = new FirebaseExceptionData();
            exception.sessionId = firebaseUserData.sessionCount;
            exception.version = Application.version;
            exception.userId = firebaseUserData.id;
            exception.detail = condition + " " + stackTrace;
            exception.time = DateTime.Now.ToString();
            exception.platform = Application.platform.ToString();
            exception.deviceModel = SystemInfo.deviceModel;
            exception.deviceName = SystemInfo.deviceName;
            exception.systemMemorySize = SystemInfo.systemMemorySize;
            exception.batteryLevel = SystemInfo.batteryLevel;
            exception.batteryStatus = SystemInfo.batteryStatus;
            exception.graphicsMemorySize = SystemInfo.graphicsMemorySize;

            //save server
            string json = JsonConvert.SerializeObject(exception);
            string key = Regex.Replace(condition, "[^0-9A-Za-z _-]", "");
            databaseRef.Child(appVersion).Child("exception").Child(key).Child(GetRandomString(4)).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Run finish, result = " + task.Status);
                Debug.Log("Exception = " + task.Exception);
            });
        }
    }
    #endregion

    public void SetUserProperty(UserProperty userProperty, string value)
    {
        if (isFirebaseReady == false) return;
        FirebaseAnalytics.SetUserProperty(userProperty.ToString(), value);
    }

    public void AnalyticsUserProperty()
    {
        SetUserProperty(UserProperty.level_max, (GameSystem.userdata.maxLevel + 1).ToString());
        SetUserProperty(UserProperty.last_level, (GameSystem.userdata.currentLevel + 1).ToString());
        SetUserProperty(UserProperty.level, (GameSystem.userdata.currentLevel + 1).ToString());
        SetUserProperty(UserProperty.retention_type, (firebaseUserData.retentionType).ToString());
        SetUserProperty(UserProperty.total_play_day, (firebaseUserData.totalPlayDay).ToString());
    }

    #region REMOTE_CONFIG
    public void SetUpRemoteConfig()
    {
        if (Constants.TEST_MODE)
        {
            ConfigSettings setting = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
            setting.MinimumFetchInternalInMilliseconds = 0;
        }

        Dictionary<string, object> defaults = new Dictionary<string, object>();

        defaults.Add(RemoteConfigKey.min_level_to_show_ads.ToString(), Constants.MIN_LEVEL_TO_SHOW_ADS);
        defaults.Add(RemoteConfigKey.min_seconds_between_intertistial.ToString(), Constants.MIN_SECONDS_BETWEEN_INTERTISTIAL);
        defaults.Add(RemoteConfigKey.min_seconds_analytics.ToString(), Constants.MIN_SECONDS_ANALYTICS_RATE);
        minsSecondsBetweenIntertistial = Constants.MIN_SECONDS_BETWEEN_INTERTISTIAL;
        minLevelToShowAds = Constants.MIN_LEVEL_TO_SHOW_ADS;
        minSecondAnalytics = Constants.MIN_SECONDS_ANALYTICS_RATE;

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FetchDataAsync();
            }
            else
            {
                Debug.LogError("Failed to set default value of remote config");
            }
        });
    }

    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task => {
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            minLevelToShowAds = (int)remoteConfig.GetValue(RemoteConfigKey.min_level_to_show_ads.ToString()).LongValue;
            minsSecondsBetweenIntertistial = (int)remoteConfig.GetValue(RemoteConfigKey.min_seconds_between_intertistial.ToString()).LongValue;
            minSecondAnalytics = (int)remoteConfig.GetValue(RemoteConfigKey.min_seconds_analytics.ToString()).LongValue;
        });
    }
    #endregion

    public void TrackRetention()
    {
        double today = new TimeSpan(DateTime.Now.Ticks).TotalDays;
        if (firebaseUserData.startPlayDay == 0)
        {
            firebaseUserData.startPlayDay = today;
        }
        if (firebaseUserData.lastPlayDay - today > 1)
        {
            firebaseUserData.lastPlayDay = today;
            firebaseUserData.totalPlayDay++;
        }
        firebaseUserData.retentionType = (int)(today - firebaseUserData.startPlayDay);
    }

    public void SaveLocal()
    {
        //save local
        string json = JsonConvert.SerializeObject(firebaseUserData);
        string path = FileUtilities.GetWritablePath(FIREBASE_USER_DATA_FILE_NAME);
        FileUtilities.SaveFile(System.Text.Encoding.UTF8.GetBytes(json), path, true);
    }

    public void SaveServer()
    {
        //save server
        string json = JsonConvert.SerializeObject(firebaseUserData);
        databaseRef.Child(appVersion).Child("userdata").Child(firebaseUserData.id).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Run finish, result = " + task.Status);
            Debug.Log("Exception = " + task.Exception);
        });
    }

    public void SaveActions()
    {
        FirebaseSessionData session = new FirebaseSessionData();
        session.id = firebaseUserData.id;
        session.language = firebaseUserData.language;
        session.retentionType = firebaseUserData.retentionType;
        session.totalPlayDay = firebaseUserData.totalPlayDay;
        session.totalPlayTime = firebaseUserData.totalPlayTime;
        session.sessionTime = Time.time;
        session.actions = actions;

        //save server
        string json = JsonConvert.SerializeObject(session);
        databaseRef.Child(appVersion).Child("session").Child(firebaseUserData.id).Child(firebaseUserData.sessionCount.ToString()).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Run finish, result = " + task.Status);
            Debug.Log("Exception = " + task.Exception);
        });
    }

    public void LoadUserData()
    {
        if (!FileUtilities.IsFileExist(FIREBASE_USER_DATA_FILE_NAME))
        {
            firebaseUserData = new FirebaseUserData();
            firebaseUserData.id = GetRandomUserKey();
            firebaseUserData.startPlayDay = new TimeSpan(DateTime.UtcNow.Ticks).TotalDays;
        }
        else
        {
            firebaseUserData = FileUtilities.DeserializeObjectFromFile<FirebaseUserData>(FIREBASE_USER_DATA_FILE_NAME);
        }
    }

    const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

    public string GetRandomString(int length)
    {
        string rand = "";
        for (int i = 0; i < length; i++)
        {
            rand += alphabet[UnityEngine.Random.Range(0, alphabet.Length)];
        }
        return rand;
    }

    public string GetRandomUserKey()
    {
        string appVersion = Application.version.Replace(".", "_");
        string datetime = DateTime.Now.ToString("yyMMdd");
        string installerName = Application.installerName.Replace(".", "_");
        string key = datetime + "_" + appVersion + "_" + Application.systemLanguage + "_" + GetRandomString(4);
        if (installerName == "com.android.vending")
        {
            key += "_user";
        }
        return key;
    }

    private void Update()
    {
        if (firebaseUserData == null) return;
        if (Time.time - lastAnalytics > minSecondAnalytics)
        {
            lastAnalytics = Time.time;
            firebaseUserData.totalPlayTime = previousPlayTime + Time.time;
            SaveLocal();
            SaveServer();
            SaveActions();
        }
    }
}