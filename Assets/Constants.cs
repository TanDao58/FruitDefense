using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public const int TEMP_PLAYER_LEVEL = 4;
    public const float DIFFICULT_RATIO = 2f;
    public const float BASE_DIFFICULT = 0.2f;

    public static bool TEST_MODE = false;
    public static bool CHEAT_NO_DIE = false;

    public const string SCENE_INTRO = "IntroScene";
    public const string SCENE_LOADING = "Loading";
    public const string SCENE_GAMEPLAY = "Gameplay";
    public const string SCENE_GAMEPLAY_NEW = "Gameplay2";
    public const string SCENE_HOME = "HeroSelect";

    public const float APP_OPEN_LOADING_TIME_OUT = 7f;
    public const float LOADING_TIME = 1f;

    public const float DP_INCREASE_PER_SEC = 2f;
    public const float DEFAULT_MONEY_GAIN = 1f;
    public const int DP_START = 150;
    public const int DP_MAX_INGAME = 250;

    public const float MIN_SPAWN = 1f;
    public const float MAX_SPAWN = 2f;

    public const float HEAL_PERCENT = 0.015f;
    public const float ENEMY_BONUS_ATTACK_RANGE = 0f;

    public const int MAX_HERO = 5;
    public const int MAX_HERO_INGAME = 10;

    public const int BACKGROUND_COUNT = 4;
    public const int WATCH_ADS_GEM_RECEIVE_AMOUNT = 5;

    public const int ENERGY_COST_EACH_PLAY = 1;
    public const int MAX_ENERGY = 100;
    public const int MINUTE_INCREASE_1_ENERGY = 5;

    public const float DEFAULT_BOSS_SPEED = 0.25f;
    public const float BLOCK_SHIELD_PERCENT = 0.6f;

    public const float FREEZE_DAME_PERCENT = 0.05f;
    public const float REFLECT_DAMAGE_PERCENT = 0.1f;

    public const string SETTING_VIBRATION = "vibration";
    public const string SETTING_MUSIC = "music";
    public const string SETTING_SOUND = "sound";

    public const int DEFAULT_VIBRATION = 1;
    public const int DEFAULT_SOUND = 1;
    public const int DEFAULT_MUSIC = 1;

    public const float REFLECT_DAMAGE = 10f;
    public const float DEFAULT_TIME_SCALE = 1.5f;
    public const float SPEED_UP_TIME_SCALE = 2f;

    public const float BASE_UPGRADE_MONEY_C = 100;
    public const float BASE_UPGRADE_MONEY_R = 5000;
    public const float BASE_UPGRADE_MONEY_SR = 12500;
    public const float BASE_UPGRADE_MONEY_SSR = 1225000;

    public const int MIN_LEVEL_TO_SHOW_ADS = 5;
    public const int MIN_SECONDS_BETWEEN_INTERTISTIAL = 20;
    public const int MIN_SECONDS_ANALYTICS_RATE = 20;

    public const float FIREWORK_TIME_BEFORE_ADS = 2.5f;
    public const int DEFAULT_LANGUAGE = 0;
    public const float APP_OPEN_MIN_BACKGROUND_TIME = 10f;
}