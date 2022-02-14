#include "ruutu.h"


Ruutu::Ruutu(int sarake, int rivi)
{
	_sarake = sarake;
	_rivi = rivi;
}


int Ruutu::getX()
{
	return _rivi;
}


int Ruutu::getY()
{
	return _sarake;
}


void Ruutu::setRivi(int rivi)
{
	this->_rivi = rivi;
	//_rivi = rivi;
}


void Ruutu::setSarake(int sarake)
{
	_sarake = sarake;
}