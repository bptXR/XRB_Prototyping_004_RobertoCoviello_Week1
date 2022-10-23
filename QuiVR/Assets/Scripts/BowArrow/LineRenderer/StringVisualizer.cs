using UnityEngine;

namespace BowArrow.LineRenderer
{
    [ExecuteInEditMode]
    public class StringVisualizer : MonoBehaviour
    {
        [Header("Render Positions")] 
        [SerializeField] private Transform start;
        [SerializeField] private Transform middle;
        [SerializeField] private Transform end;
        [SerializeField] private UnityEngine.LineRenderer lineRenderer;

        private void Update()
        {
            if (Application.isEditor && !Application.isPlaying) UpdatePosition();
        }

        private void OnEnable() => Application.onBeforeRender += UpdatePosition;

        private void OnDisable() => Application.onBeforeRender -= UpdatePosition;

        private void UpdatePosition() => lineRenderer.SetPositions(new Vector3[] { start.position, middle.position, end.position });
    }
}