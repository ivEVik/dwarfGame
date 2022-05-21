using System;
using System.Collections.Generic;
using System.Linq;

namespace dwarfGame
{
	public class Mob
	{
		private List<Tile> movePath;
		private bool acting;
		private bool current;
		private string act;
		
		private int flags;
		
		public bool Ally;
		
		public Spritesheet Sheet;
		public string Name;
		
		public int MaxHealth;
		public int Health;
		public bool Action;
		public Attack Weapon;
		
		public Dice Str;
		public Dice Con;
		public Dice Dex;
		public Dice Per;
		public Dice Int;
		public Dice Wil;
		
		public Dictionary<string, int> Skills;
		
		public int Dir;
		public int MaxMovement;
		public int Movement;
		public int X;
		public int Y;
		
		public Mob() {}
		
		public Mob(MobTemplate template, string name = "", int x = 0, int y = 0, bool ally = false)
		{
			X = x;
			Y = y;
			Dir = CONST.DIR_SOUTH;
			act = "";
			
			Sheet = new Spritesheet(template.MobID);
			Sheet.SetCoords(X, Y);
			Sheet.SetDestination(X, Y);
			Sheet.StartAnimation(CONST.ACTION_IDLE, Dir);
			
			Name = name;
			
			Weapon = template.Weapon;
			
			Str = template.Str;
			Con = template.Con;
			Dex = template.Dex;
			Per = template.Per;
			Int = template.Int;
			Wil = template.Wil;
			
			Skills = new Dictionary<string, int>(template.Skills);
			
			Ally = ally;
			MaxMovement = template.Movement;
			
			MaxHealth = template.Health + Con.RollSuc();
			Health = MaxHealth;
			
			flags = template.Flags;
			
			movePath = new List<Tile>();
			acting = false;
			current = false;
		}
		
		public void StartTurn()
		{
			if(CheckFlag(FLAG.MOB_DEAD))
			{
				Game.WipeCurrent();
				EndTurn();
			}
			
			DropFlags(FLAG.MOB_DEFENDING);
			
			Movement = MaxMovement;
			Action = true;
			acting = false;
			current = true;
		}
		
		public void EndTurn()
		{
			current = false;
			Game.PassTurn();
		}
		
		public void TakeDamage(int damage)
		{
			Health -= damage;
			
			if(Health <= 0)
				Die();
		}
		
		private void Die()
		{
			flags = FLAG.MOB_DEAD;
			Game.WipeMob(this);
		}
		
		public bool CheckFlag(int flag)
		{
			if((flags & flag) == flag)
				return true;
			return false;
		}
		
		public void DropFlags(int flags)
		{
			this.flags -= this.flags & flags;
		}
		
		public void Process()
		{
			Sheet.Process();
			if(!acting && current && Movement == 0)
				EndTurn();
			if(current)
				TryMove();
			
			if(acting && Sheet.GetAction() != act)
				acting = false;
		}
		
		public Tile GetTile()
		{
			return Game.Map[X, Y];
		}
		
		public void Move(List<Tile> path)
		{
			if(path.Count() > Movement || acting)
				return;
			
			movePath = path;
			acting = true;
		}
		
		private void TryMove()
		{ 
			if(movePath.Count() == 0 || Sheet.IsLocked())
				return;
			
			Move(movePath.FirstOrDefault());
			movePath.Remove(movePath.FirstOrDefault());
			
			if(movePath.Count() == 0 && Sheet.IsLocked())
			{
				Sheet.PrepIdle = true;
				acting = false;
			}
		}
		
		private void Move(Tile tile)
		{
			if(!tile.IsPassable())
				throw new Exception("Invalid target tile.");
			
			Dir = GetDir(tile);
			
			GetTile().RemoveMob(this);
			tile.AddMob(this);
			
			if(Sheet.GetDir() != Dir || Sheet.GetAction() != CONST.ACTION_MOVE)
				Sheet.StartAnimation(CONST.ACTION_MOVE, Dir, true);
			
			Sheet.SetDestination(X, Y);
			Movement--;
		}
		
		public bool CanPath(Tile tile)
		{
			return GetTile().CanPath(tile, Movement);
		}
		
		public List<Tile> Path(Tile tile)
		{
			return GetTile().Path(tile, Movement);
		}
		
		public int GetDistance(int x, int y)
		{
			return GetTile().GetDistance(x, y);
		}
		
		public int GetDistance(Tile tile)
		{
			return GetTile().GetDistance(tile.X, tile.Y);
		}
		
		public int GetDistance(Mob mob)
		{
			return GetTile().GetDistance(mob.X, mob.Y);
		}
		
		private int GetDir(Tile tile)
		{
			return GetTile().GetDir(tile);
		}
		
		private int GetDir(Mob mob)
		{
			return GetTile().GetDir(mob);
		}
		
		public void Animate(string action, bool doLock = false, bool noCycle = false)
		{
			if(Sheet.IsLocked())
				return;
			
			act = action;
			acting = doLock;
			Sheet.StartAnimation(action, Dir, doLock, noCycle);
		}
		
		public int Check(string action, int mod = 0)
		{
			switch(action)
			{
				case CONST.ACTION_STRIKE:
					return (Str + Skills[CONST.SKILL_MELEE] + mod).RollSuc();
				case CONST.ACTION_DODGE:
					return (Dex + Skills[CONST.SKILL_DODGE] + mod).RollSuc();
				case CONST.ACTION_CAST:
					return (Wil + Skills[CONST.SKILL_CAST] + mod).RollSuc();
				default:
					throw new Exception("Invalid skill check ID '${action}' passed to '${Name}' at (${X} ${Y})");
			}
		}
		
		public void Act(string action, Mob mob = null, Tile tile = null)
		{
			if(!Action)
				return;
			Action = false;
			
			switch(action)
			{
				case CONST.ACTION_STRIKE:
					if(mob == null)
						throw new Exception("Invalid target for action passed to '${Name}' at (${X} ${Y})");
					
					Dir = GetDir(mob);
					mob.Dir = mob.GetDir(this);
					Animate(action, true, true);
					
					if(Check(CONST.ACTION_STRIKE) > 0 && !mob.CheckFlag(FLAG.MOB_DEFENDING))
					{
						mob.Animate(CONST.ACTION_STAGGER, true);
						mob.TakeDamage(Weapon.RollDamage(Str));
						break;
					}
					
					if(Check(CONST.ACTION_STRIKE) > mob.Check(CONST.ACTION_DODGE))
					{
						mob.Animate(CONST.ACTION_STAGGER, true);
						mob.TakeDamage(Weapon.RollDamage(Str));
						break;
					}
					mob.Animate(CONST.ACTION_DODGE, true);
					
					break;
				default:
					throw new Exception("Invalid action ID '${action}' passed to '${Name}' at (${X} ${Y})");
			}
		}
	}
}
