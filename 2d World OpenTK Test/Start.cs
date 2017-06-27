using System;
using OpenTK;
using Control;

namespace Control
{
    class Start
    {
        static void Main(string[] args)
        {
            GameWindow window = new GameWindow(2000,1000);
            Game game = new Game(window);
        }
    }
}