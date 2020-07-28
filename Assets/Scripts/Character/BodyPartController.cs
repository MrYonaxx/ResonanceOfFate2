/*****************************************************************
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
    public class BodyPartController: MonoBehaviour
    {

        [SerializeField]
        protected float angleMin;
        public float AngleMin
        {
            get { return angleMin; }
        }

        [SerializeField]
        protected float angleMax;
        public float AngleMax
        {
            get { return angleMax; }
        }

        [SerializeField]
        protected bool aerialPart;

        [SerializeField]
        [ProgressBar(0, 3)]
        protected int layer = 1;
        public int Layer
        {
            get { return layer; }
        }

        /*[SerializeField]
        StatController statData;*/
        [SerializeField]
        List<StatModifier> bodyPartBonus;

        [SerializeField]
        protected CharacterStatController statController;
        public CharacterStatController StatController
        {
            get { return statController; }
        }

        [SerializeField]
        SpriteRenderer sprite;
        [SerializeField]
        BlinkScript blink;
        [SerializeField]
        BodyPartDestroyed bodyPartDestroyed;

        [ExecuteInEditMode]
        private void Reset()
        {
            sprite = GetComponent<SpriteRenderer>();
            blink = GetComponent<BlinkScript>();
        }

        public virtual void InitializeBodyPart()
        {
            //statController = new CharacterStatController()
            /*for (int i = 0; i < bodyPartBonus.Count; i++)
            {
                statController.StatController.AddStat(new Stat(bodyPartBonus[i].StatName, bodyPartBonus[i].StatValue), bodyPartBonus[i].ModifierType);
            }*/
        }

        public virtual bool CheckHit(float angle)
        {
            return (angleMin < angle && angle < angleMax);
        }

        public virtual DamageMessage Damage(AttackData attackData)
        {
            //statController.AddStat()
            blink.Blink();
            return attackData.AttackProcessor.ProcessAttack(attackData.AttackDataStat, statController);
            /*DamageMessage msg = 
            if (statController.Hp <= 0)
                PartDestroy()
            return msg;*/
        }

        public virtual bool PartDestroy()
        {
            if (statController.Hp <= 0)
            {
                // On fait des trucs
                BodyPartDestroyed b = Instantiate(bodyPartDestroyed, this.transform.position, Quaternion.identity);
                b.CreateBodyPart(sprite);
                this.gameObject.SetActive(false);
                return true;
            }
            return false;
        }
    } 

} // #PROJECTNAME# namespace