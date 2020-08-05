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

    public class AimReticle: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        TypeDictionary weaponData;

        [Title("Parameter")]
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statMagazineNumber;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statAimAccel;

#if UNITY_EDITOR
        private static List<string> GetStatList()
        {

            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;

        }
#endif


        [SerializeField]
        int bonusMagazine = 0;
        [SerializeField]
        float speedMultiplier = 1;


        [Title("Ui")]
        [SerializeField]
        RectTransform reticlePosition;

        Transform targetAim;
        public Transform TargetAim
        {
            get { return targetAim; }
        }

        [SerializeField]
        Image imageFillPrefab;
        [SerializeField]
        Transform imageFillParent;


        [Space]
        [SerializeField]
        RectTransform imageDelimitationPrefab;
        [SerializeField]
        Transform imageDelimitationParent;

        [Title("Feedback")]
        [SerializeField]
        Animator reticleStart;
        [SerializeField]
        Animator reticleFeedback;
        [SerializeField]
        Animator reticleFeedbackFill;
        [SerializeField]
        Animator reticleFeedbackFlash;

        [SerializeField]
        List<BulletDrawer> bulletDrawer = new List<BulletDrawer>();

        [Title("Color")]
        [SerializeField]
        Image[] imageColors;


        [Title("Sound (A dégager de là)")]
        [SerializeField]
        AudioSource aimSound;
        [SerializeField]
        AudioClip chargeSound;

        bool pause = false;

        float maxAmount = 1f;

        List<int> magazineNumber = new List<int>();
        List<float> currentFillMultiplier = new List<float>();

        int indexMainCharacter = 0;
        List<PlayerCharacter> charaAiming = new List<PlayerCharacter>();

        List<Image> imageFill = new List<Image>();
        List<RectTransform> imageDelimitation = new List<RectTransform>();

        //List<RectTransform> listBullets = new List<RectTransform>();
        public delegate void ChargeAction(int mag, PlayerCharacter c);
        public event ChargeAction OnCharge;

        public delegate void StopAction();
        public event StopAction OnStop;

        private IEnumerator fillCoroutine;

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

        public void SetTarget(Transform newTransform)
        {
            targetAim = newTransform;
            if (targetAim == null)
                reticlePosition.anchoredPosition = new Vector2(-9999, -9999);
        }


        private void Update()
        {
            if (targetAim == null)
                return;
            if (Vector3.Dot(globalCamera.Forward(), targetAim.position - globalCamera.Position()) >= 0)
            {
                reticlePosition.anchoredPosition = Vector2.Lerp(reticlePosition.anchoredPosition, globalCamera.WorldToScreenPoint(targetAim.position), 0.5f);
                //reticlePosition.anchoredPosition = globalCamera.WorldToScreenPoint(targetAim.position);
            }
            else
            {
                reticlePosition.anchoredPosition = new Vector2(-9999, -9999);
            }      
        }

        public void SetMainColor(WeaponData weapon)
        {
            Color color = weaponData.GetColorType(weapon.GetWeaponType());
            for (int i = 0; i < imageColors.Length; i++)
                imageColors[i].color = color;
        }


        public void AddCharacterAiming(List<PlayerCharacter> party, int startID)
        {
            //int id = startID;
            for (int i = 0; i < party.Count; i++)
            {
                AddCharacterAiming(party[i]);//id
                //id += 1;
                //if (id >= party.Count)
                //    id = 0;
            }
        }

        public void AddCharacterAiming(PlayerCharacter chara)
        {
            charaAiming.Add(chara);
            if (magazineNumber.Count < charaAiming.Count)
                magazineNumber.Add(0);
            if(currentFillMultiplier.Count < charaAiming.Count)
                currentFillMultiplier.Add(1);
        }

        public void SetIndexMainCharacter(int i)
        {
            indexMainCharacter = i;
            if(OnCharge != null) OnCharge.Invoke(magazineNumber[i], charaAiming[i]);
        }

        public void StartAim()
        {
            aimSound.Play();
            pause = false;
            reticlePosition.gameObject.SetActive(true);
            reticleStart.SetTrigger("Feedback");
            if (fillCoroutine != null)
                StopCoroutine(fillCoroutine);
            fillCoroutine = FillCoroutine();
            StartCoroutine(fillCoroutine);
        }

        private IEnumerator FillCoroutine()
        {
            // Initialize
            for (int i = 0; i < charaAiming.Count; i++)
            {
                currentFillMultiplier[i] = 1f;
                magazineNumber[i] = 0;
            }
            CreateFill();
            // Update (ne s'arrête que si on call StopAim)
            while (true)
            {
                if (pause == true)
                {
                    yield return null;
                    continue;
                }
                for (int i = 0; i < charaAiming.Count; i++)
                {
                    UpdateFill(i);
                }
                yield return null;
            }
        }

        private void CreateFill()
        {
            Vector3 rotation = new Vector3(0, 0, -360f / charaAiming.Count);
            maxAmount = (1f / charaAiming.Count);
            for (int i = 0; i < charaAiming.Count; i++)
            {
                if (i >= imageFill.Count)
                {
                    imageFill.Add(Instantiate(imageFillPrefab, imageFillParent));
                    imageDelimitation.Add(Instantiate(imageDelimitationPrefab, imageDelimitationParent));
                }

                imageFill[i].gameObject.SetActive(true);
                imageFill[i].rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                imageFill[i].rectTransform.Rotate(rotation * i);
                imageFill[i].color = weaponData.GetColorType(charaAiming[i].CharacterEquipement.GetWeaponType());

                imageDelimitation[i].gameObject.SetActive(true);
                imageDelimitation[i].rotation = Quaternion.Euler(0, 0, 0);
                imageDelimitation[i].Rotate(rotation * i);

                bulletDrawer[i].SetBulletType(charaAiming[i].CharacterEquipement.GetWeaponType());
            }
            for(int i = charaAiming.Count; i < imageFill.Count; i++)
            {
                imageFill[i].gameObject.SetActive(false);
                imageDelimitation[i].gameObject.SetActive(false);
            }
            if (charaAiming.Count <= 1)
            {
                // IndexOut of range si on désactive les ennemis à la main
                //imageDelimitation[0].gameObject.SetActive(false);
                reticleFeedback.gameObject.SetActive(true);
            }
            else
            {
                reticleFeedback.gameObject.SetActive(false);
            }
        }

        private void UpdateFill(int index)
        {
            if (magazineNumber[index] < charaAiming[index].CharacterStatController.GetStat(statMagazineNumber) + bonusMagazine)
            {
                float filAmount = charaAiming[index].CharacterStatController.GetAimSpeed(charaAiming[index].transform.position, targetAim.transform.position) * speedMultiplier;
                imageFill[index].fillAmount += ((filAmount * currentFillMultiplier[index]) * maxAmount) * Time.deltaTime;
                if (index == indexMainCharacter)
                {
                    //aimSound.pitch = filAmount;
                    reticleFeedback.SetFloat("Blend", imageFill[index].fillAmount);
                }

                if (imageFill[index].fillAmount >= maxAmount)
                {
                    magazineNumber[index] += 1;
                    imageFill[index].fillAmount = 0;
                    currentFillMultiplier[index] += charaAiming[index].CharacterStatController.GetStat(statAimAccel);
                    if (index == indexMainCharacter)
                    {
                        aimSound.Stop();
                        aimSound.Play();
                        AudioManager.Instance.PlaySound(chargeSound);
                        OnCharge.Invoke(magazineNumber[index], charaAiming[index]);
                        reticleFeedbackFill.SetTrigger("Feedback");
                        reticleFeedbackFlash.SetTrigger("Feedback");
                    }
                    bulletDrawer[index].DrawBullet(magazineNumber[index]);
                }
            }
            else
            {
                if (index == indexMainCharacter)
                {
                    aimSound.Stop();
                }
                imageFill[index].fillAmount = maxAmount;
            }
        }




        public int GetBulletNumber(Character c) // Pas opti du tout
        {
            for (int i = 0; i < charaAiming.Count; i++)
            {
                if(c == charaAiming[i])
                    return magazineNumber[i];
            }
            return 0;
        }

        public void HideAimReticle()
        {
            reticlePosition.gameObject.SetActive(false);
        }
        public void ShowAimReticle()
        {
            reticlePosition.gameObject.SetActive(true);
        }


        public void PauseAim()
        {
            aimSound.Stop();
            pause = true;
        }
        public void ResumeAim()
        {
            aimSound.Play();
            pause = false;
        }

        public void StopAim(PlayerCharacter c)
        {
            ResetAim(c);
            charaAiming.Remove(c);
            if (charaAiming.Count == 0)
            {
                aimSound.Stop();
                if (fillCoroutine != null)
                    StopCoroutine(fillCoroutine);
            }
            else
            {
                CreateFill();
            }
            bulletDrawer[charaAiming.Count].HideBullet();
        }

        public void StopAim()
        {
            if (fillCoroutine != null)
                StopCoroutine(fillCoroutine);
            for (int i = 0; i < charaAiming.Count; i++)
            {
                ResetAim(charaAiming[i]);
            }
            charaAiming.Clear();
            CreateFill(); // Hide fill en fait vu que chara aiming est vide
            aimSound.Stop();
        }

        public void ResetAim(int index)
        {
            OnStop.Invoke();
            pause = false;
            reticlePosition.gameObject.SetActive(true);
            reticleFeedback.SetFloat("Blend", 0);
            magazineNumber[index] = 0;
            currentFillMultiplier[index] = 1;
            imageFill[index].fillAmount = 0;
            bulletDrawer[index].HideBullet();
        }

        public void ResetAim(Character c) // Pas opti du tout
        {
            for (int i = 0; i < charaAiming.Count; i++)
            {
                if (c == charaAiming[i])
                {
                    ResetAim(i);
                }
            }

        }

        /// <summary>
        /// Reset aim for everyone
        /// </summary>
        public void ResetAim()
        {
            for (int i = 0; i < charaAiming.Count; i++)
            {
                ResetAim(i);
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace