using System;

using Microsoft.Xna.Framework;

namespace StardewValley {
    public class GameRunner : Game {
        public static GameRunner instance;

        static GameRunner() {
            throw new InvalidProgramException();
        }
    }
}
