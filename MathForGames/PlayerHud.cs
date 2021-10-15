using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class PlayerHud : Actor
    {
        private Player _player;
        private UIText _lives;

        public PlayerHud(Player player, UIText lives)
        {
            _player = player;
            _lives = lives;
        }

        public override void Start()
        {
            base.Start();
            _lives.Start();
        }
        public override void Update()
        {
            _lives.Text = "Lives: " + _player.Lives.ToString();
        }

        public override void Draw()
        {
            _lives.Draw();
        }
    }
}
