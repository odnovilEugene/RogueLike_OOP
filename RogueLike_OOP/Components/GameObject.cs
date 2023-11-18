using RogueLike.Components.Position;

namespace RogueLike.Components
{
    public abstract class GameObject
    {
        public Position2D Position { get; set; }
        public string DefaultSymbol { get; set; }
        public string CurrentSymbol { get; set; }
        public bool BeingVisited { get; set; }
        public bool Visitable { get; }
        public GameObject(Position2D pos, bool beingVisited, bool visitable, string defaultSymbol = " ")
        {
            DefaultSymbol = defaultSymbol;
            CurrentSymbol = DefaultSymbol;
            Position = pos;
            BeingVisited = beingVisited;
            Visitable = visitable;
        }

        public override string ToString()
        {
            return string.Format($"{CurrentSymbol}");
        }

        public static string operator +(string a, GameObject b)
        {
            return a + b.CurrentSymbol;
        }
    }
}