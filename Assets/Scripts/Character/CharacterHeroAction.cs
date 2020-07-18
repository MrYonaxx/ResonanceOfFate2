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
        CharacterController cursor;

        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        float speedCursor = 2;
        [SerializeField]
        List<CharacterHeroAction> allyTransform = new List<CharacterHeroAction>(); // à renomme mais je refactorai le truc quand y'aura 4 persos dans la team

        [Header("Draw")]
        [SerializeField]
        Animator spritePrefab;
        [SerializeField]
        Animator intersectionPrefab;
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
        List<Animator> intersectionList = new List<Animator>();
        bool isActive = false;
        bool intersection = false;
        public bool Intersection
        {
            get { return intersection; }
        }

        private float intersectionT;
        public float IntersectionT
        {
            get { return intersectionT; }
        }

        // debug
        Vector3 offset = new Vector3(0, 0.2f, 0);
        private IEnumerator lineUpdateCoroutine;

        AudioSource audioSource;

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
            return cursor.transform.position - this.transform.localPosition;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Activate()
        {

        }

        public void Desactivate()
        {
            audioSource.Stop();
            isActive = false;
            intersection = false;
            cursor.transform.localPosition = Vector3.zero;
            cursor.gameObject.SetActive(false);
            HideIntersection();
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
                cursor.gameObject.SetActive(true);
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
            cursor.Move(new Vector3(0, -200 * Time.deltaTime, 0));

            if (directionX != 0 || directionZ != 0)
            {
                if(audioSource.isPlaying == false)
                    audioSource.Play();
                if (CheckIntersection(new Vector2(this.transform.position.x, this.transform.position.z),
                                 new Vector2((cursor.transform.position - this.transform.position).x, (cursor.transform.position - this.transform.position).z),
                                 new Vector2(allyTransform[0].transform.position.x, allyTransform[0].transform.position.z),
                                 new Vector2((allyTransform[1].transform.position - allyTransform[0].transform.position).x, (allyTransform[1].transform.position - allyTransform[0].transform.position).z)))
                {
                    DrawLine(1, allyTransform[0].transform.position + offset, allyTransform[1].transform.position + offset);
                    if (intersection == false)
                    {
                        FeedbackLine(1);
                        FeedbackLine(0);
                        intersection = true;
                    }
                }
                else
                {
                    if (intersection == true)
                    {
                        intersection = false;
                        HideLine(1);
                        HideIntersection();
                    }
                }
                DrawLine(0, this.transform.position, cursor.transform.position);
            }
            else
            {
                audioSource.Stop();
            }
        }

        public void SetCursor(Vector3 position)
        {
            cursor.transform.position = position + this.transform.localPosition;
            DrawLine(0, this.transform.position, cursor.transform.position);
            if (lineUpdateCoroutine != null)
                StopCoroutine(lineUpdateCoroutine);
            lineUpdateCoroutine = UpdateLineCoroutine();
            StartCoroutine(lineUpdateCoroutine);
        }



        private bool CheckIntersection(Vector2 cursorPosition, Vector2 cursorSegment, Vector2 allyPosition, Vector2 allySegment)
        {
            // https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

            float rs = CrossProduct(cursorSegment, allySegment);
            float t = CrossProduct(new Vector2(allyPosition.x - cursorPosition.x, allyPosition.y - cursorPosition.y), allySegment) / rs;
            float u = CrossProduct(new Vector2(allyPosition.x - cursorPosition.x, allyPosition.y - cursorPosition.y), cursorSegment) / rs;
            if (rs == 0 && u == 0)// Colinéaire (superposé quoi)
            {
                Debug.Log("Quoi ?");
                return false;
            }
            else if (rs == 0 && u != 0) // Parallel 
            {
                return false;
            }
            else if (rs != 0 && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                intersectionT = t;
                DrawIntersection((cursorSegment) * t);
                return true;
            }
            return false;
        }


        float CrossProduct(Vector2 v1, Vector2 v2)
        {
            return (v1.x* v2.y) - (v1.y* v2.x);
        }


        private void DrawIntersection(Vector2 intersec)
        {
            intersectionPrefab.gameObject.SetActive(true);
            intersectionPrefab.transform.localPosition = new Vector3(intersec.x, 0, intersec.y);
            intersectionPrefab.transform.rotation = globalCamera.Rotation();
            //intersectionPrefab.transform.localPosition = new Vector3(intersectionPrefab.transform.localPosition.x, 0, intersectionPrefab.transform.localPosition.z);
            intersectionPrefab.SetTrigger("Feedback");
            feedback.StartAfterImage();
            for(int i = 0; i < allyTransform.Count;i++)
            {
                allyTransform[i].feedback.StartAfterImage();
            }
        }
        private void HideIntersection()
        {
            intersectionPrefab.gameObject.SetActive(false);
            feedback.EndAfterImage();
            for (int i = 0; i < allyTransform.Count; i++)
            {
                allyTransform[i].feedback.EndAfterImage();
            }
        }




        // Line Draw
        private void DrawLine(int lineID, Vector3 startLine, Vector3 endLine)
        {
            HeroActionLine lineToRender = GetLine(lineID);
            Vector3 direction = endLine - startLine;
            float size = 0;
            int index = 0;
            while(size < (direction.magnitude - spriteOffset))
            {
                size += spriteOffset;
                AddSpriteToLine(lineToRender, index, startLine + (direction * size / direction.magnitude));
                index += 1;
            }
            for(int i = index; i < lineToRender.line.Count; i++)
            {
                lineToRender.line[i].gameObject.SetActive(false);
            }
        }

        private void AddSpriteToLine(HeroActionLine lineToRender, int index, Vector3 pos)
        {
            if(lineToRender.line.Count <= index)
            {
                lineToRender.line.Add(Instantiate(spritePrefab, this.transform));
            }
            lineToRender.line[index].gameObject.SetActive(true);
            lineToRender.line[index].transform.position = pos;
        }

        private void HideLine(int lineID)
        {
            HeroActionLine lineToRender = GetLine(lineID);
            for (int i = 0; i < lineToRender.line.Count; i++)
            {
                lineToRender.line[i].gameObject.SetActive(false);
            }
        }

        private void FeedbackLine(int lineID)
        {
            HeroActionLine lineToRender = GetLine(lineID);
            for (int i = 0; i < lineToRender.line.Count; i++)
            {
                lineToRender.line[i].SetTrigger("LineFeedback");
            }
        }


        private HeroActionLine GetLine(int id)
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
                    lineToRender.line[i].SetBool("Intersection", intersection);
                    lineToRender.line[i].ResetTrigger("DirectionFeedback");
                }
                feedbackID = (int) Mathf.Lerp(0f, lineToRender.line.Count, t);
                if (lineToRender.line.Count > 0)
                {
                    //spriteDirection = lineToRender.line[feedbackID].transform;
                    lineToRender.line[feedbackID].SetTrigger("DirectionFeedback");
                }
                yield return null;
            }
        }


        #endregion

    } 

} // #PROJECTNAME# namespace