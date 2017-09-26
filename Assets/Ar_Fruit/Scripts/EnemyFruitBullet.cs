using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Fruit
{
    public class EnemyFruitBullet : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 targetMoving;
        [HideInInspector]
        public Vector3 speed;

        public int damage;
        [HideInInspector]
        public bool isHitPlayer;
        [HideInInspector]
        public bool isStartRemove;

        public void Setup(Vector3 destAttack, int damage)
        {
            this.targetMoving = destAttack;
            float distanceXZ = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destAttack.x, destAttack.z));
            float timeBulletMove = distanceXZ / Constants.DEFAULT_SPEED_XZ;
            speed = Vector3.zero;
            speed.x = (destAttack.x - transform.position.x) / timeBulletMove;
            speed.z = (destAttack.z - transform.position.z) / timeBulletMove;
            speed.y = (destAttack.y - transform.position.y - 0.5f * Constants.GRAVITY_FRUIT_BULLET * timeBulletMove * timeBulletMove) / timeBulletMove;

            isHitPlayer = true;
        }

        public void Update()
        {
            if (!isHitPlayer && !isStartRemove)
            {
                // Update XZ,
                speed.y = speed.y + Constants.GRAVITY_FRUIT_BULLET * Time.deltaTime;
                transform.position += speed * Time.deltaTime;
                // Update Y
                if (transform.position.y < -5f)
                {
                    StartCoroutine(IERemove());
                }
            }

        }

        private void OnCollisionEnter(Collision collision)
        {

            if (!isStartRemove)
            {
                if (collision.gameObject.tag == "Player")
                {
                    isHitPlayer = true;
                    isStartRemove = true;
                    StartCoroutine(IERemove());
                    FruitPlayer player = collision.gameObject.GetComponent<FruitPlayer>();
                    player.TakeDamage(collision.contacts[0].point, damage);
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

        public void OnHit()
        {
            if (!isStartRemove)
            {
                isStartRemove = true;
                StartCoroutine(IERemove());
            }

        }

    }
}
