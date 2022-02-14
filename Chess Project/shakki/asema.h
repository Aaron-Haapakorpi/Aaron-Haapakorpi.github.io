#pragma once

#include <list>
#include <string>
#include "minmaxpaluu.h"
#include "siirto.h"
#include <deque>
#include <vector>
#include <stack>


// Ns. "forward declaration". Nyt Asema-luokassa voidaa esitell‰ Nappula-osoittimia ilman,
// ett‰ nappula.h -tiedostoa t‰ytyy includoida.
class Nappula;


// Asema sis‰lt‰‰ kaiken tarvittavan informaation pelitilanteen kuvaamiseksi
// (nappuloiden sijainti, siirtovuoro, linnoitusoikeudet jne.).
class Asema
{

public:
	// Pelilauta sis‰lt‰‰ osoittimet kunkin ruudun nappula-olioon (nullptr/NULL/0 jos ruutu on tyhj‰).
	// Public-m‰‰reell‰, koska t‰t‰ k‰ytet‰‰n paljon muualla.
	Nappula* _lauta[8][8];

	// Nappula-oliot. Huomaa, ett‰ samaa nappulaa voidaan k‰ytt‰‰ useissa eri ruuduissa.
	// M‰‰ritelty static-m‰‰reell‰, joten nappulat ovat kaikkien lauta-olioiden "yhteisk‰ytˆss‰"
	// (suorituskyvyn vuoksi).
	static Nappula* vk, * vd, * vt, * vl, * vr, * vs;	// Valkeat nappulat.
	static Nappula* mk, * md, * mt, * ml, * mr, * ms;	// Mustat nappulat.
	// Ohestalyˆnti‰ varten (-1 = sotilaan kaksoisaskelta ei tapahtunut edellisell‰ siirrolla).
	int kaksoisaskelSarakkeella = -1;

	int arraySize = 0;

	Asema();												// Asettaa alkuaseman.
	void paivitaAsema(Siirto*);								// P‰ivitt‰‰ aseman annetulla siirrolla.
	void paivitaAsemaVirtual(Siirto*);
	double evaluoi(int color, std::vector<Siirto>&lista);										// Aseman numeerinen arviointi.
	MinMaxPaluu maxi(int syvyys, MinMaxPaluu& alpha, MinMaxPaluu& beta, bool maximizing);							// Minimax (max:n siirtovuoro).
	MinMaxPaluu minimaxRoot(int syvyys,MinMaxPaluu& alpha, MinMaxPaluu& beta, bool maximizing);							// Minimax (min:n siirtovuoro).
	MinMaxPaluu minimax(int syvyys, MinMaxPaluu alpha, MinMaxPaluu beta, bool maximizing, int color, bool nullMove);						// Minimax-algoritmi.
	MinMaxPaluu negaMax(int syvyys);//negamax
	MinMaxPaluu quiescence(MinMaxPaluu alpha, MinMaxPaluu beta, int&color, std::vector<Siirto> lista);
	void orderByCaptures(std::vector<Siirto>&);
	void annaLaillisetSiirrot(std::vector<Siirto>& lista);	// Siirtogeneraattori. k‰yt‰ static arrayt‰ jotta nopeampi
	int getSiirtovuoro();									// Palauttaa siirtovuoron.
	void setSiirtovuoro(int);								// Asettaa siirtovuoron.
	bool getOnkoValkeaKuningasLiikkunut();					// Linnoittuminen mahdollista?
	bool getOnkoMustaKuningasLiikkunut();					// Linnoittuminen mahdollista?
	bool getOnkoValkeaDTliikkunut();						// Linnoittuminen mahdollista?
	bool getOnkoValkeaKTliikkunut();						// Linnoittuminen mahdollista?
	bool getOnkoMustaDTliikkunut();							// Linnoittuminen mahdollista?
	void annaSiirtoCount(int vari);
	bool getOnkoMustaKTliikkunut();							// Linnoittuminen mahdollista?

private:

	// Lis‰informaatio pelitilanteesta.
	int _siirtovuoro;					// 0 = valkea, 1 = musta.
	bool _onkoValkeaKuningasLiikkunut = false;	// Linnoitus ei ole sallittu, jos kuningas on liikkunut.
	bool _onkoMustaKuningasLiikkunut = false;// Linnoitus ei ole sallittu, jos kuningas on liikkunut.
	bool _onkoValkeaDTliikkunut = false;		// Linnoitus ei ole sallittu, jos daamisivustan torni on liikkunut.
	bool _onkoValkeaKTliikkunut = false;		// Linnoitus ei ole sallittu, jos kuningassivustan torni on liikkunut.
	bool _onkoMustaDTliikkunut = false;			// Linnoitus ei ole sallittu, jos daamisuvustan torni on liikkunut.	
	bool _onkoMustaKTliikkunut = false;			// Linnoitus ei ole sallittu, jos kuningassivustan torni on liikkunut.

	double laskeNappuloidenArvo(int);
	bool onkoAvausTaiKeskipeli(int);
	double nappuloitaKeskella(int);
	double linjat(int);
	bool onkoRuutuUhattu(Ruutu*, int vastustajanVar,Asema &asema);
	void annaLinnoitusSiirrot(std::vector<Siirto>& lista, int vari);

	// Karsii siirrot, jotka j‰tt‰v‰t oman K:n shakkiin.
	void huolehdiKuninkaanShakeista(std::vector<Siirto>& lista, int vari);
};