#include "siirto.h"
#include <iostream>´;


Siirto::Siirto(Ruutu &alkuRuutu, Ruutu &loppuRuutu) //just assign siirto, also copy should be enough
{
	_alkuRuutu = alkuRuutu;
	_loppuRuutu = loppuRuutu;
	_lyhytLinna = false;
	_pitkaLinna = false;
	
}


Siirto::Siirto(bool lyhytLinna, bool pitkaLinna)
{
	_lyhytLinna = lyhytLinna;
	_pitkaLinna = pitkaLinna;
}





Ruutu Siirto::getAlkuruutu()
{
	return _alkuRuutu;
}


Ruutu Siirto::getLoppuruutu()
{
	return _loppuRuutu;
}


bool Siirto::onkoLyhytLinna()
{
	return _lyhytLinna;
}


bool Siirto::onkoPitkälinna()
{
	return _pitkaLinna;
}
