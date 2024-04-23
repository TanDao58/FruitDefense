using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public HeroCard[] heroCards;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        heroCards = GetComponentsInChildren<HeroCard>();
    }
}
