# CarcassonneDiscovery.Entity

V tomto projektu VS se nach�z� pomocn� n�stroje pro manipulaci s hern�mi prvky.


## Extension metody

### `CoordsExtensions`

T��da `CoordsExtensions` obsahuje extension metody pro strukturu `Coords`.

Je zde implementovan� metoda `GetNeighboringCoords` pro z�sk�n� sou�adnice sousedn�ho pol��ka podle dan�ho sm�ru. 

### `TileOrientationExtensions`

T��da `TileOrientationExtensions` obsahuje extension metody pro strukturu `TileOrientation`.

Jsou zde implementovan� metody `Rotate` a `Derotate` pro z�sk�n� orientace karti�ky oto�en� o dan� �hel.

### `ITileSchemeExtensions`

T��da `ITileSchemeExtensions` obsahuje extension metody pro rozhran� `ITileScheme`.

Je zde implementovan� metoda `TileEquals` pro porovn�n� karti�ek
na ekvivalenci, tedy zda nesou toto�nou informaci (bez ohledu na vnit�n� implementaci).

D�le je implementovan� metody `IsConsistent` pro zji�t�n� konistence karti�ek.
Kontroluje, zda existuj� m�sta a regiony podle ur�en�ho po�tu, zda si navz�jem odpov�daj�
soused�c� regiony a zda jsou spr�vn� definov�ny regiony na hranic�ch karti�ky (pr�v� jeden
region na ka�d� hranici a konzistence metod `GetRegionBorders` � `GetRegionOnBorder`).

## Standardn� implementace karti�ek

T��da `TileScheme` p�edstavuje obecnou implementaci rozhran� `ITileScheme`.
Jednotliv� informace o regionech, kter� jsou z�sk�v�ny metodami `ITileScheme`,
jsou ulo�eny v instanc�ch `RegionInfo`.

## Prohled�v�n� na hern� desce

TODO: popis, graf karti�ek, implementovan� metody