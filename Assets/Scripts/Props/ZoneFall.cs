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
    public class ZoneFall: MonoBehaviour
    {

        [SerializeField]
        WeaponData fallAttackData;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerCharacter c = other.GetComponent<PlayerCharacter>();
                c.CharacterDamage.Damage(c.transform.position, new AttackData(fallAttackData.AttackProcessor, null, fallAttackData));
                c.CharacterMovement.ResetPosition(new Vector3(0, 2, 0));
                c.CharacterMovement.SetInput(false);
                c.CharacterTriAttack.CallWallCollision(99);
                StartCoroutine(FallCoroutine(c));
            }
        }

        private IEnumerator FallCoroutine(Character c)
        {
            yield return new WaitForSeconds(2f);
            c.CharacterMovement.SetInput(true);
        }
        /*private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerCharacter>().Interactions.Remove(this);
            }
        }*/

    } 

} // #PROJECTNAME# namespace