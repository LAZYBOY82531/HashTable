using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
	internal class MyHashTable<TKey, TValue> where TKey : IEquatable<TKey>
	{
		private const int DefaultCapacity = 1000;               //해시테이블 기본크기

		private struct Entry
		{
			public enum State { None, Using, Deleted }

			public int hashCode;
			public State state;
			public TKey key;
			public TValue value;
		}

		private Func<TKey, int> hashFunc;           //인덱스 값
		private Entry[] table;                              //해쉬태이블 정의

		public TValue this[TKey key]                    //키를 사용해 값을 받는 행위
		{
			get
			{
				int index = Math.Abs(hashFunc(key)) % table.Length;   //키를 받아 인덱스로 해싱
				while (table[index].state == Entry.State.Using)
				{
					if (key.Equals(table[index].key))        					//동일한 키 값을 찾았을때 반환하기
					{
						return table[index].value;
						break;                                                             //반환하고 반복종료
					}
					index = index < table.Length - 1 ? index + 1 : 0;     //인덱스 값에 1씩 더하며 계속 반복
				}
				throw new InvalidOperationException();              //예외발생경우
			}
			set
			{
				// key를 인덱스로 해싱
				int index = Math.Abs(hashFunc(key)) % table.Length;
				while (table[index].state == Entry.State.Using)
				{
					if (key.Equals(table[index].key))
					{
						table[index].value = value;              //값을 인덱스에 덮어씌움
						return;
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
				if (key.Equals(table[index].key))          //이미 값이 있을 경우
				{
					throw new InvalidOperationException();      //예외처리
				}
				index = index < table.Length - 1 ? index + 1 : 0;
			}
			table[index].hashCode = hashCode;                //인덱스값 저장
			table[index].state = Entry.State.Using;         //값상태 저장
			table[index].key = key;                                   //키 저장
			table[index].value = value;                              //값 저장
		}

		public void Clear()                                      //테이블을 초기화 하는 함수
		{
			table = new Entry[DefaultCapacity];
		}

		public void Remove(TKey key)                                             //키를받아 키안의 값을 삭제하는 함수
		{
			int index = Math.Abs(hashFunc(key)) % table.Length;
			while (table[index].state == Entry.State.Using)
			{
				if (key.Equals(table[index].key))                          //값을 찾으면
				{
					table[index].value = default;                            //값을 기본값으로 덮어쓴다
					table[index].state = Entry.State.Deleted;       //값상태를 deleted로 저장한다
					break;
				}
				if (index < table.Length - 1)
				{
					index++;
				}
                else
                {
					break;
                }
			}
			throw new InvalidOperationException();
		}
	}
}
/*해싱과 해시함수에 대한 조사
*해시의 원리 : 해시함수란 임의의 길이를 갖는 키를 입력받아 고정된 길이의
*인덱스 값을 출력해주는 함수입니다. 그리고 이 함수에서 출력해 주는 인덱스 값을
*해시라고 합니다. 해시함수의 특징으론 입력값이 같다면 항상 같은 값을 내준다는
*특징을 가지고 있습니다.
*
*해싱함수의 효율 :해시테이블의 효율은 1이지만 어떤 입력값에 대해서도 해시 
*값을 구하는데 많은 자원과 노력이 소요되지 않고 계산속도가 빨라야 합니다. 
*그렇지 않을 경우 다른 자료구조를 사용하는 것이 더 효율적일 수 있습니다. 
*
*해시함수는 결정론적으로 작동해야 하며, 따라서 두 해시 값이 다르다면 그 해시값에
*대한 원래 데이터도 달라야 합니다. 해시함수의 질은 입력영역에서의 해시충돌 확률로
*결졍되는데, 해시충돌의 확률이 높을수록 서로 다른 데이터를 구별하기 어려워지고
*검색하는 비용이 증가하게 됩니다. 하지만 이를 만족하기 위해 함수가 복잡해지면
*자료구조의 효율이 떨어진다.
*
*해시테이블의 충돌 : 해시함수가 서로 다른 두 개의 입력값에 대해 동일한 출력값을
*내는 상황을 의미합니다. 해시함수가 무한한 가짓수의 입력값을 받아 유한한 가짓수의 
*출력값을 생성하는 경우, 비둘기집 원리에 의해 해시 충돌은 항상 존재합니다.
*해시충돌은 해시함수를 이용한 자료구조나 알고리즘의 효율성을 떨어뜨리며, 따라서
*해시함수는 해시충돌이 자주 발생하지 않도록 구성되야 합니다. 
*
*해시테이블 충돌을 해결하는 법으론 보통 두가지를 사용합니다.
*체이닝 : 중복된 해시값이 있는 경우 해당 슬롯을 연결리스트로 저장하는 방법입니다.
*이방법은 연결리스트로 인해 최악의 경우 수행시간이 O(n)이 됩니다. 해시를 사용하는
*이유는 복잡도가 O(1)이라는 장점인데 O(n)이 된다면 문제가 됩니다. 또한
*연결리스트에서 삭제가 진행되는 경우 갈비지콜렉터가 사용되므로 이에 대한 문제도
*있습니다.
*
*두번째 방법으론 개방주소법이 있습니다. 개방주소법이란 충돌을 피하기 위해 key를 
*해시테이블에 직접 저장하여 사용하는 것입니다. 개방주소법의 장점은 포인터를
*사용하지 않아도 되어 구현이 간편하며, 검색도 미세하기 빨라진다는 장점이 있으나, 
*테이블에 자료가 많아질 수록 성능저하가 눈에띄게 발생한다는 단점이 있습니다.
*해시테이블의 공간 사용률이 높을 경우 성능저하가 발생하므로 재해싱 과정을 진행하는
*방식으로 재해싱을 하여 이 단점을 매꾸는 것이 가능합니다.
*재해싱이란 해시테이블의 크기를 늘리고 테이블 내의 모든 데이터를 
*다시 해싱하는 방법입니다.*/

