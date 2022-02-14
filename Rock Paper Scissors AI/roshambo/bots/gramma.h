#include "../bot_interface.h"
#include <unordered_map>
#include <cstdlib>
#include <algorithm>
#include <iostream>

class My_Bot_gramma : public Bot
{
private:
	int count = 1000;

	int depth = 4;

	struct record {
		int rps[3];
	};

	std::unordered_map<std::string, record> ntable;

	void createTablePermutation(std::string prev, int n) {
		if (n == 0) {
			ntable[prev] = {};
		}
		else {
			for (size_t i = 0; i < 3; i++)
			{
				for (size_t j = 0; j < 3; j++)
				{
					std::string key = prev;
					key.append(std::to_string(i));
					key.append(std::to_string(j));
					createTablePermutation(key, n - 1);
				}
			}
		}
	}

public:

	My_Bot_gramma() {
		createTablePermutation(std::string(), depth);
		createTablePermutation(std::string(), depth - 3);
		createTablePermutation(std::string(), depth - 2);
		createTablePermutation(std::string(), depth - 1);

	}

	virtual std::string get_name()
	{
		/*
			COME UP WITH A NICE BOT NAME.
		*/
		return "1-4:n gramman botti";
	}

	virtual int get_throw(int round)
	{
		
		if (round <= depth) {
			return std::rand() % 3;
		}
		else {
			// Store enemy throws from previous 5 rounds
			for (size_t n = 0; n < 4; n++)
			{
				std::string key;
				if (round > 2 + n) {
					for (size_t i = 3 + n; i > 1; i--)
					{
						key.append(std::to_string(own_throw[round - i]));
						key.append(std::to_string(opp_throw[round - i]));
					}

					ntable[key].rps[opp_throw[round - 1]]++;
				}
			}

			// Find record by creating a key from last 4 rounds
			std::string key = std::string();
			for (size_t i = depth; i > 0; i--)
			{
				key.append(std::to_string(own_throw[round - i]));
				key.append(std::to_string(opp_throw[round - i]));
			}

			auto rec = ntable[key].rps;



			int tempDepth = depth;


			//if (round == 1000 - 1)std::cout << ntable.size();
			// If records is empty (no such sequence has been recored yet), try to find a shorter sequence
			while (rec[ROCK] == 0 && rec[PAPER] == 0 && rec[SCISSORS] == 0 && tempDepth > 1) {
				tempDepth--;
				key = std::string();
				for (size_t i = tempDepth; i > 0; i--)
				{
					key.append(std::to_string(own_throw[round - i]));
					key.append(std::to_string(opp_throw[round - i]));
				}

				
				rec = ntable[key].rps;
				//if (rec[0] != 0 && rec[0] > 1)break;
				//if (rec[1] != 0 && rec[1] > 1)break;
				//if (rec[2] != 0 && rec[1] > 1)break;
				
			}
			//std::cout << " at length " << tempDepth;
			//std::cout << "found " << rec[0] << " r" << rec[1]<< " p" << rec[2] << " s" << "\n";

			//else //std::cout << "found" << tempDepth;
			if (rec[ROCK] > rec[PAPER] && rec[ROCK] > rec[SCISSORS]) { // ROCK biggest
				return PAPER;
			}
			else if (rec[PAPER] > rec[ROCK] && rec[PAPER] > rec[SCISSORS]) { // PAPER biggest
				return SCISSORS;
			}
			else if (rec[SCISSORS] > rec[ROCK] && rec[SCISSORS] > rec[PAPER]) { // SCISSORS biggest
				return ROCK;
			}

			if (rec[ROCK] > rec[SCISSORS] && rec[ROCK] == rec[PAPER]) { // ROCK & PAPER equal > SCISSORS
				return PAPER;
			}
			else if (rec[ROCK] > rec[PAPER] && rec[ROCK] == rec[SCISSORS]) { // ROCK & SCISSORS equal > PAPER
				return ROCK;
			}
			else if (rec[PAPER] > rec[ROCK] && rec[PAPER] == rec[SCISSORS]) { // PAPER & SCISSORS equal > ROCK
				return SCISSORS;
			}
			
			return std::rand() % 3;
		}
	}
};
