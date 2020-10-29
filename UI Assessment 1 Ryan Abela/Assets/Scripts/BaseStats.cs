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
        public Text reviveText;
        public AudioClip deathClip;
        public AudioSource playersAudio;
        public string[] deathMessages;
        public string[] reviveMessages;
        public Transform currentCheckpoint;
        int msgIndex;

        public float flashSpeed = 5;
        public Color flashColour = new Color(1, 0, 0, 0.2f);
        public Color transparent = new Color(0, 0, 0, 0);
        public Color deathTextColor = new Color(255, 0, 0, 1);
        public static bool isDead;
        //REMOVE LATER
        public bool damaged = false;
        public Vector3 savedPosition;

        

        #endregion
        // Start is called before the first frame update
        void Start()
        {
            deathText.color = transparent;
            reviveText.color = transparent;
            damaged = false;
            isDead = false;
            LoadPlayer();
            msgIndex = Random.Range(0, 4);
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
            if (damaged == true && !isDead)
            {
                damageImage.color = flashColour;
                damaged = false;
            }
            else if (damageImage.color.a > 0)
            {
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
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
            deathText.text = deathMessages[msgIndex];
            deathText.color = deathTextColor;
            reviveText.text = "";
            //play death audio
            playersAudio.clip = deathClip;
            playersAudio.Play();
            //Trigger
            deathImage.GetComponent<Animator>().SetTrigger("isDead");
            Invoke("ReviveText", 2f);
            Invoke("Revive", 6f);
            //2 Death Text
            //6 Rev Text
            //9 Respawn Function
        }
        void ReviveText()
        {
            reviveText.color = deathTextColor;
            reviveText.text = reviveMessages[msgIndex];
            
        }
        void Revive()
        {
            deathText.color = transparent;
            reviveText.color = transparent;
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
