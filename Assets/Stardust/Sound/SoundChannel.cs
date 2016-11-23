namespace Stardust
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A single audio source for sound playing, which also contains a sound queue.
    /// </summary>
    public class SoundChannel
    {
        private Queue<SoundPlayInfo> m_queue = new Queue<SoundPlayInfo>();

        private float volume;
        private float targetVolume;
        private float initTargetVolume;
        private float tempFadeSeconds;
        private float fadeInterpolater;
        private float onFadeStartVolume;

        /// <summary>
        /// The ID of the Audio.
        /// </summary>
        public int audioID { get { return CurrentSound != null ? CurrentSound.AudioId : -1; } }

        /// <summary>
        /// Priority of the audio clip.
        /// </summary>
        public int Priority { get { return CurrentSound != null ? CurrentSound.Priority : 0; } }

        /// <summary>
        /// Name of the clip
        /// </summary>
        public string Name { get { return CurrentSound != null ? CurrentSound.Name : null; } }

        public SoundPlayInfo CurrentSound { get; private set; }

        /// <summary>
        /// The audio source that is responsible for this audio
        /// </summary>
        public AudioSource audioSource { get; private set; }

        /// <summary>
        /// Whether the audio persists in between scene changes
        /// </summary>
        public bool Persist { get { return CurrentSound != null ? CurrentSound.Persist : false; } }

        /// <summary>
        /// Whether the current audio can be interrupted.
        /// </summary>
        public bool Interruptable { get { return CurrentSound != null ? CurrentSound.Interruptable : true; } }

        /// <summary>
        /// How many seconds it needs for the audio to fade in/ reach target volume (if higher than current)
        /// </summary>
        public float fadeInSeconds { get { return CurrentSound != null ? CurrentSound.FadeInSeconds : 0f; } }

        /// <summary>
        /// How many seconds it needs for the audio to fade out/ reach target volume (if lower than current)
        /// </summary>
        public float fadeOutSeconds { get { return CurrentSound != null ? CurrentSound.FadeOutSeconds : 0f; } }

        /// <summary>
        /// Whether the audio is currently playing
        /// </summary>
        public bool Playing { get; private set; }

        /// <summary>
        /// Whether the audio is paused
        /// </summary>
        public bool Paused { get; private set; }

        /// <summary>
        /// Whether the audio is stopping
        /// </summary>
        public bool Stopping { get; private set; }

        public int TimeStartedPlaying { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return CurrentSound == null;
            }
        }

        public SoundChannel(AudioSource source)
        {
            audioSource = source;
            Clear();
        }

        public bool IsSoundQueued(int audioId)
        {
            if (m_queue.Count == 0)
            {
                return false;
            }
            foreach (var item in m_queue)
            {
                if (item.AudioId == audioId)
                {
                    return true;
                }
            }
            return false;
        }


        public void UpdatePitch(float pitch)
        {
            if (this.CurrentSound != null)
            {
                this.CurrentSound.Pitch = pitch;
                audioSource.pitch = pitch;
            }
        }

        public void Clear()
        {
            this.targetVolume = volume;
            this.initTargetVolume = volume;
            this.tempFadeSeconds = -1;
            this.volume = 0f;
            this.TimeStartedPlaying = 0;

            this.Playing = false;
            this.Paused = false;

            this.CurrentSound = null;
        }

        public bool PlayClip(SoundPlayInfo info)
        {
            bool enqueue = false;
            if (info.Mode == SoundPlayMode.Queued)
            {
                if (Playing || Stopping)
                {
                    enqueue = true;
                }
            }
            else
            {
                if (CurrentSound != null && CurrentSound.NeedFadeOut)
                {
                    enqueue = true;
                    // Lets the current sound fades out.
                    Stop();
                }
            }
            if (enqueue)
            {
                info.Mode = SoundPlayMode.Instant;
                m_queue.Enqueue(info);
                return true;
            }

            if (BindClip(info))
            {
                Play();
                return true;
            }
            else
            {
                return PlayNextInQueue();
            }
        }

        /// <summary>
        /// Start playing audio clip from the beggining
        /// </summary>
        public void Play()
        {
            Play(initTargetVolume);
        }

        /// <summary>
        /// Start playing audio clip from the beggining
        /// </summary>
        /// <param name="volume">The target volume</param>
        public void Play(float volume)
        {
            audioSource.Play();
            TimeStartedPlaying = DateTimeUtility.GetCurrentUnixTimestamp();
            Playing = true;

            fadeInterpolater = 0f;
            onFadeStartVolume = this.volume;
            targetVolume = volume;
        }

        /// <summary>
        /// Stop playing audio clip
        /// </summary>
        public void Stop()
        {
            if (CurrentSound == null)
            {
                return;
            }
            if (CurrentSound.NeedFadeOut)
            {
                fadeInterpolater = 0f;
                onFadeStartVolume = volume;
                targetVolume = 0f;

                Stopping = true;
            }
            else
            {
                audioSource.Stop();
                Playing = false;
                Clear();
            }
        }

        /// <summary>
        /// Pause playing audio clip
        /// </summary>
        public void Pause()
        {
            if (CurrentSound == null)
            {
                return;
            }
            audioSource.Pause();
            Paused = true;
        }

        /// <summary>
        /// Resume playing audio clip
        /// </summary>
        public void UnPause()
        {
            if (CurrentSound == null)
            {
                return;
            }
            audioSource.UnPause();
            Paused = false;
        }

        /// <summary>
        /// Resume playing audio clip
        /// </summary>
        public void Resume()
        {
            if (CurrentSound == null)
            {
                return;
            }
            audioSource.UnPause();
            Paused = false;
        }

        /// <summary>
        /// Sets the audio volume
        /// </summary>
        /// <param name="volume">The target volume</param>
        public void SetVolume(float volume)
        {
            if (volume > targetVolume)
            {
                SetVolume(volume, fadeOutSeconds);
            }
            else
            {
                SetVolume(volume, fadeInSeconds);
            }
        }

        /// <summary>
        /// Sets the audio volume
        /// </summary>
        /// <param name="volume">The target volume</param>
        /// <param name="fadeSeconds">How many seconds it needs for the audio to fade in/out to reach target volume. If passed, it will override the Audio's fade in/out seconds, but only for this transition</param>
        public void SetVolume(float volume, float fadeSeconds)
        {
            SetVolume(volume, fadeSeconds, this.volume);
        }

        /// <summary>
        /// Sets the audio volume
        /// </summary>
        /// <param name="volume">The target volume</param>
        /// <param name="fadeSeconds">How many seconds it needs for the audio to fade in/out to reach target volume. If passed, it will override the Audio's fade in/out seconds, but only for this transition</param>
        /// <param name="startVolume">Immediately set the volume to this value before beginning the fade. If not passed, the Audio will start fading from the current volume towards the target volume</param>
        public void SetVolume(float volume, float fadeSeconds, float startVolume)
        {
            targetVolume = Mathf.Clamp01(volume);
            fadeInterpolater = 0;
            onFadeStartVolume = startVolume;
            tempFadeSeconds = fadeSeconds;
        }

        /// <summary>
        /// Sets the Audio 3D max distance
        /// </summary>
        /// <param name="max">the max distance</param>
        public void Set3DMaxDistance(float max)
        {
            audioSource.maxDistance = max;
        }

        /// <summary>
        /// Sets the Audio 3D min distance
        /// </summary>
        /// <param name="max">the min distance</param>
        public void Set3DMinDistance(float min)
        {
            audioSource.minDistance = min;
        }

        /// <summary>
        /// Sets the Audio 3D distances
        /// </summary>
        /// <param name="min">the min distance</param>
        /// <param name="max">the max distance</param>
        public void Set3DDistances(float min, float max)
        {
            Set3DMinDistance(min);
            Set3DMaxDistance(max);
        }

        public void Update()
        {
            if (volume != targetVolume)
            {
                float fadeValue;
                fadeInterpolater += Time.deltaTime;
                if (volume > targetVolume)
                {
                    fadeValue = tempFadeSeconds != -1 ? tempFadeSeconds : fadeOutSeconds;
                }
                else
                {
                    fadeValue = tempFadeSeconds != -1 ? tempFadeSeconds : fadeInSeconds;
                }

                volume = Mathf.Lerp(onFadeStartVolume, targetVolume, fadeInterpolater / fadeValue);
            }
            else if (tempFadeSeconds != -1)
            {
                tempFadeSeconds = -1;
            }

            if (volume == 0f && Stopping)
            {
                Clear();

                if (!PlayNextInQueue())
                {
                    audioSource.Stop();
                }
            }

            // Update playing status
            if (audioSource.isPlaying != Playing)
            {
                Playing = audioSource.isPlaying;
                if (!Paused && !Playing)
                {
                    Clear();

                    PlayNextInQueue();
                }
            }
        }

        private bool PlayNextInQueue()
        {
            if (m_queue.Count > 0)
            {
                return PlayClip(m_queue.Dequeue());
            }
            return false;
        }

        private bool BindClip(SoundPlayInfo info)
        {
            AudioClip clip = info.Clip;
            if (clip == null)
            {
                return false;
            }

            this.CurrentSound = info;

            float dis = (audioSource.transform.position - info.SourcePosition).magnitude;
            audioSource.clip = clip;
            audioSource.loop = info.Loop;
            audioSource.spread = 360;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = info.MinDistance;
            audioSource.maxDistance = info.MaxDistance;

            float volumeFactor = info.Volume >= 0 && info.Volume <= 1 ? info.Volume : 1;
            initTargetVolume = Mathf.Max(0, 1 - (dis / audioSource.maxDistance)) * volumeFactor;
            audioSource.pitch = info.Pitch;

            return true;
        }
    } 
}