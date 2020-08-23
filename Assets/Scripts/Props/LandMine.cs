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
    public class LandMine: MonoBehaviour
    {
        [SerializeField]
        WeaponData mineAttackData;
        [SerializeField]
        UnityEngine.Events.UnityEvent mineEvent;

        bool active = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && active == false)
            {
                active = true;
                PlayerCharacter c = other.GetComponent<PlayerCharacter>();
                c.CharacterDamage.Damage(c.transform.position, new AttackData(mineAttackData.AttackProcessor, null, mineAttackData));
                //c.CharacterMovement.ResetPosition(new Vector3(0, 2, 0));
                c.CharacterMovement.SetInput(false);
                c.CharacterTriAttack.CallWallCollision(99);
                mineEvent.Invoke();
                GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(FallCoroutine(c));
            }
        }

        private IEnumerator FallCoroutine(Character c)
        {
            yield return new WaitForSeconds(2f);
            c.CharacterMovement.SetInput(true);
            Destroy(this.gameObject);
        }

    } 

} // #PROJECTNAME# namespace