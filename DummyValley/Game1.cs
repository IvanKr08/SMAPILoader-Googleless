using System;

namespace StardewValley {
    public class Game1 : InstanceGame {
        public static Game1 game1;
        public static int xEdge;
        public static int toolbarPaddingX;

        static Game1() {
            throw new InvalidProgramException();
        }

        public static void emergencyBackup() { }
        public void OnAppPause() { }
        public void OnAppResume() { }
    }
}
