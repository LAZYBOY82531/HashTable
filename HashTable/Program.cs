﻿namespace HashTable
{
	internal class Program
	{
		/******************************************************
		 * 해시테이블 (Dictionary)
		 * 
		 * 키 값을 해시함수로 해싱하여 해시테이블의 특정 위치로 직접 엑세스하도록 만든 방식
		 * 해시 : 임의의 길이를 가진 데이터를 고정된 길이를 가진 데이터로 매핑
		 ******************************************************/
		//해시함수의 조건
		//1, 입력에 대한 해시함수의 결과가 항상 동일한 값이어야 한다.
		//2, 해시함수자체가 복잡하여 처리가 느린경우 의미가 없슴.

		//해시테이블 관한 내용
		//해시함수 결과의 밀집도가 낮아야 함.
		//해시테이블의 공간이 넓을수록 효율이 좋아진다. 다만 메모리에 부담이 된다.
		//삽입 삭제가 자주 일어날 경우 삽입삭제를 하면 할수록 효율이 안 좋아진다.


		// <해시테이블의 시간복잡도>
		// 접근			탐색			삽입			삭제
		//   X			O(1)	    	O(1)		    O(1)

		// <해시테이블 주의점 - 충돌> 외 기타 등등
		// 해시함수가 서로 다른 입력 값에 대해 동일한 해시테이블 주소를 반환하는 것
		// 모든 입력 값에 대해 고유한 해시 값을 만드는 것은 불가능하며 충돌은 피할 수 없음
		// 대표적인 충돌 해결방안으로 체이닝과 개방주소법이 있음
		//c#에서 체이닝은 노드기반이라 가비지컬렉터에 부담을 주므로 개방주소법을 더 많이 사용함.

		// <충돌해결방안 - 체이닝>
		// 해시 충돌이 발생하면 연결리스트로 데이터들을 연결하는 방식
		// 장점 : 해시테이블에 자료가 많아지더라도 성능저하가 적음
		// 단점 : 해시테이블 외 추가적인 저장공간이 필요

		// <충돌해결방안 - 개방주소법>
		// 해시 충돌이 발생하면 다른 빈 공간에 데이터를 삽입하는 방식
		// 해시 충돌시 선형탐색, 제곱탐색, 이중해시 등을 통해 다른 빈 공간을 선정
		// 장점 : 추가적인 저장공간이 필요하지 않음, 삽입삭제시 오버헤드가 적음
		// 단점 : 해시테이블에 자료가 많아질수록 성능저하가 많음
		// 해시테이블의 공간 사용률이 높을 경우 성능저하가 발생하므로 재해싱 과정을 진행함
		// 재해싱 : 해시테이블의 크기를 늘리고 테이블 내의 모든 데이터를 다시 해싱 

		void Dictionary()
		{
            System.Collections.Generic.Dictionary<string, Item> dictionary = new System.Collections.Generic.Dictionary<string, Item>();

			dictionary.Add("초기아이템", new Item("초보자용 검", 10));
			dictionary.Add("초기방어구", new Item("초보자용 가죽갑옷", 30));
			dictionary.Add("전직아이템", new Item("푸른결정", 1));

			Console.WriteLine(dictionary["초기아이템"]);       // 키값은 인덱서를 통해 접근


			if (dictionary.ContainsKey("전직아이템"))
				Console.WriteLine("전직아이템 키 값의 데이터가 있음");
			else
				Console.WriteLine("전직아이템 키 값의 데이터가 없음");

			if (dictionary.Remove("mp3"))
				Console.WriteLine("mp3 키 값에 해당하는 데이터를 지움");
			else
				Console.WriteLine("mp3 키 값에 해당하는 데이터를 못지움");

			if (dictionary.ContainsKey("초기아이템"))
				Console.WriteLine("딕셔너리에 초기아이템이 있음");
			else
				Console.WriteLine("딕셔너리에 초기아이템이 없음");
			/*
			string output;

			if (dictionary.TryGetValue("bmp", out output))
				Console.WriteLine(output);
			else
				Console.WriteLine("bmp 키 값의 데이터가 없음");*/
		}

		static void Main(string[] args)
		{
			HashTable.MyHashTable<string, string> me = new HashTable.MyHashTable<string, string>();
			me.Add("붕괴", "키아나");
			me.Add("원신", "루미네");
			me.Add("스타레일", "누군가");
			Console.WriteLine(me["스타레일"]);

		}
		public class Item
		{
			public string name;
			public int weight;

			public Item(string name, int weight)
			{
				this.name = name;
				this.weight = weight;
			}
		}
	}
}