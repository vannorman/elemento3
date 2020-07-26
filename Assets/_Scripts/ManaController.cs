using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Elemento.Spells;
using Random = UnityEngine.Random;

namespace Elemento
{ 

	//using Unity.UI;
	public class ManaController : MonoBehaviour
	{

		public Image manaImage;
		public float manaCount = 100;
        public float maxMana = 100;
        AudioSource aud; 
        public AudioClip[] notEnoughMana;
        public AudioClip manaPickup;

        public static ManaController Instance { get; private set; }

        private void Start()
        {
			Instance = this;
            aud = this.gameObject.AddComponent<AudioSource>();
            aud.loop = false;
            aud.spatialBlend = 0;
            UpdateManaGFX();
        }

        internal void PickupMana(int manaAmount)
        {
            manaCount += manaAmount;
            if (manaCount > maxMana) manaCount = maxMana;
            aud.clip = manaPickup;
            aud.Play();
            UpdateManaGFX();
        }

        private void UpdateManaGFX()
        {
            manaImage.fillAmount = Instance.manaCount / Instance.maxMana;
        }

        public void TryCastSpell(Spell s)
		{ 
			
		}

        internal static bool UseMana(float manaRequired)
        {
            if (Instance.manaCount >= manaRequired)
            {
                Instance.manaCount -= manaRequired;
                Instance.UpdateManaGFX();
                return true;
            }
            else
            {
                Instance.aud.clip = Instance.notEnoughMana[Random.Range(0, Instance.notEnoughMana.Length)];
                Instance.aud.Play();
                return false;
            }
        }
    }
}
