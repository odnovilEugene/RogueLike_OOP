using RogueLike.Components.Position;
using RogueLike.Core;

namespace RogueLike.Components
{
    public abstract class MovingGameObject : GameObject
    {
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public bool IsDead
        {
            get
            {
                return Hp <= 0;
            }
        }


        public MovingGameObject(Position2D pos,
                                int maxHp,
                                int attack,
                                string defaultSymbol)
                                :
                                base(pos, false, false, defaultSymbol) 
        {
            MaxHp = maxHp;
            Hp = MaxHp;
            Attack = attack;
        }

        public void Move(int dy, int dx)
        {
            Position = new Position2D(Position.Y + dy, Position.X + dx);
        }

        public void Move(Position2D pos)
        {
            Position = pos;
        }
        public void TakeDamage(int amount)
        {
            Hp -= amount;
            Hp = Hp > MaxHp ? MaxHp : Hp;
        }

        public string GetInfo()
        {
            var className = GetType().Name;
            return $"{className}: Hp {Hp} / {MaxHp}, Position {Position}";
        }
    }
}