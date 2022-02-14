#include <iostream>
#include <Windows.h>
#include <io.h>
#include <fcntl.h>
#include <iostream>
#include <string>
#include <conio.h>
#include <chrono>
#include <thread>
#include "kayttoliittyma.h"
#include "asema.h"

bool getChar = false;
bool botValkoinen = true;
bool botMusta = true;
int evaValkoinen = 3;
int evaMusta = 3;

using namespace std;
using namespace std::this_thread;
using namespace std::chrono;

void testSiirtoGenerator(int y, int x, int vari, Asema* asema) {
	Ruutu ruutu(y, x);
	vector<Siirto> lista;
	asema->_lauta[y][x]->annaSiirrot(lista, &ruutu, asema, vari);
	for (auto s : lista) {
		wcout << "siirto" << s.getLoppuruutu().getY() << s.getLoppuruutu().getX() << "\n";

	}
}

void testAllMoves(Asema* asema) {
	vector<Siirto> lista;
	asema->annaLaillisetSiirrot(lista);
	int i = 0;
	//wcout << "täs o lista";
	for (auto s : lista) {
		i++;
		if (!s.onkoLyhytLinna() && !s.onkoPitkälinna()) {
		wcout << asema->_lauta[s.getAlkuruutu().getY()][s.getAlkuruutu().getX()]->getUnicode();
		wcout << " siirto " << s.getLoppuruutu().getY() << s.getLoppuruutu().getX();
		wcout << " ruudusta " << s.getAlkuruutu().getY() << s.getAlkuruutu().getX() <<"\n";
		}
		else {
			wcout << "linnoitus! " << "\n";
		}
	}
	//wcout << "number of moves was " << i << "\n";
}

int main()
{
	char x;
	wcout << "Asetetaanko getch paalle? (suositellaan vain kone vs kone peleihin)\nY = paalle, muuten pois.\n? Avaa lisatietoja.\n";
	cin >> x;

	if (x == 'Y') {
		getChar = true;
	}
	else if (x == '?') {
		wcout << "Getch tarkoittaa sita, etta peli odottaa kayttajan painavan nappia edetakseen.\nPohjaan painaessa tai getchin ollessa pois paalta, peli etenee automaattisesti.\n\n";
		wcout << "Katkaisusyvyys tarkoittaa sita, kuinka monta siirtoa eteenpain tietokone laskee.\nSiirrot laskettuaan, tietokone valitsee parhaan vaihtoehdon.\n";
		wcout << "Mita isompi katkaisusyvyys sita kauemmin kestaa. Suurinosa koneista laskee 1-3 lahes valittomasti.\n5-6 ja niita isommat luvut tarvitsevat huomattavasti pidempia aikoja laskuihin.\n\n";
		wcout << "Jos shakkinappulat eivat nay oikein, varmista fonttisi tukevan shakkimerkkeja.\nItse suosittelemme MS Gothic fonttia.\n(right clickaa konsolia -> properties/ominaisuudet -> font/fontti -> font/fontti ruudussa MS Gothic)\n\n";
		wcout << "Asetetaanko getch paalle? (Y = paalle, muuten pois) \n";
		cin >> x;
		if (x == 'Y') {
			getChar = true;
		}
		else {
			getChar = false;
		}

	}
	else if(x != 'Y' && x != '?'){
		getChar = false;
	} else {
	
	}

	wcout << "Onko valkoinen tietokone vai pelaaja? (K/P)\n";
	cin >> x;
	if (x == 'P') {
		botValkoinen = false;
	}
	else {
		botValkoinen = true;
	}

	wcout << "Onko musta tietokone vai pelaaja? (K/P)\n";
	cin >> x;
	if (x == 'P') {
		botMusta = false;
	}
	else {
		botMusta = true;
	}

	if (botValkoinen == true) {
		wcout << "Anna 1-9 valkoisen katkaisusyvyydeksi (suositellaan 5 tai vahemman) \n";
		cin >> evaValkoinen;
	}
	if (botMusta == true) {
	wcout << "Anna 1-9 mustan katkaisusyvyydeksi (suositellaan 5 tai vahemman) \n";
	cin >> evaMusta;
	}

	Asema asema;
	Kayttoliittyma::getInstance()->aseta_asema(&asema);
	Kayttoliittyma::getInstance()->piirraLauta();
	asema.setSiirtovuoro(0);
	MinMaxPaluu alpha;
	MinMaxPaluu beta;
	alpha._evaluointiArvo = -100000;
	beta._evaluointiArvo = 100000;
	MinMaxPaluu test1;

	while (true) {
		if(getChar == true){
		wcout << "Odotetaan painallusta\n";
		getch();
		}
		//testAllMoves(&asema);
		        
		Ruutu r;
		
		vector<Siirto> lista;
		asema.annaLaillisetSiirrot(lista);
		//wcout << "points are" << asema.evaluoi();
		if (lista.size() == 0) {
			wcout << "No moves!\nWait for 4 seconds and then press anything to exit.\n";
			sleep_for(4s);
			break;
		}

		MinMaxPaluu test;
		
		if (asema.getSiirtovuoro() == 0)test = asema.minimaxRoot(evaValkoinen, alpha, beta, true);
		else test = asema.minimaxRoot(evaMusta, alpha, beta, true);
		
		if(asema.getSiirtovuoro() == 0){
		wcout << "Valkoisen pisteet: " << test._evaluointiArvo << endl;
		}
		else {
			wcout << "Mustan pisteet: " << test._evaluointiArvo << endl;
		}

		//wcout << "Siirto on " << test._parasSiirto.getAlkuruutu().getY() << test._parasSiirto.getAlkuruutu().getX()
		//	<< " ruutuun " << test._parasSiirto.getLoppuruutu().getY() << test._parasSiirto.getLoppuruutu().getX() << endl;
		
		if(asema.getSiirtovuoro() == 0){
		wcout << "Valkoisen vuoro";
		}
		else {
			wcout << "Mustan vuoro";
		}

		Siirto siirto;
		if (asema.getSiirtovuoro() == 0) {
			//VALKOISET
			if(botValkoinen == true){
			siirto = test._parasSiirto;
			} else {
			siirto = Kayttoliittyma::getInstance()->annaVastustajanSiirto();
			}
		}
		else {
			//MUSTAT
			if(botMusta == true){
			siirto = test._parasSiirto;
			}
			else {
			siirto = Kayttoliittyma::getInstance()->annaVastustajanSiirto();
			}
			
		}
		//wcout << "\n linnalyh" << siirto.onkoLyhytLinna();
		//wcout << siirto.onkoPitkälinna();
		//wcout << "pisteet ovat " << asema.evaluoi();
		
		if ( (test._evaluointiArvo >500 || test._evaluointiArvo <-500) ) {
			
		}
		asema.paivitaAsema(&siirto);
		//testSiirtoGenerator(siirto.getLoppuruutu().getY(), siirto.getLoppuruutu().getX(), 0, &asema);
		//asema.setSiirtovuoro(0);
		
		Kayttoliittyma::getInstance()->piirraLauta();
		
		
	}

	return 0;
}