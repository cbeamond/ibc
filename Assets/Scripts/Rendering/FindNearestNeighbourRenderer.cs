using UnityEngine;

namespace Ironbelly.Utility
{
	[RequireComponent(typeof(FindNearestNeighbour))]
	[RequireComponent(typeof(LineRenderer))]
	public class FindNearestNeighbourRenderer : MonoBehaviour
	{
		private LineRenderer lineRenderer;
		private FindNearestNeighbour nearestNeighbour;

		private void Start()
		{
			nearestNeighbour = GetComponent<FindNearestNeighbour>();
		}

		public void OnEnable()
		{
			if (!lineRenderer)
			{
				lineRenderer = GetComponent<LineRenderer>();
				lineRenderer.useWorldSpace = true;
			}

			lineRenderer.enabled = false;
		}

		public void OnDisable()
		{
			lineRenderer.enabled = false;
		}

		// We want to use LateUpdate to ensure that all movement and animation
		// has concluded before we update and render
		private void LateUpdate()
		{
			FindNearestNeighbour targetNeighbour = nearestNeighbour.Find();

			if (!targetNeighbour)
			{
				lineRenderer.enabled = false;
				return;
			}

			lineRenderer.enabled = true;
			lineRenderer.SetPositions(new Vector3[]
			{
				transform.position,
				targetNeighbour.transform.position
			});
		}
	}
}