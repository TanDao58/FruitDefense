using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [SerializeField] private Tutorial[] tutorials;

    private void Awake()
    {
        Instance = this;
    }

    public void DoDragTutorial()
    {
        Instantiate(tutorials[0], Gameplay.Intansce.canvas.transform);
    }
}
