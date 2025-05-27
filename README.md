Det här är ett konsolprogram byggt i C# som simulerar en djurpark. Man kan lägga till djur, habitat, besökare och registrera besök. Det går även att visa statistik, t.ex. populäraste djuret och hur många besök parken haft.
Jag har använt:
Entity Framework Core för databas
LINQ för filtrering och statistik
Objektoienterad programmering med tydliga klasser (djur, habitat osv.)
En serviceklass (DjurparkService) för all logik
Program.cs har bara menyer och användargränssnitt
Jag har delat upp allt så att UI och logik är åtskilda. Det gör koden lättare att läsa, testa och bygga vidare på. Jag har också lagt till mockdata och en meny där man enkelt kan utföra CRUD-operationer (lägga till, visa, uppdatera, ta bort).
Syftet var att visa grunderna i OOP, EF och hur man strukturerar ett program tydligt.

jag valde att lägga all logik i en separat klass – DjurparkService – så att Program.cs bara hanterar användargränssnittet. Det gör det mycket lättare att testa, återanvända och underhålla koden.
Jag har lagt till en metod som automatiskt lägger in testdata första gången man kör programmet – så att man snabbt kan se hur systemet fungerar utan att mata in allt manuellt.
Jag harlagt till en många-till-många-relation mellan besökare och djur, där en besökare kan ha flera favoritdjur.
Jag har använt TryParse och kontroller på alla användarinmatningar, så att programmet inte kraschar om man t.ex. råkar skriva text istället för siffror.
Lagt till en simulering som räknar på intäkter, kostnader och antal besökare under 7 dagar, vilket gör att man kan testa verksamhetens lönsamhet.
Jag använder LINQ för att räkna ut populäraste djuret, habitat med flest djur, och antal besök per månad.

Jag har även lagt till en utkommenterad Azure sträng, så att denna kan kopplas.
