using System;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public class Dicepool
	{
		private Dictionary<int, Dice> dice;
		
		public Dicepool() {}
		
		public Dicepool(IEnumerable<Dice> pool, Dice dc)
		{
			dice = new Dictionary<int, Dice>();
			
			foreach(Dice d in pool)
				Add(d);
			
			Add(dc);
		}
		
		public Dicepool(params Dice[] pool)
		{
			dice = new Dictionary<int, Dice>();
			
			foreach(Dice d in pool)
				Add(d);
		}
		
		public Dicepool(IEnumerable<Dice> pool)
		{
			dice = new Dictionary<int, Dice>();
			
			foreach(Dice d in pool)
				Add(d);
		}
		
		public int RollSum()
		{
			int result = 0;
			
			foreach(Dice d in dice.Values)
				result += d.RollSum();
			
			return result;
		}
		
		public int RollSuc()
		{
			int result = 0;
			
			foreach(Dice d in dice.Values)
				result += d.RollSuc();
			
			return result;
		}
		
		public IEnumerable<Dice> GetPool()
		{
			return dice.Values;
		}
		
		private void Add(Dice d)
		{
			if(dice.ContainsKey(d.GetSize()))
				dice[d.GetSize()] += d.GetCount();
			else
				dice[d.GetSize()] = d;
		}
		
		public static Dicepool operator +(Dicepool pool) => pool;
		public static Dicepool operator +(Dicepool pool, Dicepool dp) => new Dicepool(pool.GetPool().Concat(dp.GetPool()));
		public static Dicepool operator +(Dicepool pool, Dice d) => new Dicepool(pool.GetPool(), d);
	}
}
