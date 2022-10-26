using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BowArrow
{
    public class PullMeasurer : XRBaseInteractable
    {
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private Material glowMaterial;

        public float PullAmount { get; private set; } = 0.0f;
        public float lastPullAmount;
        private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");

        public Vector3 PullPosition => Vector3.Lerp(start.position, end.position, PullAmount);

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            lastPullAmount = PullAmount;
            PullAmount = 0;
            glowMaterial.SetFloat(EmissionIntensity, 0);
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (!isSelected) return;
            
            if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic) return;
            GetPullAmount();
            glowMaterial.SetFloat(EmissionIntensity, (PullAmount*5));
        }

        private void GetPullAmount()
        {
            Vector3 handPosition = firstInteractorSelecting.transform.position;
            PullAmount = CalculatePull(handPosition);
        }

        private float CalculatePull(Vector3 handPosition)
        {
            Vector3 position = start.position;
            Vector3 pullDirection = handPosition - position;
            Vector3 targetDirection = end.position - position;
            float maxLength = targetDirection.magnitude;

            targetDirection.Normalize();
            float pullDistance = Vector3.Dot(pullDirection, targetDirection) / maxLength;

            return Mathf.Clamp(pullDistance, 0.0f, 1.0f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(start.position, end.position);
        }
    }
}