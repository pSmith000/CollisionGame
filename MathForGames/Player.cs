using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MathLibrary;

namespace MathForGames
{
    class Player : Actor
    {
        private float _speed;
        private Vector2 _velocity;
        private int _lives = 3;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int Lives
        {
            get { return _lives; }
            set { _lives = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Player(char icon, float x, float y, float speed, string name = "Actor", ConsoleColor color = ConsoleColor.White) 
            : base(icon, x, y, name, color)
        {
            _speed = speed;
        }

        public override void Update()
        {
            Vector2 moveDirection = new Vector2();

            ConsoleKey keyPressed = Engine.GetNextKey();

            if (keyPressed == ConsoleKey.A)
                moveDirection = new Vector2 { X = -1 };
            if (keyPressed == ConsoleKey.D)
                moveDirection = new Vector2 { X = 1 };
            if (keyPressed == ConsoleKey.W)
                moveDirection = new Vector2 { Y = -1 };
            if (keyPressed == ConsoleKey.S)
                moveDirection = new Vector2 { Y = 1 };

            Velocity = moveDirection * Speed;

            Position += Velocity;
            
        }

        public override void OnCollision(Actor actor)
        {
            if (actor.Name == "Wall")
                Position -= Velocity;
            else if (actor.Name == "ThrowZone")
                Position -= Velocity;
            else if (actor.Name == "Enemy")
            {
                Lives--;
                Position = new Vector2 { X = 3, Y = 10 };

                if (Lives <= 0)
                    Engine.CloseApplication();
            }
            else if (actor.Name == "End")
            {
                Engine.CloseApplication();
            }
                
            
                
                
        }
    }
}
