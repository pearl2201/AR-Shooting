using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Fruit
{
    public class FruitGameManager : MonoBehaviour
    {
        public enum GAMESTATE
        {
            CHECK_PLANE,
            INIT,
            INFO,
            PLAYING,
            END
        }
        public GAMESTATE gameState;

        public int indexRound;

        private float cooldownRound;
     
        public List<EnemyFruitBullet> bulletModels;
        [SerializeField]
        private GameObject txtRequirePlane;

        public GameObject arPlaneMarginDetect;
        [SerializeField]
        private WorldLand worldLand;
        [SerializeField]
        private Text txtHp;
        [SerializeField]
        private FruitGameOverPopup gameoverPopup;
        [SerializeField]
        private Scrollbar scrollBar;
        [SerializeField]
        private Text txtStartRound;

        private void Start()
        {
            gameState = GAMESTATE.CHECK_PLANE;

        }


        public void StartGame(Vector3 positionInitWorld)
        {
            gameState = GAMESTATE.INIT;
            txtRequirePlane.gameObject.SetActive(false);
            arPlaneMarginDetect.gameObject.SetActive(false);
            worldLand.gameObject.SetActive(true);
            worldLand.transform.position = positionInitWorld;
            SetupRound(0);
        }

        private void Update()
        {
            if (gameState == GAMESTATE.PLAYING)
            {
                cooldownRound -= Time.deltaTime;
                scrollBar.value = cooldownRound / Constants.TIME_PER_ROUND;
                if (cooldownRound <= 0)
                {
                    NextRound();
                }
            }

        }

        public void NextRound()
        {

            SetupRound(indexRound + 1);
        }

        public void SetupRound(int indexRound)
        {
            this.indexRound = indexRound;
            gameState = GAMESTATE.INFO;
            cooldownRound = Constants.TIME_PER_ROUND;
            StartCoroutine(IESetupRound());
        }

        IEnumerator IESetupRound()
        {
            txtStartRound.gameObject.SetActive(true);
            txtStartRound.text = "Round " + indexRound;
            float oldScrollBarValue = scrollBar.value;
            worldLand.SetNextRound();
            float p = 0;
            while (p < 1)
            {

                p += Time.deltaTime / Constants.TIME_SETUP_ROUND;
                p = Mathf.Clamp01(p);
                scrollBar.value = Mathf.Lerp(oldScrollBarValue, 1, p);
                worldLand.UpdateNextRoundScale(p);
                yield return null;
            }
            txtStartRound.gameObject.SetActive(false);
            gameState = GAMESTATE.PLAYING;
        }

        public void GameOver()
        {
            gameState = GAMESTATE.END;
            gameoverPopup.gameObject.SetActive(true);
        }
    }
}
