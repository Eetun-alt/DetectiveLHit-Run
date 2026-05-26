# Detective L Hit-Run

## Yleiskuvaus
Detective L Hit-Run on taisteluteemainen peli, jossa pelaaja tutkii karttaa ja siirtyy eri sijainteihin taistelemaan.

Pelin tämänhetkiset ominaisuudet:
- Main Menu
- Settings-valikko
- Pelaajan nimen syöttö
- Karttanäkymä
- Sijaintien valinta punaisista nuppineuloista

Kun pelaaja valitsee sijainnin kartalta, avautuu taistelukenttä.

## Suunnitellut ominaisuudet
- 1v1 turn based combat
- Viholliset eri alueilla
- Lisää karttoja ja tehtäviä

Huom: Taistelusysteemi ei ole vielä toteutettu loppuun.

---

## Teknologiat
- Unity
- C#
- GitHub versionhallinta

---

## Projektin rakenne

### Main Menu
Pelin aloitusvalikko, josta pelaaja voi:
- Aloittaa pelin
- Avata asetukset


### Settings
Asetusvalikko pelin asetusten muuttamiseen.
-äänenvoimakkuus (ei toimi mutta tallentaa arvon.)

### Name System
Pelaaja voi syöttää oman nimensä ennen pelin aloittamista.

### Map System
Pelaaja siirtyy karttaan, jossa eri sijainnit näkyvät punaisina nuppineuloina.

### Battle Area
Kun sijaintia painetaan:
- Pelaaja siirtyy taistelukenttään
- Tulevaisuudessa tarkoitus toteuttaa 1v1 vuoropohjainen taistelu

---

## Käynnistys

Lataa projektin valmis julkaisuversio (Detective_L_Hit_And_Run_Build.zip)

Pura ZIP tiedoston sisältö kokonaan tyhjään kansioon tietokoneella.
Avaa purettu kansio ja käynnistä peli avaamalla tiedosto "Detective L Hit & Run.exe"

---

## Testaus

Testattavat ominaisuudet:
- Main Menu toimii
- Settings avautuu
- Nimen syöttö toimii
- Kartta avautuu
- Sijainteja voi painaa
- Scene vaihtuu kartasta taistelualueeseen

---

## Tiimi
- Aleksi
- Lassi
- Eetu

---

## Versionhallinta
Projektissa käytetään GitHubia versionhallintaan ja tehtävien hallintaan.
