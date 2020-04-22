# TV-Game-Jam

To download and try the game, check out https://itch.io/jam/tvgamejam/rate/370956

This game was a submission to the TV Game Jam, 2019 and was our first game jam.
The game was based on Doctor who, season 7, episode 10.

We realise that much of this game needs to be revised to be a complete and polished game. 
That being said, the main gameplay mechanics were developed in time for submission to the gamejam.
The game may be updated in the future to improve it's polish and general condition, however there are no plans to do so currently.
If you feel inspired to make some improvements, feel free to make a PR or fork the project.

## Structure

- Rougelite style
- Randomly Generated Map (or procedurally connected tiles)
  - Traps/Puzzles in some tiles
- Terraria-style movement
- Time distortion
  - Player locations are tracked such that a "echo" of their character will wander the map as they explore, following their previous path.
    - If you touch an echo, you die and assume control of the echo.  All echos that spawned before the one you assumed control of die as well. If killed by other means, you create a zombie at your death point and assume control of oldest echo
    - Zombies should dissapear if a puzzle is solved and spawn at the new puzzle.
  - Player deaths result in hostile zombies that spawn based on the location the player died
    - If killed by one of these monsters, the game re-starts completely.
