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
    public class EnemyHeroAction: CharacterHeroAction
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterHeroAction[] playerHeroAction;


        List<float> intersectionEnemyT = new List<float>();
        List<Animator> intersectionsEnemyList = new List<Animator>();
        bool enemyIntersection = false;

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

        private void Awake()
        {
            base.Awake();
            intersectionEnemyT = new List<float>();
            intersectionsEnemyList.Add(Instantiate(intersectionPrefab, this.transform));

            // Automatisé playerHeroAction sinon il risque d'y avoir des cas avec des erreurs
            for (int i = 0; i < playerHeroAction.Length; i++)
            {
                intersectionEnemyT.Add(-1);
                intersectionsEnemyList.Add(Instantiate(intersectionPrefab, this.transform));
            }

        }


        public bool CheckPlayerIntersections()
        {
            Vector2 playerPos = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 cursorDirection = new Vector2((cursor.transform.position - this.transform.position).x, (cursor.transform.position - this.transform.position).z);

            Vector2 playerPosition;
            Vector2 playerCursorDirection;
            float intersection = 0;
            for (int i = 0; i < playerHeroAction.Length; i++)
            {
                intersection = -1;
                if (playerHeroAction[i].IsCursorActive() == true)
                {
                    playerPosition = new Vector2(playerHeroAction[i].transform.position.x, playerHeroAction[i].transform.position.z);
                    playerCursorDirection = playerHeroAction[i].GetCursorPositionV2() - playerPosition;
                    intersection = CheckIntersection(playerPos, cursorDirection, playerPosition, playerCursorDirection);
                }
                if (intersection >= 0)
                {
                    if (intersectionEnemyT[i] == -1) // Pas d'antécédant donc feedback
                    {
                        FeedbackLine(0);
                        Feedback.StartAfterImage();
                        enemyIntersection = true;
                    }
                    intersectionEnemyT[i] = intersection;
                    DrawEnemyIntersection(i, new Vector3(cursorDirection.x, 0, cursorDirection.y) * intersection);

                }
                else if (intersectionEnemyT[i] != -1) // Autrefois y'avait intersection donc maintenant qu'il n'y en a plus, faut effacer
                {
                    HideEnemyIntersection(i);
                    intersectionEnemyT[i] = -1;
                    for (int k = 0; k < intersectionEnemyT.Count; k++)
                    {
                        if (intersectionEnemyT[k] != -1)
                            return enemyIntersection;
                    }
                    UnfeedbackLine();
                    enemyIntersection = false;
                    Feedback.EndAfterImage();
                }

            }
            return enemyIntersection;
        }



        private void DrawEnemyIntersection(int id, Vector3 intersec)
        {
            intersectionsEnemyList[id].gameObject.SetActive(true);
            if (Physics.Raycast(this.transform.position + intersec, Vector3.down, out hit, 10f, layerMask))
            {
                intersectionsEnemyList[id].transform.position = hit.point + offset;
            }
            else
            {
                intersectionsEnemyList[id].transform.position = this.transform.position + intersec - new Vector3(0, this.transform.localPosition.y, 0) + offset;
            }
            intersectionsEnemyList[id].SetTrigger("Feedback");
        }

        private void HideEnemyIntersection(int id)
        {
            intersectionsEnemyList[id].gameObject.SetActive(false);
        }



        private void UnfeedbackLine()
        {
            HeroActionLine lineToRender = GetLine(0);
            for (int i = 0; i < lineToRender.line.Count; i++)
            {
   
                lineToRender.line[i].SetTrigger("Reset");
            }
        }




        protected override void HideAllIntersection()
        {
            base.HideAllIntersection();
            for (int i = 0; i < intersectionsEnemyList.Count; i++)
            {
                HideEnemyIntersection(i);
            }
        }

        protected override void UpdateOtherLines()
        {
            base.UpdateOtherLines();
            for (int i = 0; i < intersectionsEnemyList.Count; i++)
            {
                intersectionsEnemyList[i].transform.rotation = globalCamera.Rotation();
            }
        }



        #endregion

    } 

} // #PROJECTNAME# namespace