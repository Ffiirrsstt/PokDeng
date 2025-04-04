The Pok Deng game is developed using C# (.NET Framework) and stores data in Microsoft Access (.mdb format).

It is a Pok Deng game between a player (one person) and the dealer (system).

For betting, players can click chips, press "All In", or choose via a trackbar.

At the beginning, players specify their starting balance (initial money), then place a bet and start playing the game.

Players can continue placing bets, but if their balance becomes too low, they will be forced to exit (the game must restart with the initial money).

Pok Deng calculates the hand rankings: Three-of-a-kind > Straight > Numeric hands, etc. In cases of special hands, such as a Three-of-a-kind vs. another Three-of-a-kind, the result will be determined by the higher hand, such as Three Kings vs. Three Queens.

This version of Pok Deng does not consider the suit (spades, hearts, diamonds, clubs) when determining the winner.

- Three-of-a-kind pays 5 times.
- Straight pays 3 times.
- A hand with all three cards in the same suit pays 3 times.
- A hand with two cards in the same suit pays 2 times.
- A hand with two cards of the same rank pays 2 times.

Note: If a player gets a Pok 8 or Pok 9, the game will immediately reveal the result of the hand.

In the case where the player's hand has any value, the player can choose to draw another card or not. After the player's decision, it will be the dealer's turn to decide whether to draw a card. Afterward, the game result will be revealed.

After the game result is revealed, there will be a brief loading period (displayed as a progress bar), before allowing the player to select how much to bet for the next round.

- The game system includes animations for dealing cards.
- The game will not store any gameplay data (it is an offline game for a single player and does not store any player data between games).
