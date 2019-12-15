# CarcassonneDiscovery.Logic

V tomto projektu VS se nachází objekty vykonávající herní akce vè. kontroly pravidel.

## Provádìní herních akcí

O provádní herních akcí se stará tøída `GameExecutor`.

Herní akce je možné provádìt dvìma variantami.
Jedna varianta pouze provede herní akci (metody `Set...`),
druhý varianta navíc kontroluje pravidla pro vykonání herní akce (metody `Try...`).

Jako výstup je navrácen objekt popisující výsledek akce.
Jednotlivé výsledky akcí mají vlastní tøídy, které dìdí z tøídy `GameExecutionResult`.

Pokud herní akce porušuje pravidla, je souèástí výsledku i popis této chyby.
Jednotlivé prohøešky proti pravidlùm jsou ve výètu `RuleViolationType`.

Jednotlivé herní akce:
- zahájení hry: `SetStartGame`, `TryStartGame`,
- zahájení tahu: `SetStartMove`, `TryStartMove`,
- umístìní kartièky: `SetPlaceTile`, `TryPlaceTile`,
- umístìní figurky: `SetPlaceFollower`, `TryPlaceFollower`,
- odebrání figurky: `SetRemoveFollower`, `TryRemoveFollower`,
- pøedání tahu: `SetPassMove`, `TryPassMove`,
- ukonèení hry: `SetEndGame`, `TryEndGame`,
