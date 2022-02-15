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

		public void OnEnable()
		{
			neighbours.Add(this);
		}

		public void OnDisable()
		{
			neighbours.Remove(this);
		}

		public FindNearestNeighbour Find()
		{
			FindNearestNeighbour nearest = null;
			float closestNeighbourDistanceSquared = float.MaxValue;

			foreach (FindNearestNeighbour neighbour in neighbours)
			{
				if (!neighbour)
					continue;

				if (neighbour == this)
					continue;

				float distanceSquared = (transform.position - neighbour.transform.position).sqrMagnitude;
				if (distanceSquared < closestNeighbourDistanceSquared)
				{
					nearest = neighbour;
					closestNeighbourDistanceSquared = distanceSquared;
				}
			}

			return nearest;
		}
	}
}