using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BowArrow
{
    public class Quiver : XRBaseInteractable
    {
        [SerializeField] private GameObject arrowPrefab;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            GetArrow(args);
        }

        private void GetArrow(SelectEnterEventArgs args)
        {
            Arrow arrow = SpawnArrow(args.interactorObject.transform);
            interactionManager.SelectEnter(args.interactorObject, arrow);
        }

        private Arrow SpawnArrow(Transform orientation)
        {
            GameObject arrowObject = Instantiate(arrowPrefab, orientation.position, orientation.rotation);
            return arrowObject.GetComponent<Arrow>();
        }
    }
}