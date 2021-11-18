using PersonAndGhost.Person;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PersonAndGhost
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManagerController : MonoBehaviour
    {
        [Header("Person field")]
        [SerializeField] private PersonMovement _personController;

        [Header("Audio fields")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _audioClips;

        // Order In Which Clips will be
        private enum Clips
        {
            Move,
            Cling,
            Jump,
            Meditation,
            Death
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private IEnumerator Start()
        {
            if (_audioClips == null || _audioClips.Length < 1)
            {
                _audioClips = Resources.FindObjectsOfTypeAll<AudioClip>();
            }

            yield return new WaitForFixedUpdate();

            OrderClips(); 

            _personController = FindObjectOfType<PersonMovement>();
        }

        private void Update()
        {
            AudioClip audioClip = SelectAudioToPlay();

            if (audioClip != null)
            {
                _audioSource.clip = audioClip;
                _audioSource.PlayDelayed(audioClip.length / 2);
            }
        }

        private AudioClip SelectAudioToPlay()
        {
            if (!_audioSource.isPlaying)
            {
                if (_personController)
                {
                    switch (_personController.MovementSM.CurrentState.StateId())
                    {
                        case "MovementState":
                            return _audioClips[(int)Clips.Move];
                        case "ClingState":
                            return _audioClips[(int)Clips.Cling];
                        default:
                            if (_personController.Jumped)
                            {
                                return _audioClips[(int)Clips.Jump];
                            }

                            else if (_personController.IsMeditating)
                            {
                                return _audioClips[(int)Clips.Meditation];
                            }

                            else if (_personController.isDead)
                            {
                                return _audioClips[(int)Clips.Death];
                            }
                            break;
                    }
                }
            }
            return null;
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
    }
}
