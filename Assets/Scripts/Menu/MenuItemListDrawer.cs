/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public delegate void ItemAction(ItemData item);
    public class MenuItemListDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Control")]
        [SerializeField]
        protected ControllerConfigurationData control;

        [Title("UI")]
        [SerializeField]
        protected RectTransform listTransform;
        [SerializeField]
        protected RectTransform selectionTransform;

        [SerializeField]
        protected ItemButton prefabItem;


        [SerializeField]
        protected List<ItemButton> listItem = new List<ItemButton>();
        public List<ItemButton> ListItem
        {
            get { return listItem; }
        }

        protected int indexSelection = 0;
        public int IndexSelection
        {
            get { return indexSelection; }
        }

        protected int listIndexCount = 0;
        protected bool padDown = false;


        public event ActionInt OnSelect;
        public event ActionInt OnValidate;
        public event Action OnQuit;


        [Title("Input")]
        [SerializeField]
        protected int timeBeforeRepeat = 10;
        [SerializeField]
        protected int repeatInterval = 3;
        [SerializeField]
        protected int scrollSize = 3;

        [Title("Sound")]
        [SerializeField]
        protected AudioClip validateClip;
        [SerializeField]
        protected AudioClip selectClip;
        [SerializeField]
        protected AudioClip cancelClip;

        protected float currentTimeBeforeRepeat = -1;
        protected float currentRepeatInterval = -1;
        protected int lastDirection = 0; // 2 c'est bas, 8 c'est haut (voir numpad)
        protected int indexLimit = 0;

        protected IEnumerator coroutineScroll = null;

        protected bool canInput = false;


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
        protected virtual void Start()
        {
            canInput = false;
            indexLimit = scrollSize;
            listIndexCount = listItem.Count;
        }

        public void DrawItemList(int i, Sprite icon, string text)
        {
            if (i >= listItem.Count)
                listItem.Add(Instantiate(prefabItem, listTransform));
            listItem[i].DrawText(icon, text);
            listItem[i].gameObject.SetActive(true);
        }

        public void SetItemCount(int count)
        {
            listIndexCount = count;
            for (int i = count; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }
        }

        public void SetInput(bool b)
        {
            canInput = b;
        }

        protected virtual void Update()
        {
            if (canInput == false)
                return;
            InputList();
        }

        public bool InputList()
        {
            if (Input.GetButtonDown(control.buttonA) == true)
            {
                Validate();
                return true;
            }
            else if (Input.GetButtonDown(control.buttonB) == true)
            {
                Quit();
                return true;
            }
            else if (Input.GetAxis(control.dpadVertical) > 0.8)
            {
                SelectUp();
                return true;
            }
            else if (Input.GetAxis(control.dpadVertical) < -0.8)
            {
                SelectDown();
                return true;
            }
            else if (Input.GetAxis(control.dpadVertical) == 0)
            {
                StopRepeat();
                return false;
            }
            return false;
        }



        public void SelectUp()
        {
            if (listIndexCount == 0)
            {
                return;
            }
            if (lastDirection != 8)
            {
                StopRepeat();
                lastDirection = 8;
            }

            if (CheckRepeat() == false)
                return;

            indexSelection -= 1;
            if (indexSelection <= -1)
            {
                indexSelection = listIndexCount - 1;
            }
            if(OnSelect != null) OnSelect.Invoke(indexSelection);
            MoveScrollRect();
        }

        public void SelectDown()
        {
            if (listIndexCount == 0)
            {
                return;
            }
            if (lastDirection != 2)
            {
                StopRepeat();
                lastDirection = 2;
            }
            if (CheckRepeat() == false)
                return;

            indexSelection += 1;
            if (indexSelection >= listIndexCount)
            {
                indexSelection = 0;
            }

            if (OnSelect != null) OnSelect.Invoke(indexSelection);
            MoveScrollRect();
        }



        public virtual void Validate()
        {
            if (listIndexCount == 0)
                return;
            AudioManager.Instance.PlaySound(validateClip);
            if (OnValidate != null) OnValidate.Invoke(indexSelection);
        }

        public virtual void Quit()
        {
            AudioManager.Instance.PlaySound(cancelClip);
            if (OnQuit != null) OnQuit.Invoke();
        }







        // Check si on peut repeter l'input
        protected bool CheckRepeat()
        {
            if (currentRepeatInterval == -100)
            {
                if (currentTimeBeforeRepeat == -100)
                {
                    currentTimeBeforeRepeat = timeBeforeRepeat * 0.016f; // (0.016f = 60 fps et opti de la division)
                    return true;
                }
                else if (currentTimeBeforeRepeat <= 0)
                {
                    currentRepeatInterval = repeatInterval * 0.016f;// (0.016f = 60 fps et opti de la division)
                }
                else
                {
                    currentTimeBeforeRepeat -= Time.deltaTime;
                }
            }
            else if (currentRepeatInterval <= 0)
            {
                currentRepeatInterval = repeatInterval * 0.016f;// (0.016f = 60 fps et opti de la division)
                return true;
            }
            else
            {
                currentRepeatInterval -= Time.deltaTime;
            }
            return false;
        }

        public void StopRepeat()
        {
            currentRepeatInterval = -100;
            currentTimeBeforeRepeat = -100;
        }

        protected void MoveScrollRect()
        {
            AudioManager.Instance.PlaySound(selectClip);
            if (listTransform == null)
            {
                if (selectionTransform != null)
                    selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
                return;
            }
            if (indexSelection > indexLimit)
            {
                indexLimit = indexSelection;
                coroutineScroll = MoveScrollRectCoroutine();
                if (coroutineScroll != null)
                {
                    StopCoroutine(coroutineScroll);
                }
                StartCoroutine(coroutineScroll);
            }
            else if (indexSelection < indexLimit - scrollSize + 1)
            {
                indexLimit = indexSelection + scrollSize - 1;
                coroutineScroll = MoveScrollRectCoroutine();
                if (coroutineScroll != null)
                {
                    StopCoroutine(coroutineScroll);
                }
                StartCoroutine(coroutineScroll);
            }
            else
            {
                if (selectionTransform != null)
                    selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
            }

        }

        private IEnumerator MoveScrollRectCoroutine()
        {
            float t = 0f;
            float time = 0.1f;
            int ratio = indexLimit - scrollSize;
            Vector2 destination = new Vector2(0, ratio * prefabItem.RectTransform.sizeDelta.y);
            while (t < 1f)
            {
                t += Time.deltaTime / time;
                listTransform.anchoredPosition = Vector2.Lerp(listTransform.anchoredPosition, destination, t);
                selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
                yield return null;
            }
            selectionTransform.anchoredPosition = listItem[indexSelection].RectTransform.anchoredPosition;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace