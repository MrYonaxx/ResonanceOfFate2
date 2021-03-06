﻿/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class Bullet: MonoBehaviour, IAttackCollision
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        /*[SerializeField]
        Transform muzzle = null;*/
        [SerializeField]
        float aimScaling = 10f;
        [SerializeField]
        Transform muzzleAnimation = null;
        [SerializeField]
        GameObject onHitParticle = null;

        [SerializeField]
        bool stopDash = false;

        AttackData attackData = null;

        private AudioSource gunSound;
        private ParticleSystem part;
        private List<ParticleCollisionEvent> collisionEvents;

        void Awake()
        {
            gunSound = GetComponent<AudioSource>();
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public void SetAttackData(AttackData attack)
        {
            attackData = attack;

            ParticleSystem.ShapeModule editableShape = part.shape;
            float variance = attack.AttackDataStat.Accuracy * (attack.AttackDataStat.AccuracyVariance * 0.01f);
            float accuracy = attack.AttackDataStat.Accuracy + Random.Range(-variance, variance);
            editableShape.angle = aimScaling - (accuracy / aimScaling);
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void Play(Transform target)
        {
            this.transform.LookAt(target);
            //muzzle.LookAt(target);
            part.Play();
            if (gunSound != null)
                gunSound.PlayOneShot(gunSound.clip);
            Instantiate(muzzleAnimation, this.transform.position, Quaternion.identity);
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            if (other.tag != this.tag)
            {
                Instantiate(onHitParticle, collisionEvents[0].intersection, Quaternion.identity);
                if (attackData == null)
                    return;
                for (int i = 0; i < numCollisionEvents; i++)
                {
                    CheckDamage(other);
                }
            }
        }

        void CheckDamage(GameObject other)
        {
            var d = other.GetComponent<IDamageable>();
            if (d == null)
                return;
            d.Damage(collisionEvents[0].intersection, attackData);

            if (stopDash == true)
            {
                var c = other.GetComponent<PlayerCharacter>();
                c.CharacterTriAttack.CallWallCollision(99);
            }

        }

        #endregion

    } 

} // #PROJECTNAME# namespace