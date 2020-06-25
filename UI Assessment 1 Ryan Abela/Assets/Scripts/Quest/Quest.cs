using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    // Start is called before the first frame update
    public QuestState questState;
    public string title;
    public string description;
    public string condition1, condition2, condition3;
    public int expReward;
    public int goldReward;
    public QuestGoal goal;
    public void Complete()
    {
        questState = QuestState.Complete;
        Debug.Log(title + " was Complete");
    }
    public void Claimed()
    {
        questState = QuestState.Claimed;
    }
   
}

