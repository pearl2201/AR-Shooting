using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Dinosaur.Scripts;
namespace Fruit
{
    public class EnemyFruitBullet : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 targetMoving;
        //[HideInInspector]
        public Vector3 speed;

        public int damage;
        [HideInInspector]
        public bool isHitPlayer;
        [HideInInspector]
        public bool isStartRemove;

        private Camera cam;
        [SerializeField]
        private GameObject particleExplo;

        private void OnEnable()
        {
            this.RegisterListener(EventID.OnOverRound, (sender, param) => Remmove());
        }



        public void Setup(Camera cam,Vector3 destAttack, int damage)
        {
            this.cam = cam;
            this.targetMoving = destAttack;
            float distanceXZ = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destAttack.x, destAttack.z));
            float timeBulletMove = distanceXZ / Constants.DEFAULT_SPEED_XZ;
            
            speed = Vector3.zero;
            speed.x = (destAttack.x - transform.position.x) / timeBulletMove;
            speed.z = (destAttack.z - transform.position.z) / timeBulletMove;
            speed.y = (destAttack.y - transform.position.y - 0.05f - 0.5f * Constants.GRAVITY_FRUIT_BULLET * timeBulletMove * timeBulletMove) / timeBulletMove;

            isHitPlayer = false;
            isStartRemove = false;
        }

        public void Update()
        {
            if (!isHitPlayer && !isStartRemove)
            {
                // Update XZ,
                speed.y = speed.y + Constants.GRAVITY_FRUIT_BULLET * Time.deltaTime;
                transform.position += speed * Time.deltaTime;
                Vector3 axis = cam.transform.right;
                transform.RotateAroundLocal(axis, Constants.DEFAULT_SPEED_ROTATE * Time.deltaTime);
                // Update Y
                if (transform.position.y < -5f)
                {
                    StartCoroutine(IERemove());
                }
            }

        }


        void OnCollisionEnter(Collision collision)
        {

            if (!isStartRemove)
            {
                if (collision.gameObject.tag == "Player")
                {
                  
                    isHitPlayer = true;
                    isStartRemove = true;
                    Explo();
                    FruitPlayer player = collision.gameObject.GetComponent<FruitPlayer>();
                    player.TakeDamage(transform.position, damage);
                    StartCoroutine(IERemove());
                }

            }
        }

        public IEnumerator IERemove()
        {
            isStartRemove = true;
            float p = 0;
            Vector3 prevScale = transform.localScale;
            while (p <= 1f)
            {
                p += Time.deltaTime / Constants.TIME_REMOVE_ENEMY;
                transform.localScale = Vector3.Lerp(prevScale, Vector3.zero, p);
                yield return null;
            }
            Destroy(gameObject);

        }

        public void Remmove()
        {
            if (!isStartRemove)
            {
                StartCoroutine(IERemove());
            }

        }

        public void OnHit()
        {
            if (!isStartRemove)
            {
                isStartRemove = true;
                Explo();
                StartCoroutine(IERemove());
            }

        }

        public void Explo()
        {
            GameObject go = Instantiate(particleExplo);
            go.transform.position = transform.position;

        }

    }
}
