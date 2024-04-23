using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<GameObject> heroRewards = new List<GameObject>();
    public List<string> rewardsTest = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GemCheck(int gemAmount)
    {
        if(GameSystem.userdata.diamond >= gemAmount)
        {
            GameSystem.userdata.diamond -= gemAmount;
            GachaReward();
        }
        else
        {
            Debug.Log("Not enough gem");
        }
    }

    public void GachaManyTimesGemCheck(int amount)
    {
        rewardsTest = new List<string>();
        int time = 0;
        if(GameSystem.userdata.diamond >= amount)
        {
            while(time < 10)
            {
                GachaReward();
                time++;
            }
        }
    }

    public void GachaReward()
    {
        int rand = Random.Range(0,100);
        if(rand <= 50)
        {
            rewardsTest.Add("C");
        }
        else if(rand > 50 && rand<= 70)
        {
            rewardsTest.Add("R");
        }
        else if(rand > 70 && rand <= 85)
        {
            rewardsTest.Add("S");
        }
        else if(rand > 85 && rand <= 95)
        {
            rewardsTest.Add("SR");
        }
        else
        {
            rewardsTest.Add("SSR");
        }
        UpdateGachaRewards();
    }

    public void UpdateGachaRewards()
    {
        int rankC = 0;
        int rankR = 0;
        int rankS = 0;
        int rankSR = 0;
        int rankSSR = 0;
        foreach(string rank in rewardsTest)
        {
            switch (rank)
            {
                case "C":
                    rankC++;
                    break;
                case "R":
                    rankR++;
                    break;
                case "S":
                    rankS++;
                    break;
                case "SR":
                    rankSR++;
                    break;
                case "SSR":
                    rankSSR++;
                    break;
            }
        }
    }
}
