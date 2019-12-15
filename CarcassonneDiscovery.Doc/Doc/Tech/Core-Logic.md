# CarcassonneDiscovery.Logic.dll

V tomto knihovnì se nachází tøída `GameExecutor`, která vykonává herní akce, a tøídy reprezentující výsledky tìchto akcí.

## Provádìní herních akcí

O provádní herních akcí se stará tøída `GameExecutor`.

Herní akce je možné provádìt dvìma variantami.
Jedna varianta pouze provede herní akci (metody `Set...`),
druhý varianta navíc kontroluje pravidla pro vykonání herní akce (metody `Try...`).

Jako výstup je navrácen objekt popisující výsledek akce.
Jednotlivé výsledky akcí mají vlastní tøídy, které dìdí z tøídy `GameExecutionResult`.

Pokud herní akce porušuje pravidla, je souèástí výsledku i popis této chyby.
Jednotlivé prohøešky proti pravidlùm odpovídají prvkùm výètu `RuleViolationType`.

Jednotlivé herní akce:
- zahájení hry: `SetStartGame`, `TryStartGame`,
- zahájení tahu: `SetStartMove`, `TryStartMove`,
- umístìní kartièky: `SetPlaceTile`, `TryPlaceTile`,
- umístìní figurky: `SetPlaceFollower`, `TryPlaceFollower`,
- odebrání figurky: `SetRemoveFollower`, `TryRemoveFollower`,
- pøedání tahu: `SetPassMove`, `TryPassMove`,
- ukonèení hry: `SetEndGame`, `TryEndGame`.

Metoda `SetStartGame` má dvì varianty,:
- prijímající jako argument poskytovatele kartièek (`ITileSupplier`); tuto variantu používá server,
- pøijímající jako argumenty parametry o umístìní první kartièky (schéma, otoèení, umístìní); tuto variantu používá klient.
