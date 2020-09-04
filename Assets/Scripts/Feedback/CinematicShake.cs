using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VoiceActing
{
    public class CinematicShake : MonoBehaviour
    {
        [SerializeField]
        float power = 0.1f;

        private void Update()
        {
            this.transform.localPosition = new Vector3(Random.Range(-power, power), Random.Range(-power, power));
        }
    }
}
