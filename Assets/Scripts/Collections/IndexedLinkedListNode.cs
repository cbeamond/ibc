namespace Ironbelly.Collections
{
	public class IndexedLinkedListNode<TValue>
	{
		public IndexedLinkedListNode(TValue value)
		{
			Value = value;
		}

		public IndexedLinkedList<TValue> List { get; set; }
		public IndexedLinkedListNode<TValue> Next { get; set; }
		public IndexedLinkedListNode<TValue> Previous { get; set; }
		public TValue Value { get; set; }
	}
}