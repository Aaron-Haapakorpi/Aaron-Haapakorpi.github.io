#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <algorithm>
#include <map>
#include <string>
#include "bot_interface.h"
#include "bots/chain_reaction.h"
#include "bots/gramma.h"

#define NOF_BOTS 2 //37
//#define WAIT_FOR_KEYPRESS

Bot* create_bot(int);				// Create a new bot object.
void err_exit(const std::string&);	// Exit program with an error message.
int get_throw_score(int, int);		// Get score for a single throw.


/*
	Rock-Paper-Scissors tournament.
*/
int main()
{
	// Initialize RNG, in case that some bots need random numbers. 
	// This should NOT be done again in bots' code.
	srand((unsigned)time(0));

	// Each bot plays each other once.
	double tour_score[NOF_BOTS] = {};
	double hundredMatchScore[NOF_BOTS] = {};
	double blueTempScore = 0;
	double redTempScore = 0;
	int match_no = 0;
	for (int i = 0; i < (NOF_BOTS - 1); ++i)
		for (int j = i + 1; j < NOF_BOTS; ++j)
		{
			blueTempScore = 0;
			redTempScore = 0;
			// Start a new match.
			match_no += 1;
			Bot* blue = create_bot(i);
			Bot* red = create_bot(j);
			printf
			(
				"MATCH %3d : %-25s vs. %-25s ", 
				match_no, 
				blue->get_name().c_str(), 
				red->get_name().c_str()
			);
			//std::cout << "\n";

			// Play the rounds.
			int blue_score = 0;
			int red_score = 0;
			for (int games = 0; games < LOOPS; games++) {
				delete blue;
				delete red;
				blue_score = 0;
				red_score = 0;
				blue = create_bot(i);
				red = create_bot(j);
				for (int r = 0; r < ROUNDS; ++r)
				{
					int blue_throw = blue->get_throw(r);
					int red_throw = red->get_throw(r);
					blue_score += get_throw_score(blue_throw, red_throw);
					red_score += get_throw_score(red_throw, blue_throw);
					blue->own_throw[r] = blue_throw;
					blue->opp_throw[r] = red_throw;
					red->own_throw[r] = red_throw;
					red->opp_throw[r] = blue_throw;
				}
				if (blue_score > red_score)
				{
					blueTempScore += 1.0;
				}
				else if (red_score > blue_score)
				{
					//winner = "won by " + red->get_name();
					redTempScore += 1.0;
				}
				else
				{
					//winner = "draw ";
					blueTempScore += 0.5;
					redTempScore+= 0.5;
				}
				//std::cout << "bl" << blue_score << "re" << red_score <<"\n";
			}
			
			
			// Decide the winner and update tournament scores.
			std::string winner;
			if (blueTempScore > redTempScore)
			{ 
				winner = "won by " + blue->get_name();
				tour_score[i] += 1.0;
			}
			else if (blueTempScore < redTempScore)
			{ 
				winner = "won by " + red->get_name();
				tour_score[j] += 1.0;
			}
			else
			{ 
				winner = "draw ";
				tour_score[i] += 0.5;
				tour_score[j] += 0.5;
			}
			printf("%-35s (%4f - %4f)\n", winner.c_str(), blueTempScore, redTempScore);
			//std::cout << winner << " " << blueTempScore << " " << redTempScore;

			// Get rid of the competitors.
			delete blue;
			delete red;

#ifdef WAIT_FOR_KEYPRESS
			getch();
#endif
		}

	// Announce tournament results.
	printf("\n=====================================================================================================================\n");
	printf("FINAL SCORES\n");
	printf("=====================================================================================================================\n");
	std::multimap<double, std::string> scores;
	for (int i = 0; i < NOF_BOTS; ++i)
	{
		Bot* b = create_bot(i);
		scores.insert(std::pair<double, std::string>(-tour_score[i], b->get_name()));
		delete b;
	}
	for (auto s : scores)
	{
		printf("%-25s : %.2f / %d (%.2f)\n", s.second.c_str(), -s.first, (NOF_BOTS - 1), -s.first / (NOF_BOTS - 1));
	}

	return 0;
}


Bot* create_bot(int id)
{
	switch (id)
	{
	case 0: return new My_Bot_gramma;
	case 1: return new My_Bot_chain_reaction;
	default	: return 0;
	}
}


void err_exit(const std::string& msg)
{
	printf("%s\n", msg.c_str());
	exit(1);
}


int get_throw_score(int own_throw, int opp_throw)
{
	// Wins.
	if (own_throw == ROCK && opp_throw == SCISSORS)
		return 1;
	if (own_throw == PAPER && opp_throw == ROCK)
		return 1;
	if (own_throw == SCISSORS && opp_throw == PAPER)
		return 1;

	// A loss or a tie.
	return 0;
}
