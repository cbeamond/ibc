using UnityEngine;

namespace Ironbelly.Systems
{
	public class ThreadedFindNearestNeighbour : MonoBehaviour
	{
		public ThreadedFindNearestNeighbour nearestNeighbour =>
			ThreadedFindNearestNeighbourSystem.Instance.GetNearestNeighbour(this);

		public void OnEnable()
		{
			ThreadedFindNearestNeighbourSystem.Instance.AddNeighbour(this);
		}

		public void OnDisable()
		{
			ThreadedFindNearestNeighbourSystem.Instance.RemoveNeighbour(this);
		}
	}
}