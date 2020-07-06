using System.Linq;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Elemento
{
	public class AudioPlayer : MonoBehaviour
	{

		public GameObject audioTonePrefab;
		public static AudioPlayer inst;

		public enum Tone
		{
			A,
			B,
			C
		}

		public Tone tone;
		public Dictionary<Tone, AudioSource> tones = new Dictionary<Tone, AudioSource>();
		public void Start()
		{
			inst = this;
			tones.Add(Tone.A, Instantiate(inst.audioTonePrefab).GetComponent<AudioSource>());
			tones.Add(Tone.B, Instantiate(inst.audioTonePrefab).GetComponent<AudioSource>());
			tones.Add(Tone.C, Instantiate(inst.audioTonePrefab).GetComponent<AudioSource>());
			foreach (var kvp in tones)
			{
				kvp.Value.volume = 0;
			}

		}

		public float pitchA = 0.8f;
		public float pitchB = 1f;
		public float pitchC = 1.2f;

		public void Update()
		{
		
			tones[Tone.A].pitch = pitchA;
			tones[Tone.B].pitch = pitchB;
			tones[Tone.C].pitch = pitchC;
		}

		public static void PlayTone(int hz)
		{
			//aud.Play();
			// https://synth.playtronica.com/
		}

	}
}
