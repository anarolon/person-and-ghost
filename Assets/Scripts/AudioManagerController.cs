using PersonAndGhost.Person;
using PersonAndGhost.Utils;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PersonAndGhost
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManagerController : MonoBehaviour
    {
        [Header("Audio fields")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _audioClips;

        private int ClipsEnumLength =>
                (int)System.Enum.GetValues(typeof(Clips)).Cast<Clips>().Max() + 1;

        // Order In Which Clips will be
        private enum Clips
        {
            Move,
            Cling,
            Jump,
            Meditati,
            Death
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (_audioClips == null || _audioClips.Length < 1)
            {
                _audioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
            }

            OrderClips();
        }

        private void OnEnable()
        {
            Actions.OnPersonRequestAudio += HandlePersonRequestAudio;
        }

        private void HandlePersonRequestAudio(string state)
        {
            for (int index = 0; index < ClipsEnumLength; index++)
            {
                if (state.ToLower().Contains(((Clips)index).ToString().ToLower()))
                {
                    AudioClip clip = _audioClips[index];

                    if (index == (int)Clips.Move)
                    {
                        if (!_audioSource.isPlaying)
                        {
                            PlayClip(clip, false, clip.length / 2, 0.75f, false);
                        }
                    }

                    else
                    {
                        PlayClip(clip, false, 0, 1, false);
                    }
                    break;
                }
            }
        }

        private void PlayClip(AudioClip audioClip, 
            bool isOneShot, float delay, float volume, bool isLoop)
        {
            if (isOneShot)
            {
                _audioSource.PlayOneShot(audioClip, volume);
            }

            else
            {
                _audioSource.clip = audioClip;
                _audioSource.volume = volume;
                _audioSource.loop = isLoop;
                _audioSource.PlayDelayed(delay);
            }
        }

        // Order audio clips according to enum Clips
        private void OrderClips()
        {
            int clipsLength = ClipsEnumLength;

            if (_audioClips != null && _audioClips.Length >= clipsLength)
            {
                AudioClip[] result = new AudioClip[_audioClips.Length];

                for (int index = 0; index < clipsLength; index++)
                {
                    foreach (AudioClip clip in _audioClips)
                    {
                        if (clip.name.ToLower().Contains(
                            ((Clips)index).ToString().ToLower()))
                        {
                            result[index] = clip;
                        }
                    }
                }

                _audioClips = result;
            }

            else
            {
                Debug.LogWarning("Audio clips array is null " +
                    "or its length is smaller than required.");
            }
        }

        private void OnDisable()
        {
            Actions.OnPersonRequestAudio -= HandlePersonRequestAudio;
        }
    }
}
