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
    public struct HeroActionLine
    {
        public int id;
        public List<Animator> line;

        public HeroActionLine(int newID)
        {
            id = newID;
            line = new List<Animator>();
        }
    }

    public class CharacterHeroAction: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        protected CharacterController cursor;
        [SerializeField]
        GameObject cursorObject;

        [SerializeField]
        protected GlobalCamera globalCamera;
        [SerializeField]
        float speedCursor = 2;
        [SerializeField]
        List<CharacterHeroAction> allyTransform = new List<CharacterHeroAction>(); // à renomme mais je refactorai le truc quand y'aura 4 persos dans la team

        [Header("Draw")]
        [SerializeField]
        Animator spritePrefab;
        [SerializeField]
        protected Animator intersectionPrefab;
        [SerializeField]
        float spriteOffset;
        [SerializeField]
        Transform spriteDirection;

        [Header("Feedback")]
        [SerializeField] // Event ?
        AfterImageEffectScale feedback;
        public AfterImageEffectScale Feedback
        {
            get { return feedback; }
        }
        [SerializeField] // Event ?
        AudioClip heroActionStartClip;

        List<HeroActionLine> heroActionLines = new List<HeroActionLine>();
        bool isActive = false;

        List<Animator> intersectionAnimList = new List<Animator>();
        public List<Animator> IntersectionAnimList
        {
            get { return intersectionAnimList; }
        }
        List<float> intersectionT = new List<float>();
        public List<float> IntersectionT
        {
            get { return intersectionT; }
        }

        bool doesIntersect = false;
        public bool DoesIntersect
        {
            get { return doesIntersect; }
        }

        // debug
        protected Vector3 offset = new Vector3(0, 0.2f, 0);
        private IEnumerator lineUpdateCoroutine;

        AudioSource audioSource;

        protected int layerMask = (1 << 13);
        protected RaycastHit hit;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public Vector2 GetCursorPositionV2()
        {
            return new Vector2(cursor.transform.position.x, cursor.transform.position.z);
        }
        public Vector3 GetCursorPositionV3()
        {
            return cursorObject.transform.position;// - this.transform.localPosition;
        }

        public void ShowCursor(bool b)
        {
            cursorObject.gameObject.SetActive(b);
        }

        public bool IsCursorActive()
        {
            return cursorObject.gameObject.activeInHierarchy;// - this.transform.localPosition;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            intersectionT = new List<float>();
            intersectionT.Add(-1);
            intersectionAnimList.Add(Instantiate(intersectionPrefab, this.transform));
            for (int i = 0; i < allyTransform.Count-1; i++)
            {
                for (int j = i+1; j < allyTransform.Count; j++)
                {
                    intersectionT.Add(-1);
                    intersectionAnimList.Add(Instantiate(intersectionPrefab, this.transform));
                }
            }
        }

        public void Activate()
        {
           
        }

        public void Desactivate()
        {
            audioSource.Stop();
            isActive = false;
            doesIntersect = false;
            cursor.transform.localPosition = Vector3.zero;
            cursorObject.gameObject.SetActive(false);
            HideAllIntersection();
            for (int i = 0; i < heroActionLines.Count; i++)
                HideLine(i);
            if (lineUpdateCoroutine != null)
                StopCoroutine(lineUpdateCoroutine);
        }
        public void PerformHeroAction(float directionX, float directionZ)
        {
            if (isActive == false && (directionX != 0 || directionZ != 0))
            {
                AudioManager.Instance.PlaySound(heroActionStartClip);
                isActive = true;
                cursorObject.gameObject.SetActive(true);
                if (lineUpdateCoroutine != null)
                    StopCoroutine(lineUpdateCoroutine);
                lineUpdateCoroutine = UpdateLineCoroutine();
                StartCoroutine(lineUpdateCoroutine);
            }
            if (isActive == true)
            {
                MoveCursor(directionX, directionZ);
            }
        }

        public void MoveCursor(float directionX, float directionZ)
        {
            var forward = globalCamera.Forward();
            var right = globalCamera.Right();
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 move = right * directionX + forward * directionZ;
            move *= (speedCursor);
            cursor.Move(move * Time.deltaTime);
            if ((cursor.transform.position - this.transform.position).magnitude > 12f)
            {
                cursor.transform.position = this.transform.position + (cursor.transform.position - this.transform.position).normalized * 12f;
            }


            if (directionX != 0 || directionZ != 0)
            {
                if(audioSource.isPlaying == false)
                    audioSource.Play();
                CheckIntersections();
                DrawLineRaycast(0, this.transform.position, cursor.transform.position);
            }
            else
            {
                audioSource.Stop();
            }
        }

        public void SetCursor(Vector3 position)
        {
            cursor.transform.position = position;
            DrawLineRaycast(0, this.transform.position, cursor.transform.position);
            if (lineUpdateCoroutine != null)
                StopCoroutine(lineUpdateCoroutine);
            lineUpdateCoroutine = UpdateLineCoroutine();
            StartCoroutine(lineUpdateCoroutine);
        }



        private void CheckIntersections()
        {
            Vector2 playerPos = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 cursorDirection = new Vector2((cursor.transform.position - this.transform.position).x, (cursor.transform.position - this.transform.position).z);
            Vector2 allyPos;
            Vector2 allyDirection;
            int id = 0;
            float intersection = 0f;
            for (int i = 0; i < allyTransform.Count-1; i++)
            {
                for (int j = i+1; j < allyTransform.Count; j++)
                {
                    id += 1;
                    allyPos = new Vector2(allyTransform[i].transform.position.x, allyTransform[i].transform.position.z);
                    allyDirection = new Vector2((allyTransform[j].transform.position - allyTransform[i].transform.position).x, (allyTransform[j].transform.position - allyTransform[i].transform.position).z);
                    intersection = CheckIntersection(playerPos, cursorDirection, allyPos, allyDirection);
                    if (intersection >= 0) // Intersection !!!
                    {
                        allyTransform[i].feedback.StartAfterImage();
                        allyTransform[j].feedback.StartAfterImage();
                        if (intersectionT[id] == -1) // Pas d'antécédant donc faut dessiner la ligne
                        {
                            DrawLineRaycast(id, allyTransform[i].transform.position, allyTransform[j].transform.position);
                            FeedbackLine(0);
                            FeedbackLine(id);
                            feedback.StartAfterImage();
                            doesIntersect = true;
                        }
                        intersectionT[id] = intersection;
                        DrawIntersection(id, new Vector3(cursorDirection.x, 0, cursorDirection.y) * intersection);
                    }
                    else
                    {
                        if (intersectionT[id] != -1) // Autrefois y'avait intersection donc maintenant qu'il n'y en a plus, faut effacer
                        {
                            HideLine(id);
                            HideIntersection(id);
                            intersectionT[id] = -1;
                            allyTransform[i].feedback.EndAfterImage();
                            allyTransform[j].feedback.EndAfterImage();
                            for (int k = 0; k < intersectionT.Count; k++)
                            {
                                if (intersectionT[k] != -1)
                                    return;
                            }
                            doesIntersect = false;
                            feedback.EndAfterImage();
                        }
                    }
                }
            }
        }

        protected float CheckIntersection(Vector2 cursorPosition, Vector2 cursorSegment, Vector2 allyPosition, Vector2 allySegment)
        {
            // https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

            float rs = CrossProduct(cursorSegment, allySegment);
            float t = CrossProduct(new Vector2(allyPosition.x - cursorPosition.x, allyPosition.y - cursorPosition.y), allySegment) / rs;
            float u = CrossProduct(new Vector2(allyPosition.x - cursorPosition.x, allyPosition.y - cursorPosition.y), cursorSegment) / rs;
            if (rs == 0 && u == 0)// Colinéaire (superposé quoi)
            {
                return -1;
            }
            else if (rs == 0 && u != 0) // Parallel 
            {
                return -1;
            }
            else if (rs != 0 && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                return t;
            }
            return -1;
        }


        float CrossProduct(Vector2 v1, Vector2 v2)
        {
            return (v1.x* v2.y) - (v1.y* v2.x);
        }


        // ================================================================================================
        // Draw Intersection

        private void DrawIntersection(int id, Vector3 intersec)
        {
            intersectionAnimList[id].gameObject.SetActive(true);
            if (Physics.Raycast(this.transform.position + intersec, Vector3.down, out hit, 10f, layerMask))
            {
                intersectionAnimList[id].transform.position = hit.point + offset;
            }
            else
            {
                intersectionAnimList[id].transform.position = this.transform.position + intersec - new Vector3(0, this.transform.localPosition.y, 0) + offset;
            }
            intersectionAnimList[id].SetTrigger("Feedback");
        }

        private void HideIntersection(int id)
        {
            intersectionAnimList[id].gameObject.SetActive(false);
        }

        protected virtual void HideAllIntersection()
        {
            for (int i = 0; i < intersectionAnimList.Count; i++)
            {
                HideIntersection(i);
            }
            feedback.EndAfterImage();
            for (int i = 0; i < allyTransform.Count; i++)
            {
                allyTransform[i].feedback.EndAfterImage();
            }
        }
        // ================================================================================================



        // ================================================================================================
        // Draw Line

        private void DrawLineRaycast(int lineID, Vector3 startLine, Vector3 endLine)
        {
            HeroActionLine lineToRender = GetLine(lineID);
            Vector3 direction = endLine - startLine;
            float size = 0;
            int index = 0;
            while (size < (direction.magnitude - spriteOffset))
            {
                size += spriteOffset;
                if (Physics.Raycast(startLine + (direction * size / direction.magnitude), Vector3.down, out hit, 10f, layerMask))
                {
                    AddSpriteToLine(lineToRender, index, hit.point);
                }
                else
                {
                    AddSpriteToLine(lineToRender, index, startLine + (direction * size / direction.magnitude) - new Vector3(0, this.transform.localPosition.y, 0));
                }
                index += 1;
            }
            for (int i = index; i < lineToRender.line.Count; i++)
            {
                lineToRender.line[i].gameObject.SetActive(false);
            }

            // Draw Cursor
            if (Physics.Raycast(endLine, Vector3.down, out hit, 10f, layerMask))
                cursorObject.transform.position = hit.point + offset;
            else
            {
                cursorObject.transform.position = endLine + offset - this.transform.localPosition;
            }
        }

        private void AddSpriteToLine(HeroActionLine lineToRender, int index, Vector3 pos)
        {
            if(lineToRender.line.Count <= index)
            {
                lineToRender.line.Add(Instantiate(spritePrefab, this.transform));
            }
            lineToRender.line[index].gameObject.SetActive(true);
            lineToRender.line[index].transform.position = pos + offset;
        }

        private void HideLine(int lineID)
        {
            HeroActionLine lineToRender = GetLine(lineID);
            for (int i = 0; i < lineToRender.line.Count; i++)
            {
                lineToRender.line[i].gameObject.SetActive(false);
            }
        }

        protected void FeedbackLine(int lineID)
        {
            HeroActionLine lineToRender = GetLine(lineID);
            for (int i = 0; i < lineToRender.line.Count; i++)
            {
                lineToRender.line[i].SetTrigger("LineFeedback");
            }
        }


        protected HeroActionLine GetLine(int id)
        {
            while (id >= heroActionLines.Count)
            {
                heroActionLines.Add(new HeroActionLine(heroActionLines.Count));
            }
            return heroActionLines[id];
        }



        private IEnumerator UpdateLineCoroutine()
        {
            HeroActionLine lineToRender = GetLine(0);
            float t = 0f;
            int feedbackID = 0; 
            while (true)
            {
                t += (Time.deltaTime);
                if (t > 1f)
                    t = 0f;

                for (int i = 0; i < lineToRender.line.Count; i++) 
                {
                    lineToRender.line[i].transform.rotation = globalCamera.Rotation();
                    lineToRender.line[i].SetBool("Intersection", doesIntersect);
                    lineToRender.line[i].ResetTrigger("DirectionFeedback");
                }
                feedbackID = (int) Mathf.Lerp(0f, lineToRender.line.Count, t);
                if (lineToRender.line.Count > 0)
                {
                    //spriteDirection = lineToRender.line[feedbackID].transform;
                    lineToRender.line[feedbackID].SetTrigger("DirectionFeedback");
                }
                UpdateOtherLines();
                yield return null;
            }
        }

        protected virtual void UpdateOtherLines()
        {
            for(int i = 0; i < heroActionLines.Count; i++)
            {
                for (int j = 0; j < heroActionLines[i].line.Count; j++)
                {
                    heroActionLines[i].line[j].transform.rotation = globalCamera.Rotation();
                }
            }
            for (int i = 0; i < intersectionAnimList.Count; i++)
            {
                intersectionAnimList[i].transform.rotation = globalCamera.Rotation();
            }
        }
        // ================================================================================================

        #endregion

    }

} // #PROJECTNAME# namespace