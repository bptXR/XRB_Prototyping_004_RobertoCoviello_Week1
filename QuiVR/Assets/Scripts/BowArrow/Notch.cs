using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BowArrow
{
    public class Notch : XRSocketInteractor
    {
        [SerializeField, Range(0, 1)] private float releaseThreshold = 0.25f;
        [SerializeField] private AudioClip[] pullStringSounds;
        [SerializeField] private AudioClip[] releaseStringSounds;
        [SerializeField] private AudioSource audioSource;

        private Bow _bow;

        public PullMeasurer PullMeasurer { get; private set; }

        public bool CanRelease => PullMeasurer.PullAmount > releaseThreshold;

        protected override void Awake()
        {
            base.Awake();
            _bow = GetComponentInParent<Bow>();
            PullMeasurer = GetComponentInChildren<PullMeasurer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PullMeasurer.selectExited.AddListener(ReleaseArrow);
            PullMeasurer.selectEntered.AddListener(PullSounds);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
            PullMeasurer.selectEntered.RemoveListener(PullSounds);
        }

        private void ReleaseArrow(SelectExitEventArgs args)
        {
            if (!hasSelection) return;
            interactionManager.SelectExit(this, firstInteractableSelected);
            ReleaseSounds();
        }

        public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractor(updatePhase);

            if (_bow.isSelected)
                UpdatePull();
        }

        private void UpdatePull()
        {
            attachTransform.position = PullMeasurer.PullPosition;
        }

        private void PullSounds(SelectEnterEventArgs args)
        {
            AudioClip clip = pullStringSounds[Random.Range(0, pullStringSounds.Length)];
            audioSource.PlayOneShot(clip);
        }

        private void ReleaseSounds()
        {
            AudioClip clip = releaseStringSounds[Random.Range(0, releaseStringSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}