using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int savedLevel;
    public float savedHealth;
    public float[] savedPlayerPos;
    public string savedName;
    public float savedExp;

    public PlayerData (Stats.BaseStats player)
    {
        savedLevel = player.level;
        savedHealth = player.characterStatus[0].currentValue;
        savedName = player.name;
        savedExp = player.currentExp;

        savedPlayerPos = new float[3];
        savedPlayerPos[0] = player.transform.position.x;
        savedPlayerPos[1] = player.transform.position.y;
        savedPlayerPos[2] = player.transform.position.z;
    }
    
}
