# TV-Game-Jam

## Structure

- Rougelite style
- Randomly Generated Map (or procedurally connected tiles)
  - Traps/Puzzles in some tiles
- Terraria-style movement
- Time distortion
  - Player locations are tracked such that a "echo" of their character will wander the map as they explore, following their previous path.
    - If you touch an echo, you die and assume control of the echo.  All echos that spawned before the one you assumed control of die as well. If killed by other means, you create a zombie at your death point and assume control of oldest echo
  - Player deaths result in hostile zombies that spawn based on the location the player died
    - If killed by one of these monsters, the game re-starts completely.
