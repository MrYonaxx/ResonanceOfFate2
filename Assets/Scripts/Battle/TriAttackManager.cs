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
using Hydra.HydraCommon.Utils.Comparers;

namespace VoiceActing
{
    public struct TriAttackPosition
    {
        public int PartyID;
        public Vector2 Position;

        public TriAttackPosition(int id, Vector2 pos)
        {
            PartyID = id;
            Position = pos;
        }
    }

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

        int resonancePoint = 3;
        public int ResonancePoint
        {
            get { return resonancePoint; }
            set { resonancePoint = value;
                 textResonance.text = resonancePoint.ToString();
            }
        }

        bool triAttackReverse = false;

        ClockwiseComparerTri clockwiseComparer = null;
        List<TriAttackPosition> triAttackPositions = new List<TriAttackPosition>();

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
            if (party.Count >= 4)
            {
                PrepareQuadAttack(startIndex, reverse);
                return;
            }

            triAttackPositions.Clear();
            triAttackPositions = new List<TriAttackPosition>();
            for (int k = 0; k < party.Count; k++)
            {
                triAttackPositions.Add(new TriAttackPosition(k, new Vector2(party[k].transform.position.x, party[k].transform.position.z)));
            }

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




        private void PrepareQuadAttack(int startIndex, bool reverse)
        {
            triAttackPositions.Clear();
            triAttackPositions = new List<TriAttackPosition>();
            float xOrigin = 0;
            float yOrigin = 0;
            for (int i = 0; i< party.Count; i++)
            {
                triAttackPositions.Add(new TriAttackPosition(i, new Vector2(party[i].transform.position.x, party[i].transform.position.z)));
                xOrigin += party[i].transform.position.x;
                yOrigin += party[i].transform.position.z;
            }
            xOrigin *= 0.25f;
            yOrigin *= 0.25f;
            if (clockwiseComparer == null)
                clockwiseComparer = new ClockwiseComparerTri(new Vector2(xOrigin, yOrigin));//triAttackPositions[startIndex].Position);
            else
                clockwiseComparer.origin = new Vector2(xOrigin, yOrigin);// triAttackPositions[startIndex].Position;

            triAttackPositions.Sort(clockwiseComparer);

            int loop = 0;
            indexLeader = startIndex;
            int index = indexLeader;
            int indexNext = indexLeader;
            while (loop < party.Count)
            {
                loop += 1;
                if (reverse == true)
                    indexNext -= 1;
                else
                    indexNext += 1;

                if (indexNext >= party.Count)
                    indexNext = 0;
                else if (indexNext < 0)
                    indexNext = party.Count - 1;

                party[triAttackPositions[index].PartyID].CharacterHeroAction.SetCursor(party[triAttackPositions[indexNext].PartyID].transform.position);
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
            int id = 0;
            int nextID = 0;
            for (int i = 0; i <= resonancePoint; i++)
            {
                for (int j = 0; j < party.Count; j++)
                {
                    /*if(reverse == false)
                        party[j].CharacterTriAttack.AddTriAttackPosition(party[(j + i) % party.Count].transform.position);
                    else
                        party[j].CharacterTriAttack.AddTriAttackPosition(party[((j + (party.Count*99))- i)  % party.Count].transform.position); // Le *99 c'est un peu viteuf, c'est pour éviter le -1*/

                    id = triAttackPositions[j].PartyID;
                    if (reverse == false) 
                    {
                        nextID = triAttackPositions[(j + i) % party.Count].PartyID;
                    }
                    else
                    {
                        nextID = triAttackPositions[((j + (party.Count * 99)) - i) % party.Count].PartyID;
                    }
                    party[id].CharacterTriAttack.AddTriAttackPosition(party[nextID].transform.position);
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