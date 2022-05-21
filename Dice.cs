using System;

namespace dwarfGame
{
	public readonly struct Dice
	{
		private readonly int count;
		private readonly int size;
		
		public Dice(int size)
		{
			count = 1;
			this.size = size;
		}
		
		public Dice(int count, int size)
		{
			if(count < 0 || size < 1)
				throw new Exception("Invalid die size or count.");
			this.count = count;
			this.size = size;
		}
		
		public int GetCount()
		{
			return count;
		}
		
		public int GetSize()
		{
			return size;
		}
		
		public int RollSum()
		{
			int result = 0;
			
			for(int t = 0; t < count; t++)
				result += Game.Randomiser.Next(1, size);
			
			return result;
		}
		
		public int RollSuc()
		{
			int result = 0;
			
			for(int t = 0; t < count; t++)
				result += Game.Randomiser.Next(1, size) > 4 ? 1 : 0;
			
			return result;
		}
		
		public static Dice operator +(Dice d) => d;
		
		public static Dice operator +(Dice die, Dice d) => new Dice(die.count + d.count, die.size);
		
		public static Dice operator +(Dice die, int number) => new Dice(die.count + number, die.size);
		public static Dice operator +(int number, Dice die) => new Dice(die.count + number, die.size);
		
		public static Dice operator *(Dice die, int number) => new Dice(die.count * number, die.size);
		public static Dice operator *(int number, Dice die) => new Dice(die.count * number, die.size);
	}
}
