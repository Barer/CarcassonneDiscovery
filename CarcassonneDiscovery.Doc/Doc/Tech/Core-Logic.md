# CarcassonneDiscovery.Logic

V tomto projektu VS se nach�z� objekty vykon�vaj�c� hern� akce v�. kontroly pravidel.

## Prov�d�n� hern�ch akc�

O prov�dn� hern�ch akc� se star� t��da `GameExecutor`.

Hern� akce je mo�n� prov�d�t dv�ma variantami.
Jedna varianta pouze provede hern� akci (metody `Set...`),
druh� varianta nav�c kontroluje pravidla pro vykon�n� hern� akce (metody `Try...`).

Jako v�stup je navr�cen objekt popisuj�c� v�sledek akce.
Jednotliv� v�sledky akc� maj� vlastn� t��dy, kter� d�d� z t��dy `GameExecutionResult`.

Pokud hern� akce poru�uje pravidla, je sou��st� v�sledku i popis t�to chyby.
Jednotliv� proh�e�ky proti pravidl�m jsou ve v��tu `RuleViolationType`.

Jednotliv� hern� akce:
- zah�jen� hry: `SetStartGame`, `TryStartGame`,
- zah�jen� tahu: `SetStartMove`, `TryStartMove`,
- um�st�n� karti�ky: `SetPlaceTile`, `TryPlaceTile`,
- um�st�n� figurky: `SetPlaceFollower`, `TryPlaceFollower`,
- odebr�n� figurky: `SetRemoveFollower`, `TryRemoveFollower`,
- p�ed�n� tahu: `SetPassMove`, `TryPassMove`,
- ukon�en� hry: `SetEndGame`, `TryEndGame`,
