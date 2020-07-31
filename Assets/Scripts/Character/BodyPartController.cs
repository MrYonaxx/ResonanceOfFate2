/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BlinkScript))]
    public class BodyPartController: MonoBehaviour
    {

        [HorizontalGroup("Render")]
        [HideLabel]
        [SerializeField]
        SpriteRenderer sprite;
        [HorizontalGroup("Render")]
        [HideLabel]
        [SerializeField]
        BlinkScript blink;

        [SerializeField]
        [HideLabel]
        BodyPartDestroyed bodyPartDestroyed;

        [Title("Parameter")]
        [SerializeField]
        [ProgressBar(0, 3)]
        protected int layer = 1;
        public int Layer
        {
            get { return layer; }
        }
        [HorizontalGroup("Angle")]
        [SerializeField]
        protected float angleMin;
        public float AngleMin
        {
            get { return angleMin; }
        }
        [HorizontalGroup("Angle")]
        [SerializeField]
        protected float angleMax;
        public float AngleMax
        {
            get { return angleMax; }
        }

        //[SerializeField]
        //protected bool aerialPart;



        /*[SerializeField]
        StatController statData;*/

        [Space]
        [HorizontalGroup("List")]
        [SerializeField]
        List<BodyPartController> linkedBodyPart;

        /*[Space]
        [HorizontalGroup("List")]
        [SerializeField]
        List<GameObject> objectToDisappear;*/

        /*[Space]
        [HorizontalGroup("List")]
        [SerializeField]
        List<StatModifier> bodyPartBonus;*/


        [Space]
        [Title("Stats")]
        [HideLabel]
        [SerializeField]
        CharacterData partData;

        [Space]
        [SerializeField]
        UnityEvent onDestroy;


        protected CharacterStatController statController;
        public CharacterStatController StatController
        {
            get { return statController; }
        }







        [ExecuteInEditMode]
        private void Reset()
        {
            sprite = GetComponent<SpriteRenderer>();
            blink = GetComponent<BlinkScript>();
        }

        public virtual void InitializeBodyPart()
        {
            statController = new CharacterStatController(partData);
            /*for (int i = 0; i < bodyPartBonus.Count; i++)
            {
                characterStatController.StatController.AddStat(new Stat(bodyPartBonus[i].StatName, bodyPartBonus[i].StatValue), bodyPartBonus[i].ModifierType);
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
        }

        public virtual void PartDestroy()
        {
            /*for (int i = 0; i < objectToDisappear.Count; i++)
            {
                objectToDisappear[i].SetActive(false);
            }*/
            statController.Hp = 0;
            BodyPartDestroyed b = Instantiate(bodyPartDestroyed, this.transform.position, Quaternion.identity);
            b.CreateBodyPart(sprite);
            for (int i = 0; i < linkedBodyPart.Count; i++)
            {
                linkedBodyPart[i].PartDestroy();
            }
            onDestroy.Invoke();
            this.gameObject.SetActive(false);
        }
    } 

} // #PROJECTNAME# namespace