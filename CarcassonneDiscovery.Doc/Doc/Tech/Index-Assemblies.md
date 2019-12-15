# Knihovny, spustiteln� soubory a jejich z�vislosti

Jednotliv� knihovny a jejich popis:
- Core ��st
  - [`CarcassonneDiscovery.Entity.dll`](Core-Entity.md) = t��dy, kter� reprezentuj� hern� prvky (stav hry, karti�ky, apod.)
  - [`CarcassonneDiscovery.Logic.dll`](Core-Logic.md) = t��dy, kter� se staraj� o hern� logiku (kontrola pravidel, vykon�n� zm�ny stavu hry)
  - [`CarcassonneDiscovery.Tools.dll`](Core-Tools.md) = pomocn� t��dy a metody, kter� manipujuj� s hern�mi prvky
- Messaging ��st (komunikace mezi serverem a klienty)
  - [`CarcassonneDiscovery.Messaging.dll`](Messaging-Messaging.md) = t��dy, kter� slou�� jako prost�edn�ci v komunikaci mezi serverem a klientem (jednotliv� "zpr�vy")
  - [`CarcassonneDiscovery.Socketing.dll`](Messaging-Socketing.md) = t��dy, kter� slou�� jako sockety (koncov� body komunikace)
- Server ��st
  - [`CarcassonneDiscovery.ServerBase.dll`](Server-ServerBase.md) = t��dy, kter� jsou z�kladem hern�ho serveru (TODO: s interfacem pro vstup a v�stup)
  - [`CarcassonneDiscovery.HeadlessServer.exe`](Server-HeadlessServer.md) = spusiteln� server v headless m�du
  - TODO: mo�n� upravit interface tak, �e se o komunikaci star� Headless remote
- Klient ��st
  - [`CarcssonneDiscovery.UserClient.exe`](Client-UserClient.md) = spusiteln� klient s GUI pro hru u�ivatele
  - TODO: mohlo by b�t op�t znou odli�iteln� a znovupou�iteln� i pro AI?!
- AI ��st
  - [`CarcassonneDiscovery.AiBase.dll`](Ai-AiBase.md) = t��dy, kter� jsou z�kladem AI klienta
  - jednotliv� implementace hr���:
    - `CarcassonneDiscovery.TrivialAi.exe`
    - `CarcassonneDiscovery.SimpleHeuristicAi.exe`

Graf z�vislost�:

![Obr�zek: graf z�vislost�](./Img/AssemblyDependency.png)