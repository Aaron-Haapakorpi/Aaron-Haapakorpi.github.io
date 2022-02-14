#include <Windows.h>
#include <iostream>
#include <string>
#include <fcntl.h>
#include <io.h>
#include <iostream>
#include "kayttoliittyma.h"
#include "ruutu.h"

using namespace std;
long long total= 0;

Kayttoliittyma* Kayttoliittyma::instance = 0;


Kayttoliittyma* Kayttoliittyma::getInstance()
{
	if (instance == 0)
		instance = new Kayttoliittyma();
	return instance;
}

void Kayttoliittyma::piirraLauta()
{
	//std::wcout << "moves were " << total;
	//Saa unicode shakkinappulat toimimaan printf kanssa:
	//SetConsoleOutputCP(65001);

	//char q[] = "King: \xE2\x99\x94.\n";
	//printf(q);
	//Saa unicode shakkinappulat toimimaan cout:n kanssa:
	_setmode(_fileno(stdout), _O_U16TEXT);

	//std::wcout << _asema->_lauta[7][7]->getUnicode();
	//std::cout << "1";

	wcout << endl;

	/*

		Kaksinkertainen for silmukka joka käy asema-olion laudan läpi ja jos
		siinä on nappula olio, niin kysytään sen nimi
		ja tulostetaan se muussa tapauksessa tulostetaan 2 tyhjää välilyöntiä.
		Parillisuuteen perustuen värjätään jokatoisen "ruudun" taustaväri
	*/
	//std::cout << "\u2654";
	//std::cout << _asema->_lauta[0][0]->getKoodi;
	//std::wcout << _asema->_lauta[0][0]->getUnicode();
	SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), BACKGROUND_INTENSITY | BACKGROUND_RED |
		BACKGROUND_GREEN | BACKGROUND_BLUE);

	for (int i = 7; i >= 0; i--) {

		if (i == 0) {
			wcout << "1";
		}
		else if (i == 1) {
			wcout << "2";
		}
		else if (i == 2) {
			wcout << "3";
		}
		else if (i == 3) {
			wcout << "4";
		}
		else if (i == 4) {
			wcout << "5";
		}
		else if (i == 5) {
			wcout << "6";
		}
		else if (i == 6) {
			wcout << "7";
		}
		else if (i == 7) {
			wcout << "8";
		}
		for (int j = 0; j < 8; j++) {

			if (j % 2 == 0) {

				if (i % 2 == 0)
					SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), BACKGROUND_INTENSITY | BACKGROUND_RED |
						BACKGROUND_GREEN | BACKGROUND_BLUE);

				else
					SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), BACKGROUND_INTENSITY | BACKGROUND_RED |
						BACKGROUND_GREEN);
			}
			else {
				if (i % 2 == 0)
					SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), BACKGROUND_INTENSITY | BACKGROUND_RED |
						BACKGROUND_GREEN);

				else
					SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), BACKGROUND_INTENSITY | BACKGROUND_RED |
						BACKGROUND_GREEN | BACKGROUND_BLUE);
			}
			if (_asema->_lauta[i][j] != nullptr) { //test asd
				std::wcout << _asema->_lauta[i][j]->getUnicode();
				std::wcout << " ";
			}
			else std::wcout << "  ";

		}
		std::wcout << "\n";
	}

	for (int x = 0; x < 8; x++) {

		if (x == 0) {
			wcout << " A ";
		}
		else if (x == 1) {
			wcout << "B ";
		}
		else if (x == 2) {
			wcout << "C ";
		}
		else if (x == 3) {
			wcout << "D ";
		}
		else if (x == 4) {
			wcout << "E ";
		}
		else if (x == 5) {
			wcout << "F ";
		}
		else if (x == 6) {
			wcout << "G ";
		}
		else if (x == 7) {
			wcout << "H " << endl;
		}
	}

}


/*
	Aliohjelma tarkistaa että käyttäjän antama syöte siirroksi on
	muodollisesti korrekti (ei tarkista aseman laillisuutta)
	Ottaa irti myös nappulan kirjaimen (K/D/L/R/T), tarkistaa että kirjain korrekti
*/

int returnCoordinate(char c) {
	int coordinate;
	coordinate = (int)c - 97;
	return coordinate;
	

}
int returnCoordinateInt(char c) {
	int coordinate;
	coordinate = (int)c - 49;
	return coordinate;
}
Siirto Kayttoliittyma::annaVastustajanSiirto()
{
	Ruutu alku;
	Ruutu loppu;
	wstring c;
	int lahtoX;
	int lahtoY;
	int tuloX;
	int tuloY;
	bool wrongInput = true;

	/*std::wcout << "a " << (int)c;//97
	 c = 'b';
	std::wcout << "b " << (int)c;//98
	 c = 'c';
	std::wcout << "c " << (int)c;//99
	c = 'd';
	std::wcout << "d " << (int)c;//100
	c = 'e';
	std::wcout << "e " << (int)c;//101
	 c = 'f';
	std::wcout << "f " << (int)c;//102
	 c = 'g';
	std::wcout << "g " << (int)c;//103
	 c = 'h';
	std::wcout << "h " << (int)c;//104

	 c = 'K';

	std::wcout <<"siirto on" << (int)c;//75
	 c = 'D';
	std::wcout << "siirto on" << (int)c;//68
	 c = 'L';
	std::wcout << "siirto on" << (int)c;//76
	c = 'R';
	std::wcout << "siirto on" << (int)c;//82
	 c = 'T';
	std::wcout << "siirto on" << (int)c;//84
	*/
	
	while (wrongInput) {
		wrongInput = false;
		wcout << "\nAnna siirto" << endl;

		wcin >> c;
		int size = c.length();
		
		if (size == 6) {

			c.erase(0, 1);

		}
		else if (c[0] == 'O') {
			//ei estä linnan tekemistä jos ei mahdollista, ei tarkista listasta
			if (size == 3) {
				Siirto linna(true, false);
				return linna;
			}
			else if (size == 5) {
				Siirto linna(false, true);
				return linna;

			}
			else wrongInput = true;
		}
		else if(size!=6 && size !=5) wrongInput = true;
		
		if (size == 5 || size == 6) {
			std::vector<Siirto> lista;
			_asema->annaLaillisetSiirrot(lista);


			lahtoX = (int)c.at(0);
			lahtoY = (int)c.at(1);
			tuloX = (int)c.at(3);
			tuloY = (int)c.at(4);

			alku.setRivi(returnCoordinate(lahtoX));
			alku.setSarake(returnCoordinateInt(lahtoY));


			loppu.setRivi(returnCoordinate(tuloX));
			loppu.setSarake(returnCoordinateInt(tuloY));
			Siirto siirto(alku, loppu);
			wrongInput = true;

			for (auto siirtoL : lista) {
				if (siirto.getAlkuruutu().getX() == siirtoL.getAlkuruutu().getX() && siirto.getAlkuruutu().getY() == siirtoL.getAlkuruutu().getY()
					&& siirto.getLoppuruutu().getX() == siirtoL.getLoppuruutu().getX() && siirto.getLoppuruutu().getY() == siirtoL.getLoppuruutu().getY()) {
					wrongInput = false;
					break;


				}
			}
		}
		
		
	}
	

//	Siirto siirto;

	lahtoX = (int)c.at(0);
	lahtoY = (int)c.at(1);
	tuloX = (int)c.at(3);
	tuloY = (int)c.at(4);


	
	alku.setRivi(returnCoordinate(lahtoX));
	alku.setSarake(returnCoordinateInt(lahtoY));

	loppu.setRivi(returnCoordinate(tuloX));
	loppu.setSarake(returnCoordinateInt(tuloY));

	
	Siirto siirto(alku, loppu);
	if ((loppu.getY() == 7 && _asema->_lauta[alku.getY()][alku.getX()]->getKoodi() == VS) || (loppu.getY() == 0 && _asema->_lauta[alku.getY()][alku.getX()]->getKoodi() == MS)) {
		//korotukset
		std::wcout << "Miksi nappulaksi haluat korottaa? (esim. D)\n";
		std::wstring korotettuNappula;
		std::wcin >> korotettuNappula;
		if (loppu.getY() == 7) {
			if (korotettuNappula == L"D" || korotettuNappula == L"d") {
				siirto._miksikorotetaan = _asema->vd;
			}
			else if (korotettuNappula == L"T" || korotettuNappula == L"t") {
				siirto._miksikorotetaan = _asema->vt;
			}
			else if (korotettuNappula == L"R" || korotettuNappula == L"r") {
				siirto._miksikorotetaan = _asema->vr;
			}
			else if (korotettuNappula == L"L" || korotettuNappula == L"l") {
				siirto._miksikorotetaan = _asema->vl;
			}
		}

		if (loppu.getY() == 0) {
			if (korotettuNappula == L"D" || korotettuNappula == L"d") {
				siirto._miksikorotetaan = _asema->md;
			}
			else if (korotettuNappula == L"T" || korotettuNappula == L"t") {
				siirto._miksikorotetaan = _asema->mt;
			}
			else if (korotettuNappula == L"R" || korotettuNappula == L"r") {
				siirto._miksikorotetaan = _asema->mr;
			}
			else if (korotettuNappula == L"L" || korotettuNappula == L"l") {
				siirto._miksikorotetaan = _asema->ml;
			}

		}
	}
	
	return siirto;

	
}


int Kayttoliittyma::kysyVastustajanVari()
{
	wcout << "millä värillä pelaat? (0 = valkoinen, 1 = musta)";
	int vari;
	wcin >> vari;
	_asema->setSiirtovuoro(vari);
	return vari;
}

