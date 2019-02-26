# Flare-battleship

## Overview

This is a Battleship program built in C# as an exercise for Flare.

There are two players in the game, and each player has a 10x10 grid. At the start of the game, each players can place any 'n' numbers of warcraft. The position of these warships are determined through `.Random()`.

Players are also provided with a console error message if they decide to place 'n' number of warships greater than the grid size (100).

### Game Structure

Players then take turns to provide coordinates (X and Y) used to attack the oppponents board/grid. The program checks to see if there is a Warship on those coordinates, displays a "HIT!" if there is and a "MISS!" if there isn't. In addition to that, logic has been implemented to display an additional error if a warship on the coordinates has ALREADY been hit.

Once the player has sunk/hit all the Warships deployed in the opponents grid, they are then presented with a console message saying that they've won the game.

The program will display errors and give users feedback when
- Users try to deploy a warcraft at the start of the game which is outside the 10x10 grid.
- Users try to attack a board coordinate.

### Classes: 
- Battleship
- Player
- Board
- Point
