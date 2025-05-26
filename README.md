Det här är ett konsolprogram byggt i C# som simulerar en djurpark. Man kan lägga till djur, habitat, besökare och registrera besök. Det går även att visa statistik, t.ex. populäraste djuret och hur många besök parken haft.
Jag har använt:
Entity Framework Core för databas
LINQ för filtrering och statistik
Objektoienterad programmering med tydliga klasser (djur, habitat osv.)
En serviceklass (DjurparkService) för all logik
Program.cs har bara menyer och användargränssnitt
Jag har delat upp allt så att UI och logik är åtskilda. Det gör koden lättare att läsa, testa och bygga vidare på. Jag har också lagt till mockdata och en meny där man enkelt kan utföra CRUD-operationer (lägga till, visa, uppdatera, ta bort).
Syftet var att visa grunderna i OOP, EF och hur man strukturerar ett program tydligt.
