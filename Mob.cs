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
		private Func<Mob, bool> controller;
		
		public bool HasMoved;
		public bool TookTurn;
		
		private int flags;
		
		public bool Ally;
		
		public Spritesheet Sheet;
		public string Name;
		
		public int MaxHealth;
		public int Health;
		public int Actions;
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
		
		public Mob(MobTemplate template, string name = "", bool ally = false)
		{
			X = -1;
			Y = -1;
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
			
			controller = template.Controller;
			
			Ally = ally;
			MaxMovement = template.Movement;
			
			MaxHealth = template.Health + Con.RollSuc() + Con.GetCount();
			Health = MaxHealth;
			
			flags = template.Flags;
			
			movePath = new List<Tile>();
			acting = false;
			current = false;
		}
		
		public void StartTurn()
		{
			if(CheckFlags(FLAG.MOB_DEAD))
			{
				Game.WipeCurrent();
				EndTurn();
			}
			
			DropFlags(FLAG.MOB_DEFENDING);
			
			Movement = MaxMovement;
			Actions = 1;
			acting = false;
			current = true;
			HasMoved = false;
			
			if(!Ally)
				controller(this);
		}
		
		public void EndTurn()
		{
			current = false;
			TookTurn = true;
			Game.PassTurn();
		}
		
		public void DropSelection()
		{
			current = false;
		}
		
		public void Select()
		{
			current = true;
		}
		
		public void Process()
		{
			Sheet.Process();
			TryMove();
			
			if(acting && Sheet.GetAction() != act)
				acting = false;
			
			if(!Ally && current)
				if(controller(this))
					EndTurn();
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
		
		public bool CheckFlags(int flags)
		{
			if((this.flags & flags) == flags)
				return true;
			return false;
		}
		
		public void DropFlags(int flags)
		{
			this.flags -= this.flags & flags;
		}
		
		public void AddFlags(int flags)
		{
			this.flags = this.flags | flags;
		}
		
		public bool CanAct()
		{
			return Actions > 0;
		}
		
		public Tile GetTile()
		{
			return Game.Map[X, Y];
		}
		
		public bool IsLocked()
		{
			return acting || Sheet.IsLocked();
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
		
		public bool CanPath(Mob mob)
		{
			return mob.GetTile().GetNeighbours().Any(tile => CanPath(tile));
		}
		
		public List<Tile> Path(Mob mob)
		{
			var tiles = mob.GetTile().GetNeighbours();
			var paths = tiles.Where(tile => CanPath(tile)).Select(tile => Path(tile));
			return paths.OrderBy(path => path.Count).FirstOrDefault();
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
			//if(!Action)
			//	return;
			//Action = false;
			
			switch(action)
			{
				case CONST.ACTION_MOVE:
					if(tile == null && mob == null)
						throw new Exception($"Invalid target for action '{action}' passed to {Name} at ({X} {Y})");
					
					if(mob != null)
						Move(Path(mob));
					else
						Move(Path(tile));
					
					HasMoved = true;
					
					return;
				case CONST.ACTION_STRIKE:
					if(Actions <= 0)
						return;
					if(mob == null)
						throw new Exception($"Invalid target for action '{action}' passed to '{Name}' at ({X} {Y})");
					
					Dir = GetDir(mob);
					mob.Dir = mob.GetDir(this);
					Animate(action, true, true);
					
					if(!mob.CheckFlags(FLAG.MOB_DEFENDING))
						if(Check(CONST.ACTION_STRIKE) > 0)
						{
							mob.Animate(CONST.ACTION_STAGGER, true, true);
							mob.TakeDamage(Weapon.RollDamage(Str));
							break;
						}
					else if(Check(CONST.ACTION_STRIKE) > mob.Check(CONST.ACTION_DODGE))
					{
						mob.Animate(CONST.ACTION_STAGGER, true, true);
						mob.TakeDamage(Weapon.RollDamage(Str));
						break;
					}
					mob.Animate(CONST.ACTION_DODGE, true, true);
					
					break;
				case null:
					return;
				default:
					throw new Exception($"Invalid action ID '{action}' passed to '{Name}' at ({X} {Y})");
			}
			
			Actions--;
		}
	}
}
