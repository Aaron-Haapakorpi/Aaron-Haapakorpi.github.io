#include "../bot_interface.h"
#include <iostream>
#include <vector>
#include <unordered_map>
#include <string>
/*
	IMPLEMENT YOUR BOT HERE. NOTE THAT A NEW BOT OBJECT IS
	CREATED FOR EACH 1000 ROUND MATCH.

	RETURN THIS FILE TO TEACHER AS INSTRUCTED DURING THE CLASS.
*/

//Contains a competitive rock paper scissors bot using a few different method, with the main one being a n-gram/markov chain model which checks
//if that sequence has occurred before and picks the counterpick accordingly.

class My_Bot_chain_reaction : public Bot
{
public:

	virtual std::string get_name()
	{
		/*
			COME UP WITH A NICE BOT NAME.
		*/
		return "Chain Reaction";
	}

	
	std::unordered_map<std::string, int[3]> sequenceMoveCount; 

	const int depth = 2; //unused
	const int markovSize = 0; 
	int pickChange[1]; //stores possible move variations, unused 
	std::vector<int> pickChangeList;

	std::vector<std::vector<int>> pickChangeListList;
	
	long const int dictionarydepth =8; //only sets depth*rounds max amount of allocations to dictionary, so it's pretty fast


	void Initialize() {
		strategyScores[0] = 25; //markov chain/n-gram seems to be the best strategy so we give it a bias.
		strategyScores[3] = -2210;//end game markov chain starts with lower score. currently unused, only uses the enemy moves.
		strategyScores[2] = -25; //random counter pick starts at a -10 deficit because we want our actual strategies to be used. If the strategies seem to fail we switch to random guessing.
		strategyScores[1] = 15; //historymatching is the backup strategy, which seems to work well in some cases.
		for (int i = 0; i < markovSize; i++)
		{
			pickChange[i] = 0;

		}
	}

	int sequence[30];
	int ownSequence[30];

	
	//added long after for clarification of what this does as i remember. Searches past history of moves for the longest sequence of moves it's seen with the current sequence.
	//then it counterpicks that move. it's pretty good. idk if this is worse or better than n-gram/markov chains overall. and well
	//markov chain could also pick with a probabliy instead. perhaps that would be even better.
	int HistoryMatching(int &r, int upToIndex) {
		bool found = false;
		int nextMove = 0;
		int foundLength = 0;
		int currentLength = 0;

		for (int j = 0; j < upToIndex; j++)
		{
			sequence[j] = opp_throw[r - j - 1];
			ownSequence[j] = own_throw[r - j - 1];
		}
		for (int index = r - upToIndex; index >= 0; index--)
		{
			if (opp_throw[index] == sequence[0] && own_throw[index] == ownSequence[0]) {
				currentLength = 1;
				for (int j = 0; j < upToIndex; j++) {
					if (opp_throw[index - j] != sequence[j] || own_throw[index - j] != ownSequence[j])
					{
						break;
					}
					else {

						currentLength += 1;
						if (foundLength < currentLength) {

							nextMove = opp_throw[index + 1]; //next move is set to opponent's move after this sequence
							foundLength = currentLength;

						}
					}

				}
			}

		}
		if (foundLength >= 1)
		{
			strategyPlayed = HISTORY;
			return counterPick(nextMove);
		}

		return -1;
	}
	int strategyScores[4]; //0 markov, 1 history, 2 kind of random, 3 markov variation 2
	int enemyMarkovScore = 0;
	int strategyPlayed = -1;//etc


	enum Strat {
		MARKOV = 0, HISTORY = 1, RANDOM = 2, MARKOVENEMY = 3
	};

	Strat pickStrategy() {

		int max = strategyScores[0];
		int strat = 0;
		for (int i = 1; i < 4; i++) {
			if (strategyScores[i] > max) {
				strat = i;
				max = strategyScores[i];
			}
		}
		return static_cast<Strat>(strat);
	}

	//decision making for picking the next round's strategy
	virtual int get_throw(int round)
	{

		if (strategyPlayed != -1) {
			if (opp_throw[round - 1] == counterPick(own_throw[round - 1]))
			{
				strategyScores[strategyPlayed]--;
			}
			else if (opp_throw[round - 1] != own_throw[round - 1])strategyScores[strategyPlayed]++;
		}
		Strat strategy = pickStrategy();
		if (round == 0)
		{
			Initialize();
			return rand() % 3;
		}
		else if (round < dictionarydepth + 1) {
			return rand() % 3;
		}
		else {

			markovIncrementDictionary(round);
		}
		if (round > dictionarydepth) //needs some moves done before history and markov chaining works
		{
			if (round == 350) {
				strategyScores[3] += 25; //giving bias to endgame strategy, though currently unused.
			}
			if (strategy == HISTORY)
			{
				int historyFound = -1;
				historyFound = HistoryMatching(round, 10);
				if (historyFound != -1)return historyFound;
				else if (strategyScores[0] > strategyScores[2]) strategy = MARKOV;
			}

			//need to pick new strat if history fails

			if (strategy == MARKOV) {
				bool retflag;
				int retval;
				retval = markovPickDictionary(round, retflag);
				if (retflag) return retval;
				
				if (strategyScores[1] > strategyScores[2]) { 
					strategy = HISTORY;
					int historyFound = -1;
					historyFound = HistoryMatching(round, 10);
					if (historyFound != -1)return historyFound;
				}
				
			}

		}

		return randomPickCounter(round);
	}
	void markovIncrementMove(int round)
	{
		int ind = 0;
		long int mul = 3;
		int lastOpp = opp_throw[round - 1];
		for (int i = 2; i < depth + 2; i++) {

			ind += opp_throw[round - i] * mul;
			mul *= 3;
			ind += own_throw[round - i] * mul;
			mul *= 3;

		}
		pickChange[ind + lastOpp] += 1;
	}

	void markovIncrementDictionary(int round) {
		std::string toDic="";
		int lastOpp = opp_throw[round - 1];
		toDic.reserve(dictionarydepth * 2);
		for (int i = 2; i < dictionarydepth+2; i++)
		{
			toDic.append(std::to_string(opp_throw[round - i]));
			toDic.append(std::to_string(own_throw[round - i]));
			sequenceMoveCount[toDic][lastOpp]++;
		}
	}

	int markovPickDictionary(int round, bool &retflag)
	{
		retflag = true;
		int rockCount = 0;
		int paperCount = 0;
		int scissorsCount = 0;
		std::string fromDic = "";
		fromDic.reserve(dictionarydepth * 2 + 2);
		for (int y = 1; y < dictionarydepth+1; y++) {

			fromDic.append(std::to_string(opp_throw[round - y]));
			fromDic.append(std::to_string(own_throw[round - y]));

		}
		fromDic.append("0");
		fromDic.append("0");//added because we remove two at the start

		for (int i = dictionarydepth - 1; i >= 0; i--) 
		{
			
			fromDic.pop_back();
			fromDic.pop_back();
			
			if (sequenceMoveCount.find(fromDic) == sequenceMoveCount.end()) {
				continue;
			}
			
			rockCount = sequenceMoveCount[fromDic][0];
			paperCount = sequenceMoveCount[fromDic][1];
			scissorsCount = sequenceMoveCount[fromDic][2];
			if (rockCount > scissorsCount) {
				strategyPlayed = MARKOV;
				if (rockCount >= paperCount) return PAPER;
				else if (rockCount == paperCount) return PAPER;
				return SCISSORS; //else biggest count must be paper, r >s but r<p
			}

			else if (paperCount > rockCount) {
				strategyPlayed = MARKOV;
				if (paperCount >= scissorsCount) return SCISSORS;
				else if (scissorsCount == paperCount) return SCISSORS;
				return ROCK; 
			}


			else if (scissorsCount > paperCount) {
				strategyPlayed = MARKOV;
				if (scissorsCount >= rockCount) return ROCK;
				else if (scissorsCount == rockCount) return ROCK; 
				return PAPER; 
			}
		}
		retflag = false;
		return {};
	}

	int counterPick(int move) {
		if (move == 0) return 1;
		else if (move == 1) return 2;
		else return 0;
	}

	int randomPickCounter(int & r) {

		strategyPlayed = RANDOM;
		return rand() % 3;

		int randomIndex = rand() % (r - 1);
		if (opp_throw[randomIndex] == 0)return 1;
		else if (opp_throw[randomIndex] == 1) return 2;
		else return 0;
	}
};
