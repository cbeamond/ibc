using System.Collections.Generic;
using UnityEngine;

//add logging!!!!
//add error handling!!!!
// right now this is O(n) complex to find. It's optimised, but still O(n). Can this be fixed? maybe BSP/etc?

namespace Ironbelly.Utility
{
	public class FindNearestNeighbour : MonoBehaviour
	{
		private static readonly HashSet<FindNearestNeighbour> neighbours
			= new HashSet<FindNearestNeighbour>();

		private int lastUpdatedFrame = -1;
		private FindNearestNeighbour nearest;

		public void OnEnable()
		{
			neighbours.Add(this);
		}

		public void OnDisable()
		{
			neighbours.Remove(this);

			if (nearest && nearest.nearest == this)
				nearest.nearest = null;
			nearest = null;
		}

		public FindNearestNeighbour Find()
		{
			if (lastUpdatedFrame == Time.frameCount)
				return nearest;

			float closestNeighbourDistanceSquared = float.MaxValue;

			foreach (var neighbour in neighbours)
			{
				if (!neighbour)
					continue;

				if (neighbour.lastUpdatedFrame == Time.frameCount)
					continue;

				float distanceSquared = (transform.position - neighbour.transform.position).sqrMagnitude;
				if (distanceSquared < closestNeighbourDistanceSquared)
					nearest = neighbour;
			}

			nearest.nearest = this;
			nearest.lastUpdatedFrame = Time.frameCount;
			return nearest;
		}
	}
}