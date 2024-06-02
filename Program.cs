using (var game = new MpDemo.Main())
game.Run(Microsoft.Xna.Framework.GameRunBehavior.Synchronous);

/* while (true) {
    // Must be called from the primary thread.
    SteamAPI.RunCallbacks();

    if (Console.KeyAvailable) {
        ConsoleKeyInfo info = Console.ReadKey(true);

        if (info.Key == ConsoleKey.Escape) {
            break;
        }
        else if (info.Key == ConsoleKey.Spacebar) {
            SteamUserStats.RequestCurrentStats();
            Console.WriteLine("Requesting Current Stats");
        }
        else if (info.Key == ConsoleKey.D1) {
            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard("Quickest Win");
            m_callResultFindLeaderboard.Set(hSteamAPICall);
            Console.WriteLine("FindLeaderboard() - " + hSteamAPICall);
        }
        else if (info.Key == ConsoleKey.D2) {
            SteamAPICall_t hSteamAPICall = SteamUserStats.GetNumberOfCurrentPlayers();
            m_NumberOfCurrentPlayers.Set(hSteamAPICall);
            Console.WriteLine("GetNumberOfCurrentPlayers() - " + hSteamAPICall);
        }
    }

    Thread.Sleep(50);
} */
