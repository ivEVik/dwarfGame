using System;
using System.Collections.Generic;

namespace dwarfGame
{
	public class MobTemplate
	{
		public readonly string MobID;
		
		public readonly int Health;
		public readonly int Movement;
		
		public readonly Attack Weapon;
		
		public readonly int Flags;
		
		public readonly Dice Str;
		public readonly Dice Con;
		public readonly Dice Dex;
		public readonly Dice Per;
		public readonly Dice Int;
		public readonly Dice Wil;
		
		public readonly Dictionary<string, int> Skills;
		
		public readonly Func<Mob, bool> Controller;
		
		public MobTemplate(
			string MobID,
			int Health,
			int Movement,
			Attack Weapon,
			int Str,
			int Con,
			int Dex,
			int Per,
			int Int,
			int Wil,
			int Flags,
			Dictionary<string, int> Skills,
			Func<Mob, bool> Controller
		)
		{
			this.MobID = MobID;
			this.Health = Health;
			this.Movement = Movement;
			
			this.Weapon = Weapon;
			
			Dice dice = new Dice(CONST.DIE_D6);
			this.Str = Str * dice;
			this.Con = Con * dice;
			this.Dex = Dex * dice;
			this.Per = Per * dice;
			this.Int = Int * dice;
			this.Wil = Wil * dice;
			
			this.Flags = Flags;
			this.Skills = Skills;
			
			this.Controller = Controller;
		}
	}
}
