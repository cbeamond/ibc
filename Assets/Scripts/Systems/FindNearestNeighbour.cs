using UnityEngine;

namespace Ironbelly.Systems
{
	public class FindNearestNeighbour : MonoBehaviour
	{
		public FindNearestNeighbour nearestNeighbour =>
			FindNearestNeighbourSystem.Instance.GetNearestNeighbour(this);

		public void OnEnable()
		{
			FindNearestNeighbourSystem.Instance.AddNeighbour(this);
		}

		public void OnDisable()
		{
			FindNearestNeighbourSystem.Instance.RemoveNeighbour(this);
		}
	}
}