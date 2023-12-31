<h1>Gamebook: Escape The Fortress</h1>
<p>Odehrávající se ve fantasy světě, konkrétně v "opuštěné" pevnosti, ze které se pokusí hráč dostat strastiplnou cestou skrz katakomby ven na svobodu.</p>
<p>Hráč bude moct poznat v jakém podlaží je podle poškození interiéru či enemáků nebude muset koukat jen tupě na hodnoty...</p>
<p>Dokud mě nic lepšího nenapadne, tak hlavní postava bude obyčejný lapka nebo prostě jen chudák co tam skončil.</p>
<p>Hráč by mohl začínat v pekle a končit někde v krásné krajině okolo pevnosti, například hory při západu slunce nebo klidný les => kontrast</p>
<p>Do popisu místností bychom mohli zakomponovat monology hráče na reakci toho kde je. Mohlo by mu vyvolat nějaké vzpomínky (Konflikty, které má sám se sebou, ...)</p>
<p>Postava by měla mít i jinou motivaci proč z pevnosti odejít než jen pud sebezáchovy</p>

<img style="display: block;" width=800 src="https://github.com/pslib-cz/2023-p3a-web-gamebook-kelbasa-matejka/assets/91247802/5d8d95cb-33d7-4c81-99cc-c133dfbeeacc">

## Mechaniky
### Hráč
<ul>
    <li>Hráči se při každém zabití protivníka zvyšuje damage, protože má logicky více zkušeností s bojem</li>
    <li>Hráč může získat brnění, to bude přičítat odpor poškození</li>
    <li>Pokud bude nalezeno lepší brnění, tak se to horší automaticky odebere s inventáře, stejně budou fungovat zbraně</li>
    <li>Zbraně násobí damage, brnění přičíta resistance</li>
    <li>Každá akce stojí nějakou energii, hraji jsi s myšlenkou, že se energie budue obnovat po každé úrovni, aby hráče lépe nakládal s energii, pokud dojde, tak se bude energie odečítat z hp</li>
    <li>Pokud hráč zemře, tak hraje úplně od začátku</li>
</ul>

### Souboje
<ul>
    <li>Hráč i protivník mají šanci na blok</li>
    <li>Hráč i protivník mají šanci na kritický zásah</li>
    <li>Na protivníka lze použít item, který ho instakillne</li>
    <li>Lze použít consumables</li>
    <li>Na výběr bude z klasického útoku a silného, silný bude ubírat více než půlku energie hráče</li>
</ul>

### Hádanky
<ul>
    <li>Některé místnosti budou v podstatě hádanky, které následně odemnkou cestu do dalších místností</li>
    <li>Hráč má pokusů kolik jenom chce</li>
</ul>

### Mapa 
<ul>
    <li>
        Mapa je rozdělena na cca 3 stages (podlaží), každé podlaží obsahuje cca 5-10 místností.
    </li>
    <li>
       Pohráváme si s myšlenkou, že by každá místnost mohla být sama o sobě náhodná a to tak, že bychom náhodně vybírali assety => musel by být min. pár assetů, protože ne každý enemy se bude hodit do každé místnosti
    </li>
    <li>Klíče budou sloužit k odemčení spojení mezi místnostmi. Pokud hráč nebude mít požadovaný item, tak danný connection nemůže použít.</li>
    <li>Každá lokace má pole connection, při přesunu budeme porovnávat zda id místnosti kam chceme jít je validní vzhlek k CurrentLocationID, které je zapsané v modelu hráče.</li>
</ul>

#### Podvádění
<ul>
    <li>Podvádění pomocí přepsání id lokace v adresním řádku nebude možné, jelikož dané přesměrování bude kontrolováno. Pokud bude požadované přesměrování vyhodnoceno jakožto nevalidní tak hráč bude přesměrován do lokace, ve které byl naposledy</li>
    <li>Pokud se hráč pokusí dostat na konec hry přepsáním adresního řádku, tak bude místo toho přesměrován na úvodní stránku a veškerá jeho herní data uložená v session budou smazána</li>
</ul>

### Data
<ul>
    <li>Herní data v jednotlivých JSON souborech</li>
    <li>Data hráče, lokací a connections v session</li>
</ul>

## Figma návrh
<a href="https://www.figma.com/file/QgTaAXxr2krxgQlMT8mOTe/GAMEBOOK?type=design&node-id=0-1&mode=design">zde</a>

## Model tříd
<p><img src="./Assets/GamebookModels.jpg"/></p>


[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/dMUm1NVd)
