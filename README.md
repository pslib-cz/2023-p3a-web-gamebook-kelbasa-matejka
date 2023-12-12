# Gamebook: Escape The Fortress
<p>Odehrávající se ve fantasy světě, konkrétně v "opuštěné" pevnosti, ze které se pokusí hráč dostat strastiplnou cestou skrz katakomby ven na svobodu.</p>
<p>Hráč bude moct poznat v jakém podlaží je podle poškození interiéru či enemáků nebude muset koukat jen tupě na hodnoty...</p>
<p>Dokud mě nic lepšího nenapadne, tak hlavní postava bude obyčejná lapka nebo prostě jen chudák co tam skončil.</p>
<p>Hráč by mohl začínat v pekle a končit někde v krásné krajině okolo pevnosti, například hory při západu slunce nebo klidný les => kontrast</p>
<p>Do popisu místností bychom mohli zakomponovat monology hráče na reakci toho kde je. Mohlo by mu vyvolat nějaké vzpomínky (Konflikty, které má sám se sebou, ...)</p>
<p>Postava by měla mít i jinou motivaci proč z pevnosti odejít než jen pud sebezáchovy</p>

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
</ul>

### Mapa 
<ul>
    <li>
        Mapa je rozdělena na cca 7 stages (podlaží), každé podlaží obsahuje 5 místností včetně, hlavní místnosti v které se ocitneme na začátku.
    </li>
    <li>
        Minimálné 1 místnost v podlaží bude náhodně generováná. Pohráváme si s myšlenkou, že by každá místnost mohla být sama o sobě náhodná a to tak, že bychom náhodně vybírali assety => musel by být min. pár assetů, protože ne každý enemy se bude hodit do každé místnosti
    </li>
    <li>Klíče budou sloužit k odemikání místností, nebo ke věcem v místnostech</li>
    <li>Každá lokace má pole connection, při přesunu budeme porovnávat zda id místnosti kam chceme jít je shodná s id _ToLocationID, id budou náhodná</li>
</ul>

### Data
<ul>
    <li>Herní data v JSON nebo CSV</li>
    <li>Uživatelský data v session</li>
    <li>Cookies jen pro sessionID</li>
</ul>

## Figma návrh
<a href="https://www.figma.com/file/QgTaAXxr2krxgQlMT8mOTe/GAMEBOOK?type=design&node-id=0-1&mode=design">zde</a>

## Model tříd
<p><img src="./Assets/GamebookModels.jpg"/></p>


[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/dMUm1NVd)
