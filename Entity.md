# CarcassonneDiscovery.Entity

V tomto projektu VS se nach�z� objekty reprezentuj�c� hern� prvky.

## V��tov� typy

### `TileOrientation`

V��tov� typ `TileOrientation` reprezentuje orientaci karti�ky, pozici na okraji karti�ky nebo sm�r ukazuj�c� na sousedn� karti�ky.
Jednotliv� prvky ozna�uj� sv�tov� strany (sever `N`, v�chod `E`, jih `S`, z�pad `W`).

Je pou�ita bitov� logika, je tedy mo�n� zapsat v�ce sm�r� nar�z (nap�. `NE` reprezentuje z�rove� sever a v�chod). ��dn� sm�r je reprezentov�n prvkem `None`.

### `RegionType`

V��tov� typ `RegionType` reprezentuje typ regionu na karti�ce (hory, n�iny a mo�e).

### `PlayerColor`

V��tov� typ `PlayerColor` reprezentuje barvu figurek hr��e. Zvl�tn� barva `PlayerColor.None` je pou�ita p�i akc�ch, kter� nejsou iniciov�ny ��dn�m hr��em.

Pomoc� barev jsou hr��i identifikov�ni pro ��ely hry.

### `MoveWorkflow`

V��tov� typ `MoveWorkflow` reprezentuje stav tahu.
TODO (p�i revizi dokumentace): Graf s popisem hern�ch f�z�


## Struktury, t��dy a rozhran�

### `ITileScheme`

Rozhran� `ITileScheme` reprezentuje vzhled karti�ky.
Vzhled karti�ek je ur�en po�tem a strukturou region� a m�st.

Je nutn� implementovat n�sleduj�c� metody a property:
- zji�t�n� po�tu region� a m�st,
- zji�t�n�, kter� region se nach�z� na dan� hranici,
- dotazy na konkr�tn� region (podle jeho identifik�toru):
   - typ regionu,
   - seznam n�le��c�ch m�st,
   - seznam sousedn�ch region�,
- identifik�tor karti�ky uvnit� hern� sady (string s libovolnou hodnotou, podle dan� hern� sady).

### `ITileSupplier`

Rozhran� `ITileSupplier` reprezentuje poskytov�n� karti�ek pro hru.

Poskytuje:
- po�et zb�vaj�c�h karti�ek,
- prvn� karti�ku ve h�e,
- n�sleduj�c� karti�ku,
- mo�nost vr�cen� karti�ky do sady (pokud nap�. nelze umstit).

### `GameParams`

T��da `GameParams` reprezentuje parametry hry.

Parametry hry zahrnuj�:
- po�et hr���,
- po�et figurek pro ka�d�ho hr��e,
- po�ad� hr��� ve h�e,
- informace o sad� karti�ek.

### `TileSetParams`

T��da `TileSetParams` reprezentuje parametry pou�it� sady karti�ek.

Parametry sady karti�ek zahrnuj�:
- n�zev sady karti�ek.

### `Coords`

Struktura `Coords` reprezentuje sou�adnice karti�ky (nebo dal��ch hern�ch prvk�) na hern� desce.

### `GameState`

T��da `GameState` reprezentuje aktu�ln� stav hry.

Stav hry zahrnuje:
- parametry hry (v�. poskytovatele karti�ek),
- desku s um�st�n�mi karti�kami a figurkami,
- aktu�ln� f�zi tahu, hr��e na tahu a vylosovanou karti�ku pro tento tah,
- aktu�ln� sk�re hr���.

### `TilePlacement`

T��da `TilePlacement` reprezentuje um�st�n� karti�ky na desce.

Um�st�n� karti�ky zahrnuje:
- sou�adnice karti�ky,
- vzhled a orientaci karti�ky,
- informaci o um�st�n� figurce na karti�ce.

### `FollowerPlacement`

T��da `FollowerPlacement` reprezentuje um�st�n� figurky na desce.

Um�st�n� figurky zahrnuje:
- barvu figurky,
- sou�adnice karti�ky, na kter� je figurka um�st�na,
- identifik�tor regionu, kde je figurka um�st�na.