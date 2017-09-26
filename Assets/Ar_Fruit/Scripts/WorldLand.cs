using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
namespace Fruit
{
    public class WorldLand : MonoBehaviour
    {
        public List<Island> islands;

        public Vector3[] originalScaleIsland;
        public void Setup()
        {
            originalScaleIsland = new Vector3[islands.Count];
            for (int i = 0; i < islands.Count; i++)
            {
                originalScaleIsland[i] = islands[i].transform.localScale;
                islands[i].Setup(gameManager.cam, gameManager.bulletModels);
            }
        }

        public FruitGameManager gameManager;

        public void SetNextRound()
        {
            islands[gameManager.indexRound].gameObject.SetActive(true);
            islands[gameManager.indexRound].transform.localScale = Vector3.zero;
            StartCoroutine(IESetupNextRound());
        }

        public IEnumerator IESetupNextRound()
        {
            float p = 0;
            while (p < 1)
            {
                p += Time.deltaTime / 0.2f;
                p = Mathf.Clamp01(p);
                islands[gameManager.indexRound].transform.localScale = originalScaleIsland[gameManager.indexRound] * p;
                yield return null;
            }
        }

        public void UpdateNextRoundScale(float scl)
        {
            Debug.Log("index: " + gameManager.indexRound);

        }

        public void PauseIsland()
        {
            for (int i = 0; i < islands.Count; i++)
            {
                islands[i].PauseGun();
            }
        }

        public void UnPauseIsland()
        {
            for (int i = 0; i < islands.Count; i++)
            {
                islands[i].ActiveGun();
            }
        }
    }
}
