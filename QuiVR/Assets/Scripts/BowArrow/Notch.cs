using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BowArrow
{
    public class Notch : XRSocketInteractor
    {
        [SerializeField, Range(0, 1)] private float releaseThreshold = 0.25f;

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
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PullMeasurer.selectExited.RemoveListener(ReleaseArrow);
        }

        private void ReleaseArrow(SelectExitEventArgs args)
        {
            if (!hasSelection) return;
            interactionManager.SelectExit(this, firstInteractableSelected);
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
    }
}