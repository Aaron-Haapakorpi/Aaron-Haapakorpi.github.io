#include "../bot_interface.h"


/*
	IMPLEMENT YOUR BOT HERE. NOTE THAT A NEW BOT OBJECT IS
	CREATED FOR EACH 1000 ROUND MATCH.

	RETURN THIS FILE TO TEACHER AS INSTRUCTED DURING THE CLASS.
*/
class My_Bot : public Bot
{
public:

	virtual std::string get_name()
	{
		/*
			COME UP WITH A NICE BOT NAME.
		*/
		return "My Bot";
	}

	virtual int get_throw(int round)
	{
		/*
			IMPLEMENT AI HERE, HOPEFULLY SOMETHING
			MORE SOPHISTICATED THAN THIS...

			REMEMBER, YOUR AI MAY STUDY EARLIER THROWS
			BY YOUR AND YOUR OPPONENT'S BOT:

				opp_throw[]
				own_throw[]
		*/
		return ROCK;
	}
};
