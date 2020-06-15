using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    public class BaseStats : MonoBehaviour
    {
        #region Structs 
        [System.Serializable]
        public struct StatBlock
        {
            public string name;
            public int value;
        }

        [System.Serializable]
        public struct LifeForceStatus
        {
            public string name;
            public float currentValue;
            public float maxValue;
            public float regenValue;
            public Image displayImage;
        }
        #endregion

        #region Variables
        [Header("Character Data")]
        public new string name;
        public StatBlock[] characterstats = new StatBlock[6];
        public LifeForceStatus[] characterStatus = new
            LifeForceStatus[3];
        [Header("Character Info")]
        public int level = 0;
        public float currentExp, neededExp, maxExp;
        public Quest quest;

        [Header("Death")]
        public Image damageImage;
        public Image deathImage;
        public Text deathText;
        public AudioClip deathClip;
        public AudioSource playersAudio;
        public string[] deathMessages;
        public string[] reviveMessages;

        public float flashSpeed = 5;
        public Color flashColour = new Color(1, 0, 0, 0.2f);
        public static bool isDead;
        //REMOVE LATER
        public bool damaged;
        public Vector3 savedPosition;

        

        #endregion
        // Start is called before the first frame update
        void Start()
        {
            LoadPlayer();
        }

        // Update is called once per frame
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.X))
            {
                damaged = true;
                characterStatus[0].currentValue -= 25;
            }
#endif

            for (int i = 0; i < characterStatus.Length; i++)
            {
                characterStatus[i].displayImage.fillAmount = Mathf.Clamp01(characterStatus[i].currentValue / characterStatus[i].maxValue);
            }



        }
        private void LateUpdate()
        {
            if (characterStatus[0].currentValue <= 0 && !isDead)
            {
                //Death Function
                Death();
            }
        }
        void Death()
        {
            //Set death flag to dead and clear existing text
            isDead = true;
            deathText.text = deathMessages[Random.Range(0, 4)];
            //play death audio
            playersAudio.clip = deathClip;
            playersAudio.Play();
            //Trigger
            deathImage.GetComponent<Animator>().SetTrigger("isDead");
            //2 Death Text
            //6 Rev Text
            //9 Respawn Function
        }
        void Revive()
        {
            deathText.text = "";
            isDead = false;
            characterStatus[0].currentValue = characterStatus[0].maxValue;
            //load position

            deathImage.GetComponent<Animator>().SetTrigger("Respawn");
        }
        #region Save and Load
        public void SavePlayer()
        {
            Save.SavePlayer(this);
        }
        public void LoadPlayer()
        {
            PlayerData data = Save.LoadPlayer();

            level = data.savedLevel;
            characterStatus[0].currentValue = data.savedHealth;
            name = data.savedName;
            currentExp = data.savedExp;

            Vector3 position;
            position.x = data.savedPlayerPos[0];
            position.y = data.savedPlayerPos[1];
            position.z = data.savedPlayerPos[2];
            transform.position = position;
            Debug.Log("Saved");
            savedPosition = position;
        }
        #endregion
        #region  Quest
        public void KilledCreature(string enemyTag)
        {
            if (quest.questState == QuestState.Active)
            {
                quest.goal.EnemyKilled(enemyTag);
                
            }
        }
        public void IemCreature(int id)
        {
            if (quest.questState == QuestState.Active)
            {
                quest.goal.ItemCollected(id);
               
            }
        }
        #endregion
    }
}
