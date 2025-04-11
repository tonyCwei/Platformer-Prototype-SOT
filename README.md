# Platformer-Prototype-SOT

Inspired by Prince of Persia: The Sands of Time on the Nintendo Game Boy Advance SP, I created a similar system that allows the player to rewind time to solve puzzles.

[![itch.io Version](https://img.shields.io/badge/Play%20on-itch.io-FA5C5C.svg?logoWidth=150)](https://frigidough.itch.io/sot)  

## Highlights

### Time Rewind

Just like in *The Sands of Time*, the player can consume energy to rewind time, solving platforming puzzles.

<p align="center">
  <img src="gifs/Time.gif" width="100%">
  <br>
  <em>Time Rewind</em>
</p>

The player's position is constantly recorded and stored in a list (`positions`). Here's a breakdown of how the system operates:

- **While not rewinding**:  
  The game pushes the player¡¯s current position into the `positions` list at regular intervals (every FixedUpdate). If the list exceeds the maximum allowed number of entries (based on the `rewindTimeLimit`), the oldest entries are removed from the front to maintain the duration cap.

- **While rewinding**:  
  The game reads the most recent position from the back of the list, sets the player to that position, and removes it. This creates the effect of the player moving backward in time. During this state, energy is consumed continuously.

Rewind ends either when:
- The position list becomes empty, or
- The player's energy is depleted.

### Animation

The animator controller as a Finite State Machine(FSM) I created for character animation

<p align="center">
  <img src="gifs/Animator.gif" width="100%">
  <br>
  <em>Animator</em>
</p>

