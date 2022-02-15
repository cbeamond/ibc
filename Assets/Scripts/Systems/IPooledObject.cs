
namespace Ironbelly.Systems
{
	public interface IPooledObject
	{
		void DespawnObject();

		void OnObjectSpawned();

		void OnObjectDespawn();
	}
}