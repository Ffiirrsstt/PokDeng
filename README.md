The Pok Deng game is developed using C# (.NET Framework) and stores data in Microsoft Access (.mdb files).

The game is played between the player (one person) and the dealer (system).

For betting, players can click chips, click "bet all," or choose via a trackbar.

At the start, players specify their initial amount of money. Then, they place bets and begin playing the game.

Players can continue to place bets, but if their remaining balance becomes too low, they will be invited to start a new game with the initial amount of money.

Pok Deng calculates the following rankings: Three of a kind > Straight > Pair, and other combinations. In the case of special hands, such as three of a kind facing another three of a kind, the winner is determined by who has the higher hand, such as Three Kings vs Three Queens.

This version of Pok Deng does not determine the winner by suit (Spades, Hearts, Clubs, Diamonds).  
- Three of a kind pays 5 times the bet.  
- Straight pays 3 times the bet.  
- Pair with the same suit on all three cards pays 3 times the bet.  
- Pair with the same suit on two cards pays 2 times the bet.  
- Pair of the same rank on two cards pays 2 times the bet.  
Note: When a Pok 8 or Pok 9 is dealt, the game ends immediately, and the result is revealed.

In cases where the player's hand has any value, they can choose to draw or not. After the player decides, it's the dealer's turn to decide whether to draw or not. Once the dealer's decision is made, the game results will be revealed.

After the game results are shown, there will be a brief loading period (displayed as a progress bar), after which the player can decide how much to bet for the next round.

- The game system displays animation for dealing the cards.  
- The game does not store gameplay data (no player information is saved; it is a single-player offline game with no tracking of individual play sessions).
