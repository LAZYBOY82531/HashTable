using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
	internal class Dictionary<TKey, TValue> where TKey : IEquatable<TKey>
	{
		private const int DefaultCapacity = 1000;

		private struct Entry
		{
			public enum State { None, Using, Deleted }

			public int hashCode;
			public State state;
			public TKey key;
			public TValue value;
		}

		private Func<TKey, int> hashFunc;
		private Entry[] table;

		public TValue this[TKey key]
		{
			get
			{
				int index = Math.Abs(hashFunc(key)) % table.Length;
				while (table[index].state == Entry.State.Using)
				{
					//동일한 키 값을 찾았을때 반환하기
					if (key.Equals(table[index].key))
					{
						return table[index].value;
					}
                    if (table[index].state != Entry.State.Using)
                    {
						break;
                    }
					index = index < table.Length - 1 ? index + 1 : 0;
				}
				throw new InvalidOperationException();
			}
			set
			{
				// key를 인덱스로 해싱
				int index = Math.Abs(hashFunc(key)) % table.Length;
				while (table[index].state == Entry.State.Using)
				{
					if (key.Equals(table[index].key))
					{
						table[index].value = value;
						return;
					}
					if (table[index].state != Entry.State.Using)
					{
						break;
					}
					index = index < table.Length - 1 ? index + 1 : 0;
				}
				throw new InvalidOperationException();
			}
		}

		private void Add(TKey key, TValue value)
		{
			//1, key를 index로 해싱
			int hashCode = hashFunc(key);
			int index = Math.Abs(hashCode) % table.Length;
			//자리를 계속 찾는 반복문
			while (table[index].state == Entry.State.Using)
			{
				if (key.Equals(table[index].key))
				{
					throw new InvalidOperationException();
				}
					index = index < table.Length - 1 ? index + 1 : 0;
			}
			table[index].hashCode = hashCode;
			table[index].state = Entry.State.Using;
			table[index].key = key;
			table[index].value = value;
		}

		public void Clear()
		{
			table = new Entry[DefaultCapacity];
		}

		public void Remove(TKey key)
		{
			int index = Math.Abs(hashFunc(key)) % table.Length;
			while (table[index].state == Entry.State.Using)
			{
				if (key.Equals(table[index].key))
				{
					table[index].state = Entry.State.Deleted;
				}
				if (table[index].state != Entry.State.Deleted)
				{
					break;
				}
				index = index < table.Length - 1 ? index + 1 : 0;
			}
			throw new InvalidOperationException();
		}
	}
}
