//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Systems
{
	public interface IPooledObject
	{
		void OnObjectSpawned();

		void OnObjectDespawn();
	}
}