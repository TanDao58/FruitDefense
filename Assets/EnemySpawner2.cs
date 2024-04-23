//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class EnemySpawner2 : EnemySpawner
//{
//    public TextMeshProUGUI txtReady;

//    public List<ParticleSystem> effects;

//    public override void Start() {
//        waveNumber = Gameplay.Intansce.canvas.GetChildComponent<TextMeshProUGUI>("WaveNumer");
//        txtReady.transform.position = new Vector2(-Screen.width * 1.5f, txtReady.transform.position.y);

//        currentWave = -1;

//        for (int i = 0; i < effects.Count; i++) {
//            effects[i].Stop();
//        }

//        StartCoroutine(IEShowMessage("PREPARE!"));
//    }

//    public void ReadyStartWave() {
//        currentWave++;
//        StartCoroutine(IESpawnCurrentWave());
//    }

//    public IEnumerator IESpawnCurrentWave() {
//        yield return IEShowMessage("READY!");

//        int playerLevel = testMode ? Constants.TEMP_PLAYER_LEVEL : GameSystem.userdata.currentLevel;
//        //var difficultRatio = CalculateDifficultRatio(playerLevel);

//        LevelData levelData = DataManager.Instance.levelDatas[playerLevel];

//        waveNumber.text = "Wave " + (currentWave + 1).ToString();
//        WaveInfo waveInfo = levelData.waveInfos[currentWave];

//        if (currentWave == levelData.waveInfos.Count - 1) {
//            if (levelData.boss != EnemyType.None) {
//                Vector3 pos = new Vector3(Random.Range(-4f, 4f), Random.Range(8f, 12f));
//                var boss = ObjectPool.Instance.GetGameObjectFromPool<MonsterAI>(levelData.boss.ToString(), pos);
//                boss.battleStat = new BattleStat(levelData.bossData);
//                boss.IsEnemy = true;
//                boss.isBoss = true;
//                EasyEffect.Appear(boss.gameObject, 0f, levelData.bossSizeFactor);
//                currentEnemiesInWave.Add(boss);
//            }
//        }

//        //yield return IESpawnWave(waveInfo);
//        yield return new WaitUntil(CheckAllEnemyDie);

//        if (currentWave == levelData.waveInfos.Count - 1) {
//            Gameplay.Intansce.ShowWinPopup();
//            GameSystem.SaveUserDataToLocal();
//        } else {
//            for (int i = 0; i < effects.Count; i++) {
//                effects[i].Play();
//            }

//            yield return IEShowMessage("WAVE CLEAR!");

//            yield return new WaitForSeconds(1f);

//            yield return IEShowMessage("PREPARE!");

//            Gameplay.Intansce.MoneyUpdate.DP += levelData.startDP;
//        }
//    }

//    public IEnumerator IEShowMessage(string message) {
//        float MOVE_TIME = 0.5f;

//        txtReady.text = message;
//        txtReady.transform.position = new Vector2(-Screen.width*1.5f, txtReady.transform.position.y);
//        LeanTween.move(txtReady.gameObject, new Vector3(Screen.width/2, txtReady.transform.position.y), MOVE_TIME).setEaseOutCubic();

//        yield return new WaitForSeconds(MOVE_TIME);

//        yield return new WaitForSeconds(MOVE_TIME);
//        //txtReady.transform.position = new Vector2(-Screen.width, txtReady.transform.position.y);
//        LeanTween.move(txtReady.gameObject, new Vector3(Screen.width * 1.5f, txtReady.transform.position.y), MOVE_TIME);
//    }
//}
