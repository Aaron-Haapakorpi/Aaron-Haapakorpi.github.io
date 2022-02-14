#include <iostream>
#include "asema.h"
#include "minMaxPaluu.h"
#include "nappula.h"
#include "ruutu.h"
#include "kayttoliittyma.h"
#include <algorithm>
double daami = 900;
double torni = 500;
double lahetti = 330;
double ratsu = 320;
double sotilas = 100;
double kuningas = 20000;
Nappula* Asema::vk = new Kuningas(L"\u2654", 0, VK,kuningas);
Nappula* Asema::vd = new Daami(L"\u2655", 0, VD,daami);
Nappula* Asema::vt = new Torni(L"\u2656", 0, VT, torni);
Nappula* Asema::vl = new Lahetti(L"\u2657", 0, VL, lahetti);
Nappula* Asema::vr = new Ratsu(L"\u2658", 0, VR, ratsu);
Nappula* Asema::vs = new Sotilas(L"\u2659", 0, VS, sotilas);

Nappula* Asema::mk = new Kuningas(L"\u265A", 1, MK, kuningas);
Nappula* Asema::md = new Daami(L"\u265B", 1, MD, daami);
Nappula* Asema::mt = new Torni(L"\u265C", 1, MT, torni);
Nappula* Asema::ml = new Lahetti(L"\u265D", 1, ML,lahetti);
Nappula* Asema::mr = new Ratsu(L"\u265E", 1, MR, ratsu);
Nappula* Asema::ms = new Sotilas(L"\u265F", 1, MS, sotilas);

Nappula* lauta_kopio[8][8];
double mobKnight = 6.4;
double mobQueen = 3.5;
int mobRook = 9;
int mobBishop = 5;
//Teh‰‰n parempaan evaluointiin tarpeellisia tauluja. pit‰‰ ehk‰ tallettaa muualle. hidastaako ohjelmaa?	
int MRatsu[8][8] =
{ { -50, -40, -30, -30, -30, -30, -40, -50 },
{ -40, -20, 0, 0, 0, 0, -20, -40},
{ -30, 0, 10, 15, 15, 10, 0, -30},
{ -30, 5, 15, 20, 20, 15, 5, -30},
{ -30, 0, 15, 20, 20, 15, 0, -30},
{ -30, 5, 10, 15, 15, 10, 5, -30},
{ -40, -20, 0, 5, 5, 0, -20, -40},
{ -50, -40, -30, -30, -30, -30, -40, -50}
};

int MSotilas[8][8] =
{ { 0, 0, 0, 0, 0, 0, 0, 0 },
{50, 50, 50, 50, 50, 50, 50, 50},
{10, 10, 20, 30, 30, 20, 10, 10},
{5, 5, 10, 25, 25, 10, 5, 5},
{0, 0, 0, 20, 20, 0, 0, 0},
{5, -5, -10, 0, 0, -10, -5, 5},
{5, 10, 10, -20, -20, 10, 10, 5},
{0, 0, 0, 0, 0, 0, 0, 0}
};

int MLahetti[8][8] =
{ { -20,-10,-10,-10,-10,-10,-10,-20},
{-10,  0,  0,  0,  0,  0,  0,-10},
{-10,  0,  5, 10, 10,  5,  0,-10},
{-10,  5,  5, 10, 10,  5,  5,-10},
{-10,  0, 10, 10, 10, 10,  0,-10},
{-10, 10, 10, 10, 10, 10, 10,-10},
{-10,  5,  0,  0,  0,  0,  5,-10},
{-20,-10,-10,-10,-10,-10,-10,-20}
};

int MTorni[8][8] =
{ { 0, 0, 0, 0, 0, 0, 0, 0 },
{5, 10, 10, 10, 10, 10, 10, 5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{0, 0, 0, 5, 5, 0, 0, 0}
};

int MDaami[8][8] = 
{{ -20, -10, -10, -5, -5, -10, -10, -20 },
{-10, 0, 0, 0, 0, 0, 0, -10},
{-10, 0, 5, 5, 5, 5, 0, -10},
{-5, 0, 5, 5, 5, 5, 0, -5},
{0, 0, 5, 5, 5, 5, 0, -5},
{-10, 5, 5, 5, 5, 5, 0, -10},
{-10, 0, 5, 0, 0, 0, 0, -10},
{-20, -10, -10, -5, -5, -10, -10, -20}
};

//Sitten mustat, jotka ovat peilikuva edellisist‰

int VRatsu[8][8] =
{ { -50, -40, -30, -30, -30, -30, -40, -50 },
{ -40, -20, 0, 5, 5, 0, -20, -40},
{ -30, 5, 10, 15, 15, 10, 5, -30},
{ -30, 0, 15, 20, 20, 15, 0, -30},
{ -30, 5, 15, 20, 20, 15, 5, -30},
{ -30, 0, 10, 15, 15, 10, 0, -30},
{ -40, -20, 0, 0, 0, 0, -20, -40},
{ -50, -40, -30, -30, -30, -30, -40, -50}
};

int VSotilas[8][8] =
{ { 0, 0, 0, 0, 0, 0, 0, 0 },
{5, 10, 10, -20, -20, 10, 10, 5},
{5, -5, -10, 0, 0, -10, -5, 5},
{0, 0, 0, 20, 20, 0, 0, 0},
{5, 5, 10, 25, 25, 10, 5, 5},
{10, 10, 20, 30, 30, 20, 10, 10},
{50, 50, 50, 50, 50, 50, 50, 50},
{0, 0, 0, 0, 0, 0, 0, 0}
};

int VLahetti[8][8] =
{ { -20,-10,-10,-10,-10,-10,-10,-20},
{-10,  5,  0,  0,  0,  0,  5,-10},
{-10, 10, 10, 10, 10, 10, 10,-10},
{-10,  0, 10, 10, 10, 10,  0,-10},
{-10,  5,  5, 10, 10,  5,  5,-10},
{-10,  0,  5, 10, 10,  5,  0,-10},
{-10,  0,  0,  0,  0,  0,  0,-10},
{-20,-10,-10,-10,-10,-10,-10,-20}
};

int VTorni[8][8] =
{ { 0, 0, 0, 5, 5, 0, 0, 0 },
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{-5, 0, 0, 0, 0, 0, 0, -5},
{5, 10, 10, 10, 10, 10, 10, 5},
{0, 0, 0, 0, 0, 0, 0, 0}
};

int VDaami[8][8] =
{ { -20, -10, -10, -5, -5, -10, -10, -20 },
{-10, 0, 5, 0, 0, 0, 0, -10},
{-10, 5, 5, 5, 5, 5, 0, -10},
{0, 0, 5, 5, 5, 5, 0, -5},
{-5, 0, 5, 5, 5, 5, 0, -5},
{-10, 0, 5, 5, 5, 5, 0, -10},
{-10, 0, 0, 0, 0, 0, 0, -10},
{-20, -10, -10, -5, -5, -10, -10, -20}
};

//Kuningasta ei tehty koska eri arvot eri vaiheissa peli‰

void getMovesByPiece(Asema&asema ,int vari, std::vector<Siirto>);

Asema::Asema()
{
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			_lauta[i][j] = nullptr;
		}
	}

	_lauta[0][0] = vt;
	_lauta[0][1] = vr;
	_lauta[0][2] = vl;
	_lauta[0][4] = vk;
	_lauta[0][3] = vd;
	_lauta[0][5] = vl;
	_lauta[0][6] = vr;
	_lauta[0][7] = vt;
	for (int i = 0; i < 8; i++) {
		_lauta[1][i] = vs;
		_lauta[6][i] = ms;
	}


	_lauta[7][0] = mt;
	_lauta[7][1] = mr;
	_lauta[7][2] = ml;
	_lauta[7][4] = mk;
	_lauta[7][3] = md;
	_lauta[7][5] = ml;
	_lauta[7][6] = mr;
	_lauta[7][7] = mt;


	// Ensin alustetaan kaikki laudan ruudut nappulla "NULL", koska muuten ruuduissa satunnaista tauhkaa

	// Asetetaan alkuaseman mukaisesti nappulat ruuduille

}

void Asema::paivitaAsemaVirtual(Siirto* siirto) {

	//Tarkastetaan on siirto lyhyt linna
	bool onkoLyhyt = siirto->onkoLyhytLinna();
	if (onkoLyhyt) {
		if (_siirtovuoro == 0) {
			_lauta[0][4] = NULL;
			_lauta[0][6] = vk;
			_lauta[0][7] = NULL;
			_lauta[0][5] = vt;
		}
		else {
			_lauta[7][4] = NULL;
			_lauta[7][6] = mk;
			_lauta[7][7] = NULL;
			_lauta[7][5] = mt;
		}
	}
	// onko pitk‰ linna
	else if (siirto->onkoPitk‰linna()) {

		if (_siirtovuoro == 0) {
			_lauta[0][4] = NULL;
			_lauta[0][2] = vk;
			_lauta[0][0] = NULL;
			_lauta[0][3] = vt;
		}
		else {
			_lauta[7][4] = NULL;
			_lauta[7][2] = mk;
			_lauta[7][0] = NULL;
			_lauta[7][3] = mt;
		}
	}
	else {
		// Kaikki muut siirrot


		//Ottaa siirron alkuruudussa olleen nappulan talteen 
		Ruutu alku = siirto->getAlkuruutu();

		//Laittaa talteen otetun nappulan uuteen ruutuun
		Ruutu loppu = siirto->getLoppuruutu();

		int alkuY = alku.getY();
		int alkuX = alku.getX();
		int loppuY = loppu.getY();
		int loppuX = loppu.getX();
		Nappula* n = _lauta[alku.getY()][alku.getX()];
		
		// Ohestalyˆnti on tyhj‰‰n ruutuun. Vieress‰ oleva (sotilas) poistetaan.
		if
			(
			(_lauta[alkuY][alkuX]->getKoodi() == VS || _lauta[alkuY][alkuX]->getKoodi() == MS) &&
				(alkuX != loppuX) &&
				(_lauta[loppuY][loppuX] == NULL)
				)
			_lauta[loppuY][alkuX] = NULL;

		if ((n->getKoodi() == VS || n->getKoodi() == MS) && (loppuY == 0 || loppuY == 7)) {
			_lauta[alkuY][alkuX] = NULL;
		}
		////muissa tapauksissa alkuruutuun null ja loppuruutuun sama alkuruudusta l‰htenyt nappula
		else {
			_lauta[alkuY][alkuX] = NULL;
			_lauta[loppuY][loppuX] = n;
		}
		switch (_siirtovuoro) {
		case 0:
			if (n == vs) {
				if (loppuY == 7) {
					_lauta[alkuY][alkuX] = NULL;
				}
				else {
					_lauta[alku.getY()][alku.getX()] = nullptr;
					_lauta[loppu.getY()][loppu.getX()] = n;
				}
			}
			



			break;
		case 1:
			if (n == ms) {
				if (loppuY == 0) {
					_lauta[alkuY][alkuX] = NULL;
				}
				else {
					_lauta[alku.getY()][alku.getX()] = nullptr;
					_lauta[loppu.getY()][loppu.getX()] = n;
				}
			}
			
			break;
		default:
			break;


		}

		if (siirto->_miksikorotetaan != NULL) {
			_lauta[loppu.getY()][loppu.getX()] = siirto->_miksikorotetaan;
		}

		//_lauta[alku.getY()][alku.getX()] = nullptr;
		//_lauta[loppu.getY()][loppu.getX()] = n;
	}





	//if (_siirtovuoro == 0)_siirtovuoro = 1;
	//else _siirtovuoro = 0;

}

void Asema::paivitaAsema(Siirto* siirto)
{
	// Kaksoisaskel-lippu on oletusarvoisesti pois p‰‰lt‰.
	this->kaksoisaskelSarakkeella = -1;
	
	//Tarkastetaan on siirto lyhyt linna
	bool onkoLyhyt = siirto->onkoLyhytLinna();
	if (onkoLyhyt) {
		if (_siirtovuoro == 0) {
			_onkoValkeaKuningasLiikkunut = true;
			_lauta[0][4] = NULL;
			_lauta[0][6] = vk;
			_lauta[0][7] = NULL;
			_lauta[0][5] = vt;
		}
		else {
			_onkoMustaKuningasLiikkunut = true;
			_lauta[7][4] = NULL;
			_lauta[7][6] = mk;
			_lauta[7][7] = NULL;
			_lauta[7][5] = mt;
		}
	}
	// onko pitk‰ linna
	else if (siirto->onkoPitk‰linna()) {

		if (_siirtovuoro == 0) {
			_onkoValkeaKuningasLiikkunut = true;
			_lauta[0][4] = NULL;
			_lauta[0][2] = vk;
			_lauta[0][0] = NULL;
			_lauta[0][3] = vt;
		}
		else {
			_onkoMustaKuningasLiikkunut = true;
			_lauta[7][4] = NULL;
			_lauta[7][2] = mk;
			_lauta[7][0] = NULL;
			_lauta[7][3] = mt;
		}
	}
	else {
		// Kaikki muut siirrot


		//Ottaa siirron alkuruudussa olleen nappulan talteen 
		Ruutu alku = siirto->getAlkuruutu();

		//Laittaa talteen otetun nappulan uuteen ruutuun
		Ruutu loppu = siirto->getLoppuruutu();

		int alkuY = alku.getY();
		int alkuX = alku.getX();
		int loppuY = loppu.getY();
		int loppuX = loppu.getX();
		Nappula* n = _lauta[alku.getY()][alku.getX()];
		// Tarkistetaan oliko sotilaan kaksoisaskel
		// (asetetaan kaksoisaskel-lippu)

		if
			(
			(_lauta[alkuY][alkuX]->getKoodi() == VS || _lauta[alkuY][alkuX]->getKoodi() == MS) &&
				(alkuY - loppuY == 2 || alkuY - loppuY == -2)
				)
			kaksoisaskelSarakkeella = alkuX;
		
		// Ohestalyˆnti on tyhj‰‰n ruutuun. Vieress‰ oleva (sotilas) poistetaan.
		if
			(
			(_lauta[alkuY][alkuX]->getKoodi() == VS || _lauta[alkuY][alkuX]->getKoodi() == MS) &&
				(alkuX != loppuX) &&
				(_lauta[loppuY][loppuX] == NULL)
				)
			_lauta[loppuY][alkuX] = NULL;

			
		//// Katsotaan jos nappula on sotilas ja rivi on p‰‰tyrivi niin ei vaihdeta nappulaa 
		////eli alkuruutuun laitetaan null ja loppuruudussa on jo kliittym‰n laittama nappula MIIKKA, ei taida minmaxin kanssa hehkua?
		if ((n->getKoodi() == VS || n->getKoodi() == MS) && (loppuY == 0 || loppuY == 7)) {
			_lauta[alkuY][alkuX] = NULL;
		}
		////muissa tapauksissa alkuruutuun null ja loppuruutuun sama alkuruudusta l‰htenyt nappula
		else {
			_lauta[alkuY][alkuX] = NULL;
			_lauta[loppuY][loppuX] = n;
		}
		switch (_siirtovuoro) {
		case 0:
			if (n == vs) {
				if (loppuY == 7) {
					_lauta[alkuY][alkuX] = NULL;
				}
				else {
					_lauta[alku.getY()][alku.getX()] = nullptr;
					_lauta[loppu.getY()][loppu.getX()] = n;
				}
			}
			else if (n == vk) {
				if (alkuY == 0 && alkuX == 4)
				_onkoValkeaKuningasLiikkunut = true;
			}
			else if (n == vt) {
				
				if (alkuY == 0 && alkuX == 7) {
					_onkoValkeaKTliikkunut = true;
				}
				else if (alkuY == 0 && alkuX == 0) {
					_onkoValkeaDTliikkunut = true;
				}

			}
		
			break;
		case 1:
			if (n == ms) {
				if (loppuY== 0) {
					_lauta[alkuY][alkuX] = NULL;
				}
				else {
					_lauta[alku.getY()][alku.getX()] = nullptr;
					_lauta[loppu.getY()][loppu.getX()] = n;
				}
			}
			else if (n == mk && (alkuY ==7 && alkuX==4)) {
				_onkoMustaKuningasLiikkunut = true;
		
			}
			else if (n == mt) {
				if (alkuY == 7 && alkuX == 0) {
					_onkoValkeaDTliikkunut = true;
				}
				else if (alkuY == 7 && alkuX == 7) {
					_onkoValkeaKTliikkunut = true;
				}
			}
			break;
		default:
			break;


		}
		
		if (siirto->_miksikorotetaan != NULL) {
			_lauta[loppu.getY()][loppu.getX()] = siirto->_miksikorotetaan;
		}

		//_lauta[alku.getY()][alku.getX()] = nullptr;
		//_lauta[loppu.getY()][loppu.getX()] = n;
	}

	if (_siirtovuoro == 0)_siirtovuoro = 1;
	else _siirtovuoro = 0;



	//
	////muissa tapauksissa alkuruutuun null ja loppuruutuun sama alkuruudusta l‰htenyt nappula


	// katsotaan jos liikkunut nappula on kuningas niin muutetaan onkoKuningasLiikkunut arvo (molemmille v‰reille)

	// katsotaan jos liikkunut nappula on torni niin muutetaan onkoTorniLiikkunut arvo (molemmille v‰reille ja molemmille torneille)

//p‰ivitet‰‰n _siirtovuoro

}


int Asema::getSiirtovuoro()
{
	return _siirtovuoro;
}


void Asema::setSiirtovuoro(int vuoro)
{
	_siirtovuoro = vuoro;
}


bool Asema::getOnkoValkeaKuningasLiikkunut()
{
	return _onkoValkeaKuningasLiikkunut;
}


bool Asema::getOnkoMustaKuningasLiikkunut()
{
	return _onkoMustaKuningasLiikkunut;
}


bool Asema::getOnkoValkeaDTliikkunut()
{
	return _onkoValkeaDTliikkunut;
}


bool Asema::getOnkoValkeaKTliikkunut()
{
	return _onkoValkeaKTliikkunut;
}


bool Asema::getOnkoMustaDTliikkunut()
{
	return _onkoMustaDTliikkunut;
}


bool Asema::getOnkoMustaKTliikkunut()
{
	return _onkoMustaKTliikkunut;
}


/* 1. Laske nappuloiden arvo
Daami = 9
Torni = 5
L‰hetti = 3,25
Ratsu = 3
Sotilas = 1

arvo asetettu nappuloihin

2. Kuninkaan hyvyys
Jos avaus tai keskipeli, niin hyv‰ ett‰ kunigas g1 tai b1/c1
Loppupeliss‰ vaikea sanoa halutaanko olla auttamassa omaa sotilasta korottumaan
vai olla est‰m‰ss‰ vastustajan korotusta siksi ei oteta kantaa
3. Arvosta keskustaa sotilailla ja ratsuilla (ei daamia)
4. Arvosta pitki‰ linjoja daami, torni ja l‰hetti
*/
//arvosta sotilaiden liikkumista
//arvosta puolustamista

double Asema::evaluoi(int color, std::vector<Siirto>&lista)
{


	double sum = 0;
	double sumWhite = 0;
	double sumBlack = 0;

	for (int y = 0; y < 8; y++) {
		for (int x = 0; x < 8; x++) {
			if (_lauta[y][x] != nullptr) {

				if (_lauta[y][x]->getVari() == 0) {
					if (_lauta[y][x]->getKoodi() == VR) {
						sumWhite += VRatsu[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == VS) {
						sumWhite += VSotilas[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == VL) {
						sumWhite += VLahetti[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == VT) {
						sumWhite += VTorni[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == VD) {
						sumWhite += VDaami[y][x];
					}

					sumWhite += _lauta[y][x]->_arvo;
				}
				else {
					if (_lauta[y][x]->getKoodi() == MR) {
						sumBlack += MRatsu[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == MS) {
						sumBlack += MSotilas[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == ML) {
						sumBlack += MLahetti[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == MT) {
						sumBlack += MTorni[y][x];
					}

					else if (_lauta[y][x]->getKoodi() == MD) {
						sumBlack += MDaami[y][x];
						
					}

					sumBlack += _lauta[y][x]->_arvo;
				}
			}

		}
	}
	
	//mobility values etc. maybe should add points for checking? need some end game evaluation also
	//need value array for king
	//endgame evaluation
	//also need to add a list for opponent moves and count size etc
	//also need to make seperate function that returns number of moves including ones that attack your own. maybe returns an array of each count 
	//maybe implement ruutuuhattu for each piece instead? 

	if (color == 0) {
	
		for (auto move : lista) {
			if (!move.onkoLyhytLinna() && !move.onkoPitk‰linna()) {
				Nappula* n = _lauta[move.getAlkuruutu().getY()][move.getAlkuruutu().getX()];
				if (n == vt) {
					//double the points for vertical rook movement
					if (move.getLoppuruutu().getY() != move.getAlkuruutu().getY())
						sumWhite += mobRook;
					else sumWhite += mobRook / 2;
				}
				else if (n == vr)sumWhite += mobKnight;
				else if (n == vl)sumWhite += mobBishop;
				else if (n == vd)sumWhite += mobQueen;
				
			}
			
		}

	
		sum = sumWhite - sumBlack;
		
		
	}
	else {
		//sumBlack += (int)lista.size() * 2;
		for (auto move : lista) {
			if (!move.onkoLyhytLinna() && !move.onkoPitk‰linna()) {
				Nappula* n = _lauta[move.getAlkuruutu().getY()][move.getAlkuruutu().getX()];
				if (n == mt) {
					if (move.getLoppuruutu().getY() != move.getAlkuruutu().getY())
						sumBlack += mobRook;
					else sumBlack += mobRook / 2;
				}
				else if (n == mr)sumBlack += mobKnight;
				else if (n == ml)sumBlack += mobBishop;
				else if (n == md)sumBlack += mobQueen;
			
			}

		}

		sum = sumBlack - sumWhite;
	}
	
	
	//kertoimet asetettu sen takia ett‰ niiden avulla asioiden painoarvoa voidaan s‰‰t‰‰ helposti yhdest‰ paikasta

	//1. Nappuloiden arvo

	//2. Kuningas turvassa

	//3. Arvosta keskustaa

	// 4. Arvosta linjoja
	return sum;
}


double Asema::laskeNappuloidenArvo(int vari)
{
	return 0;

}


bool Asema::onkoAvausTaiKeskipeli(int vari)
{
	return 0;
	// Jos upseereita 3 tai v‰hemm‰n on loppupeli
	// mutta jos daami laudalla on loppueli vasta kun kuin vain daami j‰ljell‰

	//Jos vari on 0 eli valkoiset
	//niin on keskipeli jos mustalla upseereita yli 2 tai jos daami+1


}


double Asema::nappuloitaKeskella(int vari)
{
	return 0;

	//sotilaat ydinkeskustassa + 0.25/napa
	//ratsut ydinkeskustassa + 0.25/napa
	//sotilaat laitakeskustassa + 0.11/napa
	//ratsut laitakeskustassa + 0.11/napa

	//valkeille ydinkeskusta



	//valkeille laitakeskusta



	//mustille ydinkeskusta

	//mustille laitakeskusta

}


double Asema::linjat(int vari)
{
	return 0;

	//valkoiset

	//mustat

}


// https://chessprogramming.wikispaces.com/Minimax MinMax-algoritmin pseudokoodi (lis‰sin parametrina aseman)
//int maxi(int depth, asema a) 
//	if (depth == 0) return evaluate();
//	int max = -oo;
//	for (all moves ) {
//		score = mini(depth - 1, seuraaja);
//		if (score > max)
//			max = score;
//	}
//	return max;
//}

//int mini(int depth, asema a) {
//	if (depth == 0) return -evaluate();
//	int min = +oo;
//	for (all moves) {
//		score = maxi(depth - 1);
//		if (score < min)
//			min = score;
//	}
//	return min;
//}
extern long long total;
MinMaxPaluu Asema::minimaxRoot(int syvyys, MinMaxPaluu& alpha, MinMaxPaluu& beta, bool maximizing)
{
	total = 0;
	//std::copy(&_lauta[0][0], &_lauta[0][0] + 8 * 8, &lauta_kopio[0][0]);

	MinMaxPaluu arvo;
	int parasArvo = -100000;
	MinMaxPaluu paluu;
	std::vector<Siirto> lista;
	lista.reserve(50); //makes it much faster when we reserve some memory
	annaLaillisetSiirrot(lista);



	paluu._evaluointiArvo = -100000;
	//Asema uusi_asema = *this;
	paluu._parasSiirto = lista[0]; //sets a move so it is not null in case no move is better
	for (auto& move : lista) {
		Asema uusi_asema = *this;
		uusi_asema.paivitaAsema(&move);
		//MinMaxPaluu arvo;
	
		arvo = uusi_asema.minimax(syvyys - 1, alpha,beta, !maximizing,_siirtovuoro,false);
	
		if (arvo._evaluointiArvo >= paluu._evaluointiArvo) {
			paluu._evaluointiArvo = arvo._evaluointiArvo;
			paluu._parasSiirto = move;
			
			
		}
	}
	return paluu;
}

MinMaxPaluu Asema::quiescence(MinMaxPaluu alpha, MinMaxPaluu beta, int& color, std::vector<Siirto> lista) {
	MinMaxPaluu paluu;
	paluu._evaluointiArvo = evaluoi(color, lista);
	if (paluu._evaluointiArvo >= beta._evaluointiArvo) {
		return beta;
	}
	if (paluu._evaluointiArvo > alpha._evaluointiArvo) {
		alpha._evaluointiArvo = paluu._evaluointiArvo;
	}

	if (lista.size() == 0) {
		return paluu; //quiescence search instead of returning paluu. not sure if works properly
	}

	//sort
	for (Siirto &move : lista) { //use & to not make copies
		Asema asema = *this;
		asema.paivitaAsema(&move);

		paluu = asema.quiescence(alpha, beta, color, lista);

		if (paluu._evaluointiArvo >= beta._evaluointiArvo) {
			return beta;
			//killer moves?
		}

		if (paluu._evaluointiArvo > alpha._evaluointiArvo) {
			alpha._evaluointiArvo = paluu._evaluointiArvo;
		}
	}
	return alpha;


}
void getKingPosition(Asema &asema, int siirtovuoro,Ruutu &kuningas);


MinMaxPaluu Asema::minimax(int syvyys, MinMaxPaluu alpha, MinMaxPaluu beta, bool maximizing, int color, bool nullmove)
{
	
	//std::copy(&_lauta[0][0], &_lauta[0][0] + 8 * 8, &lauta_kopio[0][0]);
	
	total += 1;
	MinMaxPaluu paluu;
	std::vector<Siirto> lista;
	lista.reserve(50); //makes it much faster when we reserve some memory
	annaLaillisetSiirrot(lista);
	orderByCaptures(lista);
	//sort moves by captures etc
	//kantatapus = peli on loppu
	//todo: tutki kumpi voitti ja palauta arvo
	//ei sis‰ll‰ tasapelin tarkistusta
	if (lista.size() == 0) {
		
		
		if (maximizing)paluu._evaluointiArvo = -1000000; 
		else paluu._evaluointiArvo = 10000000;

		return paluu;
	}

	if (syvyys == 0) {
		
		paluu._evaluointiArvo = evaluoi(color, lista); //added
	
		return paluu;
	}
	//null move. need some additional checks. e.g in end game
	if (!nullmove && syvyys >= 3) {
		Ruutu r;
		Asema uus = *this;
		getKingPosition(uus, _siirtovuoro, r);
		bool uhattu;
		if (r.getX() == -1) uhattu = true;
		else uhattu = onkoRuutuUhattu(&r, !_siirtovuoro, *this);



		if (!uhattu) {
			uus.setSiirtovuoro(!_siirtovuoro);
			MinMaxPaluu beta2;
			beta2._evaluointiArvo = beta._evaluointiArvo - 1; //-1 or +1?

			paluu = uus.minimax(syvyys - 1 - 2, beta, beta2, !maximizing, color, true);

			if (paluu._evaluointiArvo >= beta._evaluointiArvo) {


				return beta;
			}
		}

	}
	//null move



	if (maximizing) {
		//std::wcout << 1;
		paluu._evaluointiArvo = -1000000;

		
		for (auto& move : lista) {

			Asema uusi_asema = *this;


			uusi_asema.paivitaAsema(&move);

			MinMaxPaluu arvo = uusi_asema.minimax(syvyys - 1, alpha, beta,!maximizing,color, false);
			//if (arvo._evaluointiArvo != NULL)std::wcout <<"nyt o" << arvo._evaluointiArvo <<"\n";
			
			
			paluu._evaluointiArvo = std::max(arvo._evaluointiArvo, paluu._evaluointiArvo);
			alpha._evaluointiArvo = std::max(alpha._evaluointiArvo, paluu._evaluointiArvo);
			if (beta._evaluointiArvo < alpha._evaluointiArvo) {
				//std::wcout << beta._evaluointiArvo << "beta" << alpha._evaluointiArvo << "alpha" << paluu._evaluointiArvo <<"paluu" <<"\n";
				return paluu;
				
			}
			
			
			
		}
		return paluu;
	}
	else {
		paluu._evaluointiArvo = 1000000;

		for (auto& move : lista) {
		
			Asema uusi_asema = *this;

			


			uusi_asema.paivitaAsema(&move);
		
			
			MinMaxPaluu arvo = uusi_asema.minimax(syvyys - 1, alpha, beta,!maximizing,color,false);

			paluu._evaluointiArvo = std::min(arvo._evaluointiArvo, paluu._evaluointiArvo);
			beta._evaluointiArvo = std::min(beta._evaluointiArvo, paluu._evaluointiArvo);
		
			if (alpha._evaluointiArvo > beta._evaluointiArvo) {
				//std::wcout << "removed";
				return paluu;
			}

			
		}
		return paluu;
	}
	
		
}

void Asema::orderByCaptures(std::vector<Siirto>& lista) {
	int pointer = 0;
	for (int i = 1; i < lista.size(); i++) {
		if (!lista[i].onkoLyhytLinna() && !lista[i].onkoPitk‰linna()) {
			if (_lauta[lista[i].getLoppuruutu().getY()][lista[i].getLoppuruutu().getX()] != nullptr) {
				Siirto temp = lista[pointer];
				lista[pointer] = lista[i];
				lista[i] = temp;
				pointer++;

			}
		}
	}
}


bool Asema::onkoRuutuUhattu(Ruutu* ruutu, int vastustajanVari, Asema &asema)
{
	int kingX = ruutu->getX();
	int kingY = ruutu->getY();
	//torni liikkeet
	int queen;
	int n;
	if (_siirtovuoro == 0) {
		queen = MD;
		
		n = MT;
	}
	else {
		n = VT;
		queen = VD;
	}
	for (int x = kingX + 1; x < 8; x++) {
		if (asema._lauta[kingY][x] != nullptr) {
			if (asema._lauta[kingY][x]->getVari() == vastustajanVari) {
				int koodi = asema._lauta[kingY][x]->getKoodi();
				
				if (koodi == n || koodi == queen) {
					
					return true;
				}
			}
			break;
		}
	}

	for (int x = kingX - 1; x > -1; x--) {
		if (asema._lauta[kingY][x] != nullptr) {
			if (asema._lauta[kingY][x]->getVari() == vastustajanVari) {
				int koodi = asema._lauta[kingY][x]->getKoodi();
				if (koodi == n || koodi == queen) {
					return true;
				}
			}
			break;
		}
	}
	for (int x = kingY - 1; x > -1; x--) {
		if (asema._lauta[x][kingX] != nullptr) {
			if (asema._lauta[x][kingX]->getVari() == vastustajanVari) {
				int koodi = asema._lauta[x][kingX]->getKoodi();
				if (koodi == n || koodi == queen) {
					return true;
				}
			}
			break;
		}
	}
	
	for (int x = kingY + 1; x <8; x++) {
		if (asema._lauta[x][kingX] != nullptr) {
			if (asema._lauta[x][kingX]->getVari() == vastustajanVari) {
			
				int koodi = asema._lauta[x][kingX]->getKoodi();
				if (koodi == n || koodi == queen) {
					return true;
				}
			}
			break;
		}
	}

	//l‰hetti
	
	if (_siirtovuoro == 0) {
		n = ML;
		
	}
	else {
		n = VL;
		
	}
	

	int x = kingX;
	int y = kingY;
	//left of piece
	int countY = y - 1;
	for (int count = x - 1; count >= 0; count--) {
		if (countY < 0)break;
		if (asema._lauta[countY][count] != nullptr) {
			if (asema._lauta[countY][count]->getVari() == vastustajanVari) {
				int koodi = asema._lauta[countY][count]->getKoodi();
				if (koodi == n || koodi ==queen )
					return true;

				
			}
			break;
		}
		countY--;
	}
	
	countY = y + 1;
	//right of piece
	for (int count = x + 1; count < 8; count++) {
		if (countY > 7)break;
		if (asema._lauta[countY][count] != nullptr) {
			if (asema._lauta[countY][count]->getVari() == vastustajanVari)
			{
				int koodi = asema._lauta[countY][count]->getKoodi();
				if (koodi == n || koodi == queen)
					return true;
			}
			break;
		}
		countY++;
	}
	countY = y - 1;
	//viistoon alas
	for (int count = x + 1; count < 8; count++) {
		if (countY < 0)break;
		if (asema._lauta[countY][count] != nullptr) {
			if (asema._lauta[countY][count]->getVari() == vastustajanVari) {
				int koodi = asema._lauta[countY][count]->getKoodi();
				if (koodi == n || koodi == queen)
					return true;
			}
			break;
		}
		countY--;
	}
	countY = y + 1;
	//viistoon ylˆs
	for (int count = kingX - 1; count >= 0; count--) {
		if (countY > 7)break;
		if (asema._lauta[countY][count] != nullptr) {
			if (asema._lauta[countY][count]->getVari() == vastustajanVari) {
				int koodi = asema._lauta[countY][count]->getKoodi();
				//std::wcout << countY << count;
				if (koodi == n || koodi == queen) {
					return true;
				}
					
			}
			break;
		}
		countY++;
	}

	//kuningas //tartteeko?.tarttee + sotilas. sotilas ei toimi oikein viel‰

	int startPosX = kingX;
	int startPosY = kingY;
	int s;
	if (_siirtovuoro == 0) {
		n = MK;
		s = MS;
	}
	else {
		n = VK;
		s = VS;
	}


	if (startPosX < 7) {
		if (asema._lauta[startPosY][startPosX + 1] != nullptr) {
			if (asema._lauta[startPosY][startPosX + 1]->getVari() == vastustajanVari)
			{
				
				if (asema._lauta[startPosY][startPosX+1]->getKoodi() == n)
					return true;
			}
				
		}

	}

	if (startPosX < 7 && startPosY < 7) {
		if (asema._lauta[startPosY + 1][startPosX + 1] != nullptr) {
			if (asema._lauta[startPosY + 1][startPosX + 1]->getVari() == vastustajanVari)
			{
				if (asema._lauta[startPosY+1][startPosX+1]->getKoodi() == n || (s==MS && asema._lauta[startPosY+1][startPosX+1]->getKoodi() == s))
					return true;
			}
		}

	}

	if (startPosY < 7) {
		if (asema._lauta[startPosY + 1][startPosX] != nullptr) {
			if (asema._lauta[startPosY + 1][startPosX]->getVari() == vastustajanVari)
				if (asema._lauta[startPosY+1][startPosX]->getKoodi() == n)
					return true;
		}
	}
	if (startPosX > 0) {
		if (asema._lauta[startPosY][startPosX - 1] != nullptr) {
			if (asema._lauta[startPosY][startPosX - 1]->getVari() == vastustajanVari)
				if (asema._lauta[startPosY][startPosX-1]->getKoodi() == n)
					return true;
		}
	}

	if (startPosX > 0 && startPosY > 0) {
		if (asema._lauta[startPosY - 1][startPosX - 1] != nullptr) {
			if (asema._lauta[startPosY - 1][startPosX - 1]->getVari() == vastustajanVari)
			{
				
				if (asema._lauta[startPosY-1][startPosX-1]->getKoodi() == n ||(asema._lauta[startPosY-1][startPosX-1]->getKoodi() == VS))
					return true;
			}
		}
	}

	if (startPosY > 0) {
		if (asema._lauta[startPosY - 1][startPosX] != nullptr) {
			if (asema._lauta[startPosY - 1][startPosX]->getVari() == vastustajanVari)
				if (asema._lauta[startPosY-1][startPosX]->getKoodi() == n)
					return true;
		}
	}

	if (startPosY > 0 && startPosX < 7) {
		if (asema._lauta[startPosY - 1][startPosX + 1] != nullptr) {
			if (asema._lauta[startPosY - 1][startPosX + 1]->getVari() == vastustajanVari)
				if (asema._lauta[startPosY-1][startPosX+1]->getKoodi() == n || asema._lauta[startPosY-1][startPosX+1]->getKoodi() == VS)
					return true;
		}
	}

	if (startPosY < 7 && startPosX > 0) {
		if (asema._lauta[startPosY + 1][startPosX - 1] != nullptr) {
			if (asema._lauta[startPosY + 1][startPosX - 1]->getVari() == vastustajanVari)
				if (asema._lauta[startPosY+1][startPosX-1]->getKoodi() == n || asema._lauta[startPosY+1][startPosX-1]->getKoodi() == MS)
					return true;
		}
	}
	
	//hevonen

	int iV[8] = { 2,2,1,1,-1,-1,-2,-2 };
	int jV[8] = { 1,-1,2,-2,-2,2,1,-1 };

	if (_siirtovuoro == 0) {
		n = MR;
	}
	else n = VR;

	
	for (int iter = 0; iter < 8; iter++) {
		int xPos = kingX + iV[iter];
		int yPos = kingY + jV[iter];
		if (xPos > -1 && yPos > -1 && xPos < 8 && yPos < 8) {
			Nappula* tarkista = asema._lauta[yPos][xPos];
			if (tarkista != nullptr) {
				if (tarkista->getVari() == vastustajanVari) {
						if (tarkista->getKoodi() == n) {
							return true;
						}
						
					
				}


			}
		}
	}

	return false;
}


void tarkistaLinnoituksetValkea(Asema &asema, std::vector<Siirto>&lista) {
	if (!asema.getOnkoValkeaKuningasLiikkunut()) {
		if (!asema.getOnkoValkeaDTliikkunut()) {
			if (asema._lauta[0][0] == asema.vt && asema._lauta[0][1] == nullptr && asema._lauta[0][2] == nullptr && asema._lauta[0][3] == nullptr)
			lista.push_back(Siirto(false, true));
		}
		if (!asema.getOnkoValkeaKTliikkunut()) {
			
			if (asema._lauta[0][5] == nullptr && asema._lauta[0][6] == nullptr &&asema._lauta[0][7] == asema.vt)
			lista.push_back(Siirto(true, false));
		}
	}
}

void tarkistaLinnoituksetMusta(Asema& asema, std::vector<Siirto>& lista) {
	if (!asema.getOnkoMustaKuningasLiikkunut()) {
		if (!asema.getOnkoMustaDTliikkunut()) {
			if(asema._lauta[7][0] == asema.mt && asema._lauta[7][1] ==nullptr && asema._lauta[7][2] == nullptr && asema._lauta[7][3] == nullptr )
			lista.push_back(Siirto(false, true));
		}
		 if (!asema.getOnkoMustaKTliikkunut()) {
			if (asema._lauta[7][7] == asema.mt &&asema._lauta[7][5] == nullptr && asema._lauta[7][6] == nullptr)
			lista.push_back(Siirto(true, false));
		}
	}
}

void getMovesByPiece(Asema& asema, int vari, std::vector<Siirto> lista) {
	lista.reserve(50);
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			if (asema._lauta[i][j] != nullptr) {

				if (vari == 0) {
					if (asema._lauta[i][j]->getKoodi() == VK) {
				
					}
				}
				else {
					if (asema._lauta[i][j]->getKoodi() == MK) {
						
					}
				}
				if (asema._lauta[i][j]->getVari() != vari) {
					Ruutu r(i, j);
					asema._lauta[i][j]->annaSiirrot(lista, &r, &asema, !vari);
				
				}
			}
		}
	}
}
void getAvailableMoves(Asema& asema,std::vector<Siirto>& lista, int vari, Ruutu& kuningas) {
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			if (asema._lauta[i][j] != nullptr) {

				if (vari== 0) {
					if (asema._lauta[i][j]->getKoodi() == VK) {
					//	std::wcout << "x oli" << j << " ja y oli" << i << "\n";
						kuningas.setRivi(j);
						kuningas.setSarake(i);
					}
				}
				else {
					if (asema._lauta[i][j]->getKoodi() == MK) {
						kuningas.setRivi(j);
						kuningas.setSarake(i);
					}
				}
				if (asema._lauta[i][j]->getVari() != vari) {
					Ruutu r(i, j);
					asema._lauta[i][j]->annaSiirrot(lista, &r, &asema, !vari);

				}
			}
		}
	}
}

void getKingPosition(Asema& asema, int vari, Ruutu& kuningas) { //could only search positions next to old king position maybe?
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			if (asema._lauta[i][j] != nullptr) {

				if (vari == 0) {
					if (asema._lauta[i][j]->getKoodi() == VK) {
						//	std::wcout << "x oli" << j << " ja y oli" << i << "\n";
						kuningas.setRivi(j);
						kuningas.setSarake(i);
						return;
					}
				}
				else {
					if (asema._lauta[i][j]->getKoodi() == MK) {
						kuningas.setRivi(j);
						kuningas.setSarake(i);
						return;
					}
				}
			}
		}
	}
	kuningas.setSarake(-1);
	kuningas.setRivi(-1);
}



void alternateMovesTest(Asema& asema, std::vector<Siirto>&lista, int vari) {
	Ruutu kuningas;
	bool valid = true;
	int kingX;
	int kingY;
	for (auto i = lista.begin(); i != lista.end();) {
		valid = true;
		
		

	}
	
}


void copyState(Nappula* board[8][8], Nappula* newBoard[8][8]) {
	for (int y = 0; y < 8; y++) {
		for (int x = 0; x < 8; x++) {
			if (newBoard[y][x] != nullptr) {

				board[y][x] = newBoard[y][x];
			}
			else {
				board[y][x] = nullptr;
			}
		}
	}
}


//koodi muutettu huomattavasti nopeammaksi. ~10x nopeampi. aseman kopiointia voi viel‰ nopeuttaa. esim. tallentamalla koordinaatit ja asettamalla vain nappuloiden koordinaatit takaisin siirron vaihtuessa.
void Asema::huolehdiKuninkaanShakeista(std::vector<Siirto>& lista, int vari)
{
	Ruutu kuningas;

	bool valid = true;
	int size = lista.size();
	int siirtoVuoro = getSiirtovuoro();

	getKingPosition(*this, siirtoVuoro, kuningas);
	if (kuningas.getX() == -1)return; //should not be necessary
	//Nappula* board[8][8];
	//copyState(board,_lauta);
	//std::copy(&_lauta[0][0], &_lauta[0][0] + 8 * 8, &board[0][0]);
	Asema uusiAsema = *this;
	Ruutu origin = kuningas;
	for (auto i = lista.begin(); i != lista.end();) { 
		valid = true;
	
		uusiAsema.paivitaAsemaVirtual(&i.operator*());
	
		//if (!_siirtovuoro)tarkistaLinnoituksetValkea(*this, vastustajanSiirrot); //ei viel‰ linnoituksia. pit‰‰ lis‰t‰. pit‰‰ my
		//else tarkistaLinnoituksetMusta(*this, vastustajanSiirrot);

		//only update king position if king moved
		kuningas = origin;
		auto x = i.operator*();
		if (x.onkoLyhytLinna() || x.onkoPitk‰linna()) {
			getKingPosition(uusiAsema, siirtoVuoro, kuningas);
			
		}
		else {
			auto alkuRuutu = x.getAlkuruutu();
			if (siirtoVuoro == 0 && _lauta[alkuRuutu.getY()][alkuRuutu.getX()] == vk)
				getKingPosition(uusiAsema, siirtoVuoro, kuningas);
			else if (siirtoVuoro == 1 && _lauta[alkuRuutu.getY()][alkuRuutu.getX()] == mk)getKingPosition(uusiAsema, siirtoVuoro, kuningas);
			
		}

	
		
		if (onkoRuutuUhattu(&kuningas, !siirtoVuoro, uusiAsema)) {
			//std::wcout << "\n"<<i.operator*().getAlkuruutu().getY() << i.operator*().getAlkuruutu().getX() <<" ruutuun" << i.operator*().getLoppuruutu().getY() << i.operator*().getLoppuruutu().getX();
			i = lista.erase(i);
			valid = false;
		}
		
		//kopioi laudan alkuper‰isen tilanteen eli poistaa tehdyn siirron. hieman hidas
		//std::copy(&board[0][0], &board[0][0] + 8 * 8, &_lauta[0][0]);
		//copyState(_lauta, board, true);
		uusiAsema = *this;
		if (valid)
		i++;

	}

}

//etsii kaikki sallitut siirrot ja lis‰‰ ne listaan.
void Asema::annaLaillisetSiirrot(std::vector<Siirto>& lista) {
	int siirtoVuoro = getSiirtovuoro();
	

	//omat siirrot
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			if (_lauta[i][j] != nullptr) {
				if (_lauta[i][j]->getVari() == siirtoVuoro) {
					Ruutu r(i, j);
					_lauta[i][j]->annaSiirrot(lista, &r, this, siirtoVuoro);

				}
			}
		}
	}
	//linnoitus
	if (!_siirtovuoro)tarkistaLinnoituksetValkea(*this, lista);
	else tarkistaLinnoituksetMusta(*this, lista);

	//muodostaa vastustajan siirtolistan joka tehdylle siirrolle ja katsoo jos kuningas on uhattuna
	huolehdiKuninkaanShakeista(lista, !siirtoVuoro); //removed for testing because of how slow it is. will rework parts of it later

}
