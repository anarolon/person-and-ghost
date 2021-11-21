using UnityEngine;
using System.Linq;
using PersonAndGhost.Utils;

namespace PersonAndGhost
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManagerController : MonoBehaviour
    {
        [Header("Audio fields")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _audioClips;

        public AudioClip[] AudioClips => _audioClips;

        private void Awake()
        {
            _audioSource ??= GetComponent<AudioSource>();

            if (_audioClips == null || _audioClips.Length < 1)
            {
                _audioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
            }

            OrderClips();
        }

        private void OnEnable()
        {
            Actions.OnRequestAudio += HandleAudioRequest;
        }

        private void HandleAudioRequest(Clips clip)
        {
            AudioClip audioClip = _audioClips[(int)clip];

            if (clip == Clips.Move || clip == Clips.BigBoyAction)
            {
                if (!_audioSource.isPlaying)
                {
                    PlayClip(audioClip, false, audioClip.length / 2, 0.75f);
                }
            }

            else
            {
                PlayClip(audioClip, false, 0, 1);
            }
        }

        private void PlayClip
            (AudioClip audioClip, bool isOneShot, float delay, float volume)
        {
            if (isOneShot)
            {
                _audioSource.PlayOneShot(audioClip, volume);
            }

            else
            {
                _audioSource.clip = audioClip;
                _audioSource.volume = volume;
                _audioSource.PlayDelayed(delay);
            }
        }

        // Order audio clips according to enum Clips
        private void OrderClips()
        {
            int clipsLength =
                (int)System.Enum.GetValues(typeof(Clips)).Cast<Clips>().Max() + 1;

            if (_audioClips != null && _audioClips.Length >= clipsLength)
            {
                AudioClip[] result = new AudioClip[_audioClips.Length];

                for (int index = 0; index < clipsLength; index++)
                {
                    foreach (AudioClip clip in _audioClips)
                    {
                        if (clip.name.ToLower()
                            .Contains(((Clips)index).ToString().ToLower()))
                        {
                            result[index] = clip;
                            break;
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
            Actions.OnRequestAudio -= HandleAudioRequest;
        }
    }
}
