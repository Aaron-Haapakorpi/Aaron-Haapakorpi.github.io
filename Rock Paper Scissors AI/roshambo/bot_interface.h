#pragma once
#include <string>

#define ROCK		0
#define PAPER		1
#define SCISSORS	2
#define ROUNDS		1000
#define LOOPS 150


/*
	Interface for a bot. Inherit your own bot from this.
*/
class Bot
{
public:

	// Return name of your bot. 
	// Using good taste is allowed.
	virtual std::string get_name() = 0;

	// Get bot's throw (ROCK, PAPER, or SCISSORS)
	// on given round (0..999). Before making
	// its decisions, your bot should study
	// opponent's earlier throws (see opp_throw).
	virtual int get_throw(int round) = 0;

	// Opponent's throws on previous rounds.
	// Your bot can study these in get_throw(),
	// valid indices go from [0] to [round - 1].
	// This array is automatically filled
	// for you as the match progresses.
	int opp_throw[ROUNDS];

	// Your bot's own, earlier throws,
	// just in case you need it.
	// This array is automatically filled
	// for you as the match progresses.
	int own_throw[ROUNDS];

	// Don't bother about this...
	virtual ~Bot() {}
};
