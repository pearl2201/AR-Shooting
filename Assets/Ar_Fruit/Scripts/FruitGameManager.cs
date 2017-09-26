using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;
using Dinosaur.Scripts;
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
        [HideInInspector]
        public GAMESTATE gameState;
        [HideInInspector]
        public int indexRound;

        public Camera cam;
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
        private Slider slider;
        [SerializeField]
        private Text txtStartRound;

        private GameObject btnShooting;
        private void Start()
        {
            gameState = GAMESTATE.CHECK_PLANE;
            if (Application.isEditor)
            {
                StartGame(new Vector3(0, -7.3f, 22.5f));
            }
        }


        public void StartGame(Vector3 positionInitWorld)
        {
            gameState = GAMESTATE.INIT;
            txtRequirePlane.gameObject.SetActive(false);
            // arPlaneMarginDetect.gameObject.SetActive(false);
            btnShooting.gameObject.SetActive(true);
            worldLand.gameObject.SetActive(true);
            worldLand.transform.position = positionInitWorld;
            worldLand.Setup();
            SetupRound(0);
        }

        private void Update()
        {
            if (gameState == GAMESTATE.PLAYING)
            {
                if (indexRound <= 3)
                {
                    cooldownRound -= Time.deltaTime;
                    slider.value = cooldownRound / Constants.TIME_PER_ROUND;
                    if (cooldownRound <= 0)
                    {
                        NextRound();
                    }
                }

            }

        }

        public void NextRound()
        {
            worldLand.PauseIsland();
            EventDispatcher.Instance.PostEvent(EventID.OnOverRound);
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
            txtStartRound.text = "Round " + (indexRound + 1);
            float oldScrollBarValue = slider.value;
            worldLand.SetNextRound();
            float p = 0;
            while (p < 1)
            {

                p += Time.deltaTime / Constants.TIME_SETUP_ROUND;
                p = Mathf.Clamp01(p);
                slider.value = Mathf.Lerp(oldScrollBarValue, 1, p);

                yield return null;
            }
            txtStartRound.gameObject.SetActive(false);
            gameState = GAMESTATE.PLAYING;
            worldLand.UnPauseIsland();
        }

        public void GameOver()
        {
            gameState = GAMESTATE.END;
            gameoverPopup.gameObject.SetActive(true);
        }

        public void UpdateTextHp(int hp)
        {
            txtHp.text = "HP: " + hp;
        }
    }
}
