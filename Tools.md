# CarcassonneDiscovery.Entity

V tomto projektu VS se nachází pomocné nástroje pro manipulaci s herními prvky.


## Extension metody

### `CoordsExtensions`

Tøída `CoordsExtensions` obsahuje extension metody pro strukturu `Coords`.

Je zde implementovaná metoda `GetNeighboringCoords` pro získání souøadnice sousedního políèka podle daného smìru. 

### `TileOrientationExtensions`

Tøída `TileOrientationExtensions` obsahuje extension metody pro strukturu `TileOrientation`.

Jsou zde implementované metody `Rotate` a `Derotate` pro získání orientace kartièky otoèené o daný úhel.

### `ITileSchemeExtensions`

Tøída `ITileSchemeExtensions` obsahuje extension metody pro rozhraní `ITileScheme`.

Je zde implementovaná metoda `TileEquals` pro porovnání kartièek
na ekvivalenci, tedy zda nesou totožnou informaci (bez ohledu na vnitøní implementaci).

Dále je implementovaná metody `IsConsistent` pro zjištìní konistence kartièek.
Kontroluje, zda existují mìsta a regiony podle urèeného poètu, zda si navzájem odpovídají
sousedící regiony a zda jsou správnì definovány regiony na hranicích kartièky (právì jeden
region na každé hranici a konzistence metod `GetRegionBorders` × `GetRegionOnBorder`).

## Standardní implementace kartièek

Tøída `TileScheme` pøedstavuje obecnou implementaci rozhraní `ITileScheme`.
Jednotlivé informace o regionech, které jsou získávány metodami `ITileScheme`,
jsou uloženy v instancích `RegionInfo`.

## Prohledávání na herní desce

TODO: popis, graf kartièek, implementované metody