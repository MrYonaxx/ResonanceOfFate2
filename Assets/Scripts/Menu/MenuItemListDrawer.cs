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
        [SerializeField]
        protected ItemDatabase itemDatabase;

        [Title("UI")]
        [SerializeField]
        protected RectTransform listTransform;
        [SerializeField]
        protected RectTransform selectionTransform;

        [SerializeField]
        protected ItemButton prefabItem;


        [SerializeField]
        protected List<ItemButton> listItem = new List<ItemButton>();

        protected int indexSelection = 0;
        protected int listIndexCount = 0;
        protected bool padDown = false;


        public event ItemAction OnSelect;
        public event ItemAction OnValidate;
        public event Action OnQuit;


        [Title("Input")]
        [SerializeField]
        protected int timeBeforeRepeat = 10;
        [SerializeField]
        protected int repeatInterval = 3;
        [SerializeField]
        protected int scrollSize = 3;

        protected int currentTimeBeforeRepeat = -1;
        protected int currentRepeatInterval = -1;
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
            indexLimit = scrollSize;
            listIndexCount = listItem.Count;
        }

        public void DrawList(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i <= listItem.Count)
                    listItem.Add(Instantiate(prefabItem, listTransform));
                ItemData item = itemDatabase.GetItemData(items[i]); // C'est très lent, peut etre faire un cache ou un truc
                listItem[i].DrawItem(item);
                listItem[i].gameObject.SetActive(true);
            }
            listIndexCount = items.Count;
            for (int i = items.Count; i < listItem.Count; i++)
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

            if (Input.GetButtonDown(control.buttonA) == true)
                Validate();
            else if (Input.GetButtonDown(control.buttonB) == true)
                Quit();

            else if (Input.GetAxis(control.dpadVertical) > 0.8)
            {
                //padDown = true;
                SelectUp();
            }
            else if (Input.GetAxis(control.dpadVertical) < -0.8)
            {
                //padDown = true;
                SelectDown();
            }
            else if (Input.GetAxis(control.dpadVertical) == 0)
            {
                StopRepeat();
                //padDown = false;
            }

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
            if(OnSelect != null) OnSelect.Invoke(listItem[indexSelection].ItemData);
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
            if (OnSelect != null) OnSelect.Invoke(listItem[indexSelection].ItemData);
            MoveScrollRect();
        }



        public virtual void Validate()
        {
            if (listIndexCount == 0)
                return;
            if (OnValidate != null) OnValidate.Invoke(listItem[indexSelection].ItemData);
        }

        public virtual void Quit()
        {
            if (OnQuit != null) OnQuit.Invoke();
        }







        // Check si on peut repeter l'input
        protected bool CheckRepeat()
        {
            if (currentRepeatInterval == -1)
            {
                if (currentTimeBeforeRepeat == -1)
                {
                    currentTimeBeforeRepeat = timeBeforeRepeat;
                    return true;
                }
                else if (currentTimeBeforeRepeat == 0)
                {
                    currentRepeatInterval = repeatInterval;
                }
                else
                {
                    currentTimeBeforeRepeat -= 1;
                }
            }
            else if (currentRepeatInterval == 0)
            {
                currentRepeatInterval = repeatInterval;
                return true;
            }
            else
            {
                currentRepeatInterval -= 1;
            }
            return false;
        }

        public void StopRepeat()
        {
            currentRepeatInterval = -1;
            currentTimeBeforeRepeat = -1;
        }

        protected void MoveScrollRect()
        {
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