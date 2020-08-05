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
    public class TriAttackManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        List<PlayerCharacter> party = new List<PlayerCharacter>();

        [Title("HUD Resonance")]
        [SerializeField]
        TMPro.TextMeshProUGUI textResonance;

        int indexLeader = 0;
        public int IndexLeader
        {
            get { return indexLeader; }
        }

        private bool isTriAttacking = false;
        public bool IsTriAttacking
        {
            get { return isTriAttacking; }
        }
        private int numberAttacker = 0;

        int resonancePoint = 0;
        public int ResonancePoint
        {
            get { return resonancePoint; }
            set { resonancePoint = value;
                 textResonance.text = resonancePoint.ToString();
            }
        }

        bool triAttackReverse = false;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void SetParty(List<PlayerCharacter> characters)
        {
            if(party != null)
            {
                for (int i = 0; i < party.Count; i++)
                {
                    party[i].CharacterTriAttack.OnIntersection -= AddResonance;
                }
            }
            party = characters;
            for (int i = 0; i < party.Count; i++)
            {
                party[i].CharacterTriAttack.OnIntersection += AddResonance;
            }
        }

        public void PrepareTriAttack(int startIndex, bool reverse = false)
        {
            int i = 0;
            indexLeader = startIndex;
            int index = indexLeader;
            int indexNext = indexLeader;
            while (i < party.Count)
            {
                i += 1;
                if (reverse == true)
                {
                    indexNext -= 1;
                }
                else
                {
                    indexNext += 1;
                }

                if (indexNext >= party.Count)
                    indexNext = 0;
                else if (indexNext < 0)
                    indexNext = party.Count-1;
                party[index].CharacterHeroAction.SetCursor(party[indexNext].transform.position);
                index = indexNext;
            }
        }

        public void CancelTriAttack()
        {
            for(int i = 0; i < party.Count; i++)
            {
                party[i].CharacterHeroAction.Desactivate();
            }
        }


        public void StartTriAttack(int startIndex, bool reverse = false)
        {
            indexLeader = startIndex;
            isTriAttacking = true;
            numberAttacker = party.Count;
            for (int i = 0; i <= resonancePoint; i++)
            {
                for (int j = 0; j < party.Count; j++)
                {
                    if(reverse == false)
                        party[j].CharacterTriAttack.AddTriAttackPosition(party[(j + i) % party.Count].transform.position);
                    else
                        party[j].CharacterTriAttack.AddTriAttackPosition(party[((j + (party.Count*99))- i)  % party.Count].transform.position); // Le *99 c'est un peu viteuf, c'est pour éviter le -1
                }
            }
            for (int i = 0; i < party.Count; i++)
            {
                party[i].CharacterHeroAction.Desactivate();
                party[i].CharacterTriAttack.StartTriAttack();
            }
            ResonancePoint = 0;
            triAttackReverse = reverse;
        }

        public void EndTriAttack(int id)
        {
            if(isTriAttacking == true)
            {
                numberAttacker -= 1;
                if (numberAttacker == 0)
                {
                    isTriAttacking = false;
                }
                else
                {
                    if (id == indexLeader)
                    {
                        for(int i = 0; i < party.Count; i++)
                        {
                            if (triAttackReverse == true)
                                indexLeader -= 1;
                            else
                                indexLeader += 1;

                            if (indexLeader >= party.Count)
                                indexLeader = 0;
                            else if (indexLeader < 0)
                                indexLeader = party.Count - 1;

                            if (party[indexLeader].CharacterTriAttack.IsTriAttacking == true)
                                return;
                        }

                    }
                }
           
            }
        }

        //private IEnumerator 



        public void AddResonance()
        {
            ResonancePoint += 1;
        }
        public void RemoveResonance()
        {
            ResonancePoint -= 1;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace