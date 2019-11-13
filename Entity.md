# CarcassonneDiscovery.Entity

V tomto projektu VS se nachází objekty reprezentující herní prvky.

## Výètové typy

### `TileOrientation`

Výètový typ `TileOrientation` reprezentuje orientaci kartièky, pozici na okraji kartièky nebo smìr ukazující na sousední kartièky.
Jednotlivé prvky oznaèují svìtové strany (sever `N`, východ `E`, jih `S`, západ `W`).

Je použita bitová logika, je tedy možné zapsat více smìrù naráz (napø. `NE` reprezentuje zároveò sever a východ). Žádný smìr je reprezentován prvkem `None`.

### `RegionType`

Výètový typ `RegionType` reprezentuje typ regionu na kartièce (hory, nížiny a moøe).

### `PlayerColor`

Výètový typ `PlayerColor` reprezentuje barvu figurek hráèe. Zvláštní barva `PlayerColor.None` je použita pøi akcích, které nejsou iniciovány žádným hráèem.

Pomocí barev jsou hráèi identifikováni pro úèely hry.

### `MoveWorkflow`

Výètový typ `MoveWorkflow` reprezentuje stav tahu.
TODO (pøi revizi dokumentace): Graf s popisem herních fází


## Struktury, tøídy a rozhraní

### `ITileScheme`

Rozhraní `ITileScheme` reprezentuje vzhled kartièky.
Vzhled kartièek je urèen poètem a strukturou regionù a mìst.

Je nutné implementovat následující metody a property:
- zjištìní poètu regionù a mìst,
- zjištìní, který region se nachází na dané hranici,
- dotazy na konkrétní region (podle jeho identifikátoru):
   - typ regionu,
   - seznam náležících mìst,
   - seznam sousedních regionù,
- identifikátor kartièky uvnitø herní sady (string s libovolnou hodnotou, podle dané herní sady).

### `ITileSupplier`

Rozhraní `ITileSupplier` reprezentuje poskytování kartièek pro hru.

Poskytuje:
- poèet zbývajícíh kartièek,
- první kartièku ve høe,
- následující kartièku,
- možnost vrácení kartièky do sady (pokud napø. nelze umstit).

### `GameParams`

Tøída `GameParams` reprezentuje parametry hry.

Parametry hry zahrnují:
- poèet hráèù,
- poèet figurek pro každého hráèe,
- poøadí hráèù ve høe,
- informace o sadì kartièek.

### `TileSetParams`

Tøída `TileSetParams` reprezentuje parametry použité sady kartièek.

Parametry sady kartièek zahrnují:
- název sady kartièek.

### `Coords`

Struktura `Coords` reprezentuje souøadnice kartièky (nebo dalších herních prvkù) na herní desce.

### `GameState`

Tøída `GameState` reprezentuje aktuální stav hry.

Stav hry zahrnuje:
- parametry hry (vè. poskytovatele kartièek),
- desku s umístìnými kartièkami a figurkami,
- aktuální fázi tahu, hráèe na tahu a vylosovanou kartièku pro tento tah,
- aktuální skóre hráèù.

### `TilePlacement`

Tøída `TilePlacement` reprezentuje umístìní kartièky na desce.

Umístìní kartièky zahrnuje:
- souøadnice kartièky,
- vzhled a orientaci kartièky,
- informaci o umístìné figurce na kartièce.

### `FollowerPlacement`

Tøída `FollowerPlacement` reprezentuje umístìní figurky na desce.

Umístìní figurky zahrnuje:
- barvu figurky,
- souøadnice kartièky, na které je figurka umístìna,
- identifikátor regionu, kde je figurka umístìna.