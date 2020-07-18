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
    public class AfterImageEffect : MonoBehaviour
    {
        [SerializeField]
        bool active = true;
        [SerializeField]
        SpriteRenderer image;
        [SerializeField]
        int afterImageNumber = 4;
        [SerializeField]
        float afterImageTime = 0.5f;
        [SerializeField]
        SpriteRenderer afterImagePrefab;


        Color colorDisappear;
        float t = 0f;
        int index = 0;
        List<SpriteRenderer> afterImage = new List<SpriteRenderer>();

        private void Start()
        {
            if(active == true)
                StartAfterImage();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAfterImages();
            if (active == false)
                return;
            t += Time.deltaTime;
            if(t >= afterImageTime)
            {
                afterImage[index].transform.position = image.transform.position;
                afterImage[index].transform.localScale = image.transform.localScale;
                afterImage[index].flipX = image.flipX;
                afterImage[index].sprite = image.sprite;
                afterImage[index].color = image.color;
                index += 1;
                if (index >= afterImage.Count)
                    index = 0;
                t = 0f;
            }
        }

        public void UpdateAfterImages()
        {
            for (int i = 0; i < afterImage.Count; i++)
            {
                afterImage[i].color -= colorDisappear;;
            }
        }

        public void StartAfterImage()
        {
            colorDisappear = new Color(0, 0, 0, 1 / ((afterImageNumber * afterImageTime) * 60));
            active = true;
            t = 0f;
            for(int i = 0; i < afterImageNumber; i++)
            {
                if(i >= afterImage.Count)
                {
                    afterImage.Add(Instantiate(afterImagePrefab));
                }
                afterImage[i].gameObject.SetActive(true);
            }
            for (int i = afterImageNumber; i < afterImage.Count; i++)
            {
                afterImage[i].gameObject.SetActive(false);
            }
        }
        public void StartAfterImage(SpriteRenderer imageToCopy)
        {
            image = imageToCopy;
            StartAfterImage();
        }

        public void EndAfterImage()
        {
            active = false;
        }
    }
}
