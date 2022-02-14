#include <list>
#include <string>
#include "asema.h"
#include "nappula.h"
using namespace std;
#include <iostream>


Nappula::Nappula(wstring unicode, int vari, int koodi, double arvo)
{
	_unicode = unicode;
	_vari = vari;
	_koodi = koodi;
	_arvo = arvo;
}


void Torni::annaSiirrot(std::vector<Siirto>& lista, Ruutu* ruutu, Asema* asema, int vari)
{
	
	int x = ruutu->getX();
	int y = ruutu->getY();
	//left of piece
	for (int count = x-1; count >=0; count--) {
		if (asema->_lauta[y][count] != nullptr) {
			if (asema->_lauta[y][count]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu,Ruutu(y,count)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(count);
			loppu.setSarake(y);
			lista.push_back(Siirto(*ruutu, loppu));
		}
	}
	//right of piece
	for (int count = x +1; count <8; count++) {
		if (asema->_lauta[y][count] != nullptr) {
			if (asema->_lauta[y][count]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(y,count)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(count);
			loppu.setSarake(y);
			lista.push_back(Siirto(*ruutu, loppu));
		}
	}

	//up of piece
	for (int count = y + 1; count <8; count++) {
		if (asema->_lauta[count][x] != nullptr) {
			if (asema->_lauta[count][x]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(count,x)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(x);
			loppu.setSarake(count);
			lista.push_back(Siirto(*ruutu, loppu));
		}
	}

	//down of piece
	for (int count = y-1; count >=0; count--) {
		if (asema->_lauta[count][x] != nullptr) {
			if (asema->_lauta[count][x]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(count, x)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(x);
			loppu.setSarake(count);
			lista.push_back(Siirto(*ruutu, loppu));
		}
	}
}


void Ratsu::annaSiirrot(std::vector<Siirto>& lista, Ruutu* ruutu, Asema* asema, int vari)
{
	//std::wcout << "Ratsu testi";
	int i[8] = { 2,2,1,1,-1,-1,-2,-2 };
	int j[8] = { 1,-1,2,-2,-2,2,1,-1 };

	int startPosX = ruutu->getX();
	int startPosY= ruutu->getY();

	for (int iter = 0; iter < 8; iter++) {
		int xPos = startPosX + i[iter];
		int yPos = startPosY + j[iter];
		//std::wcout << "move " << yPos << " " << xPos << "\n";
		if (xPos > -1 && yPos > -1 && xPos < 8 && yPos < 8) {
			if (asema->_lauta[yPos][xPos] != nullptr) {
				//std::wcout << "piece in way";
				//std::wcout << asema->_lauta[yPos][xPos]->getUnicode();	
				if (asema->_lauta[yPos][xPos]->getVari() != vari) {
					lista.push_back(Siirto(*ruutu, Ruutu(yPos, xPos)));
				}
				
			}
			else {
				//std::wcout << "ei nappulaa\n";
				lista.push_back(Siirto(*ruutu, Ruutu(yPos, xPos)));
				//std::wcout << xPos << " ja " << yPos;
			}
			
		}
	}
}


void Lahetti::annaSiirrot(std::vector<Siirto>& lista, Ruutu* ruutu, Asema* asema, int vari)
{
	
	int x = ruutu->getX();
	int y = ruutu->getY();
	//left of piece
	int countY = y-1;
	for (int count = x - 1; count >= 0; count--) {
		if (countY < 0)break;
		if (asema->_lauta[countY][count] != nullptr) {
			if (asema->_lauta[countY][count]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(countY, count)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(count);
			loppu.setSarake(countY);
			lista.push_back(Siirto(*ruutu, loppu));
		}
		countY--;
	}
	countY = y+1;
	//right of piece
	for (int count = x + 1; count < 8; count++) {
		if (countY > 7)break;
		if (asema->_lauta[countY][count] != nullptr) {
			if (asema->_lauta[countY][count]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(countY, count)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(count);
			loppu.setSarake(countY);
			lista.push_back(Siirto(*ruutu, loppu));
		}
		countY++;
	}
	countY = y-1;
	//viistoon alas
	for (int count = x + 1; count < 8; count++) {
		if (countY < 0)break;
		if (asema->_lauta[countY][count] != nullptr) {
			if (asema->_lauta[countY][count]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(countY,count)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(count);
			loppu.setSarake(countY);
			lista.push_back(Siirto(*ruutu, loppu));
		}
		countY--;
	}
	countY = y + 1;
	//viistoon ylˆs
	for (int count = x - 1; count >= 0; count--) {
		if (countY > 7)break;
		if (asema->_lauta[countY][count] != nullptr) {
			if (asema->_lauta[countY][count]->getVari() != vari) {
				lista.push_back(Siirto(*ruutu, Ruutu(countY, count)));
			}
			break;
		}
		else {
			Ruutu loppu;
			loppu.setRivi(count);
			loppu.setSarake(countY);
			lista.push_back(Siirto(*ruutu, loppu));
		}
		countY++;
	}
}


void Daami::annaSiirrot(std::vector<Siirto>& lista, Ruutu* ruutu, Asema* asema, int vari)
{
	
	
	asema->vt->annaSiirrot(lista, ruutu, asema, vari);
	asema->vl->annaSiirrot(lista, ruutu, asema, vari);

}


void Kuningas::annaSiirrot(std::vector<Siirto>& lista, Ruutu* ruutu, Asema* asema, int vari)
{
       /* perusidea on ett‰ kaikki viereiset ruudut ovat sallittuja.kuten tornilla ja l‰hetill‰,
		oman nappulan p‰‰lle ei voi menn‰ ja vastustajan nappulan voi syˆd‰.

		Kaikki muu kuninkaaseen liittyv‰ tarkistus tehd‰‰n eri paikassa */

	int startPosX = ruutu->getX();
	int startPosY = ruutu->getY();

	if (startPosX < 7) {
		if (asema->_lauta[startPosY][startPosX + 1] != nullptr) {
			if (asema->_lauta[startPosY][startPosX + 1]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY, startPosX + 1)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY, startPosX + 1)));
		}
	}

	if (startPosX < 7 && startPosY < 7) { //??
		if (asema->_lauta[startPosY + 1][startPosX + 1] != nullptr) {
			if (asema->_lauta[startPosY + 1][startPosX + 1]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX + 1)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX + 1)));
		}
	}

	if (startPosY < 7) {
		if (asema->_lauta[startPosY + 1][startPosX] != nullptr) {
			if (asema->_lauta[startPosY + 1][startPosX]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX)));
		}
	}
	if (startPosX > 0) {
		if (asema->_lauta[startPosY][startPosX - 1] != nullptr) {
			if (asema->_lauta[startPosY][startPosX - 1]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY, startPosX - 1)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY, startPosX - 1)));
		}
	}

	if (startPosX > 0 && startPosY > 0) {
		if (asema->_lauta[startPosY - 1][startPosX - 1] != nullptr) {
			if (asema->_lauta[startPosY - 1][startPosX - 1]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX - 1)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX - 1)));
		}
	}

	if (startPosY > 0) {
		if (asema->_lauta[startPosY - 1][startPosX] != nullptr) {
			if (asema->_lauta[startPosY - 1][startPosX]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX)));
		}
	}

	if (startPosY > 0 && startPosX < 7) {
		if (asema->_lauta[startPosY - 1][startPosX + 1] != nullptr) {
			if (asema->_lauta[startPosY - 1][startPosX + 1]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX + 1)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX + 1)));
		}
	}

	if (startPosY < 7 && startPosX > 0) {
		if (asema->_lauta[startPosY + 1][startPosX - 1] != nullptr) {
			if (asema->_lauta[startPosY + 1][startPosX - 1]->getVari() != vari)
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX - 1)));
		}
		else {
			lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX - 1)));
		}
	}
}



void Sotilas::annaSiirrot(std::vector<Siirto>& lista, Ruutu* ruutu, Asema* asema, int vari)
{
	int startPosX = ruutu->getX();
	int startPosY = ruutu->getY();
	
	//oletus ett‰ valkoinen pelaa alhaalla ja musta ylh‰‰ll‰
	if (vari==0) { 
		if (startPosY == 1) {
			if (asema->_lauta[startPosY+ 2][startPosX] == nullptr && asema->_lauta[startPosY + 1][startPosX] == nullptr) {
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 2, startPosX)));
			}
		}
		if (startPosY < 7) {
			
			if (asema->_lauta[startPosY + 1][startPosX] == nullptr) {
				Siirto s(*ruutu, Ruutu(startPosY +1, startPosX));
				lista.push_back(s);
				if (startPosY == 6) {
					lisaaSotilaanKorotukset(&s, lista, asema);
				}
			}
			if (startPosX > 0) {
				if (asema->_lauta[startPosY + 1][startPosX - 1] != nullptr) {
					if (asema->_lauta[startPosY + 1][startPosX - 1]->getVari() != vari)
						lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX - 1)));
				}
			}
		    if (startPosX < 7) {
				if (asema->_lauta[startPosY + 1][startPosX + 1]) {
					if (asema->_lauta[startPosY + 1][startPosX + 1]->getVari() != vari)
						lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX + 1)));
				}
			}


			int kaksois = asema->kaksoisaskelSarakkeella;
			//kaksoisaskel 
			if (kaksois != -1) {

				if (kaksois == startPosX - 1) {
					if (startPosY == 4) {

						lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX - 1)));
					}

				}
				else if (kaksois == startPosX + 1) {
					if (startPosY == 4) {

						lista.push_back(Siirto(*ruutu, Ruutu(startPosY + 1, startPosX + 1)));
					}
				}
			}
		}
	}
	else if (vari == 1) {
		if (startPosY == 6) {
			
			if (asema->_lauta[startPosY -2][startPosX] == nullptr && asema->_lauta[startPosY - 1][startPosX] == nullptr) {
				lista.push_back(Siirto(*ruutu, Ruutu(startPosY -2, startPosX)));
			}
		}
	    if (startPosY > 0) {
			if (asema->_lauta[startPosY - 1][startPosX] == nullptr) {
				Siirto s(*ruutu, Ruutu(startPosY - 1, startPosX));
				lista.push_back(s);
				if (startPosY == 1) {
					lisaaSotilaanKorotukset(&s, lista, asema);
				}
			}
			if (startPosX > 0) {
				if (asema->_lauta[startPosY - 1][startPosX - 1] != nullptr) {
					if (asema->_lauta[startPosY - 1][startPosX - 1]->getVari() != vari)
						lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX - 1)));
				}
			}
			if (startPosX < 7) {
				if (asema->_lauta[startPosY - 1][startPosX + 1]) {
					if (asema->_lauta[startPosY - 1][startPosX + 1]->getVari() != vari)
						lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX + 1)));
				}
			}
			//kaksoisaskel
			//ei testattu kunnolla viel‰

			int kaksois = asema->kaksoisaskelSarakkeella;
			
			if (kaksois!=-1) {
				
				if (kaksois == startPosX - 1) {
					if (startPosY == 3) {
						
						lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX - 1)));
					}
					
				}
				else if (kaksois == startPosX + 1) {
					if (startPosY == 3) {
						
						lista.push_back(Siirto(*ruutu, Ruutu(startPosY - 1, startPosX + 1)));
					}
				}
			}
		}

		
	}

	 
}

//lis‰‰ vain daamin ja hevosen korotuksiin. ovatko muut edes tarpeellisia? pit‰‰ viel‰ lis‰t‰ koodiin
void Sotilas::lisaaSotilaanKorotukset(Siirto* siirto, std::vector<Siirto>& lista, Asema* asema) {
	int vuoro = asema->getSiirtovuoro();
	if (vuoro == 0) {
		siirto->_miksikorotetaan = asema->vr;
		lista.push_back(*siirto);
		siirto->_miksikorotetaan = asema->vd;
		lista.push_back(*siirto);
	}
	else {
		siirto->_miksikorotetaan = asema->mr;
		lista.push_back(*siirto);
		siirto->_miksikorotetaan = asema->md;
		lista.push_back(*siirto);
	}
	
}
