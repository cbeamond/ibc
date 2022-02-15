using System.Collections.Generic;

namespace Ironbelly.Collections
{
	/// <summary>
	/// Allows O1 complexity for insertion, removal and index lookups
	/// </summary>
	/// <typeparam name=">The type of key to use as a key</typeparam>
	/// <typeparam name="TValue">The type of value being stored</typeparam>
	public class IndexedLinkedList<TValue>
	{
		private readonly Dictionary<TValue, IndexedLinkedListNode<TValue>> index;
		public int Count => index.Count;
		public IndexedLinkedListNode<TValue> Last { get; private set; }
		public IndexedLinkedListNode<TValue> First { get; private set; }

		public TValue this[TValue index]
		{
			get => this.index[index].Value;
			set => this.index[index].Value = index;
		}

		public IndexedLinkedList()
		{
			index = new Dictionary<TValue, IndexedLinkedListNode<TValue>>();
		}

		public IndexedLinkedListNode<TValue> AddAfter(IndexedLinkedListNode<TValue> node, TValue value)
			=> AddAfter(node, new IndexedLinkedListNode<TValue>(value));

		public IndexedLinkedListNode<TValue> AddAfter(IndexedLinkedListNode<TValue> node, IndexedLinkedListNode<TValue> newNode)
		{
			if (Last == node)
				Last = newNode;
			else
				node.Next.Previous = newNode;

			node.Next = newNode;
			index[newNode.Value] = newNode;
			return newNode;
		}

		public IndexedLinkedListNode<TValue> AddBefore(IndexedLinkedListNode<TValue> node, TValue value)
			=> AddBefore(node, new IndexedLinkedListNode<TValue>(value));

		public IndexedLinkedListNode<TValue> AddBefore(IndexedLinkedListNode<TValue> node, IndexedLinkedListNode<TValue> newNode)
		{
			if (First == node)
				First = newNode;
			else
				node.Previous.Next = newNode;

			node.Previous = newNode;
			index[newNode.Value] = newNode;
			return newNode;
		}

		public IndexedLinkedListNode<TValue> AddFirst(TValue value)
			=> AddFirst(new IndexedLinkedListNode<TValue>(value));

		public IndexedLinkedListNode<TValue> AddFirst(IndexedLinkedListNode<TValue> node)
		{
			if (First != null)
			{
				node.Next = First;
				First.Previous = node;
			}

			if (Last == null)
				Last = node;

			First = node;
			index[node.Value] = node;
			return node;
		}

		public IndexedLinkedListNode<TValue> AddLast(TValue value)
			=> AddLast(new IndexedLinkedListNode<TValue>(value));

		public IndexedLinkedListNode<TValue> AddLast(IndexedLinkedListNode<TValue> node)
		{
			if (Last != null)
			{
				node.Previous = Last;
				Last.Next = node;
			}

			if (First == null)
				First = node;

			Last = node;
			index[node.Value] = node;
			return node;
		}

		public void Clear()
		{
			First = null;
			Last = null;
			index.Clear();
		}

		public bool Contains(TValue value) => index.ContainsKey(value);

		public TValue Pop()
		{
			TValue value = First.Value;
			RemoveFirst();
			return value;
		}

		public void Remove(TValue value)
		{
			if (index.TryGetValue(value, out var node))
				Remove(node);
		}

		public void Remove(IndexedLinkedListNode<TValue> node)
		{
			if (node.Previous != null)
				node.Previous.Next = node.Next;

			if (node.Next != null)
				node.Next.Previous = node.Previous;

			if (First == node)
				First = node.Next;

			if (Last == node)
				Last = node.Previous;

			index.Remove(node.Value);
		}

		public void RemoveFirst() => Remove(First);

		public void RemoveLast() => Remove(Last);
	}
}