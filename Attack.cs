using System;

namespace dwarfGame
{
	public class Attack
	{
		public readonly Dice Damage;
		public readonly int Reach;
		
		private readonly int flags;
		
		
		public Attack() {}
		
		public Attack(Dice damage, int reach, int flags)
		{
			Damage = damage;
			Reach = reach;
			this.flags = flags;
		}
		
		public bool CheckFlags(int flags)
		{
			if((this.flags & flags) == flags)
				return true;
			return false;
		}
		
		public int RollDamage(Dice attribute)
		{
			if(CheckFlags(FLAG.ATTACK_ROLL_ATTRIBUTE))
				return (Damage + attribute.GetCount()).RollSum();
			
			return Damage.RollSum() + attribute.GetCount();
		}
	}
}
