using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
namespace Fruit
{
    public class Island : MonoBehaviour
    {

        public enum ISLAND_STATE
        {
            PAUSE,
            ACTIVE,
            PREPARE
        }

        public GameObject gun;

        public Transform headGun;
        [HideInInspector]
        public List<EnemyFruitBullet> bulletModels;
        [HideInInspector]
        public float cooldownShooting;
        [HideInInspector]
        public float deltatimeUpdateShooting;

        public ISLAND_STATE state;
        [HideInInspector]
        public Camera cam;

        void Setup(Camera cam, List<EnemyFruitBullet> bulletModels)
        {
            this.cam = cam;
            this.bulletModels = bulletModels;
            deltatimeUpdateShooting = Constants.SPEED_SHOOTING + Random.Range(0f, 1f) * Constants.SPEED_SHOOTING;
        }

        public void Update()
        {
            if (state == ISLAND_STATE.ACTIVE)
            {
                Quaternion startAngle = transform.rotation;
                Vector3 dir = (cam.transform.position - gun.transform.position).normalized;
                var rotation = Quaternion.LookRotation(dir);
                var eulerAngle = Quaternion.ToEulerAngles(rotation);
                eulerAngle.z = 0;
                eulerAngle.y = eulerAngle.y * Mathf.Rad2Deg;
                eulerAngle.x = 0;
                gun.transform.localEulerAngles = eulerAngle;

                deltatimeUpdateShooting += Time.deltaTime;
                if (deltatimeUpdateShooting >= cooldownShooting && Random.Range(0, 10) == 0)
                {
                    deltatimeUpdateShooting = 0;
                    int typeBullet = Random.Range(0, bulletModels.Count);
                    GameObject bulletGo = Instantiate(bulletModels[typeBullet].gameObject, headGun.transform.position, Quaternion.identity);
                    EnemyFruitBullet bulletScript = bulletGo.GetComponent<EnemyFruitBullet>();
                    bulletScript.Setup(cam.transform.position, 1);
                }
            }

        }

        public void PauseGun()
        {
            state = ISLAND_STATE.PAUSE;
        }

        public void ActiveGun()
        {
            state = ISLAND_STATE.ACTIVE;
        }

        public void PrepareGun()
        {
            StartCoroutine(IEPrepareGun());
        }

        IEnumerator IEPrepareGun()
        {
            yield return null;
        }
    }
}
