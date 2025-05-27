using System;
using System.Threading.Tasks;
using DjurparkGUI.Data;
using DjurparkGUI.Tjänster;

namespace DjurparkGUI
{
    internal class Program
    {
        // Denna klass innehåller endast användargränssnitt och menysystem.
        // All logik finns i DjurparkService (separation av ansvar enligt OOP/SRP).
        static async Task Main(string[] args)
        {
            {
                // Enkel login innan man kommer in i systemet
                bool loggedIn = false;
                while (!loggedIn)
                {
                    Console.Clear();
                    Console.WriteLine("Logga in för att använda Djurparken\n");
                    Console.Write("Användarnamn: ");
                    string username = Console.ReadLine();
                    Console.Write("Lösenord: ");
                    string password = Console.ReadLine(); // För enkelhet – visas i klartext

                    // Hårdkodade konton – kan bytas ut mot databas i framtiden
                    if ((username == "admin" && password == "admin123") ||
                        (username == "zoo" && password == "zoo2024"))
                    {
                        Console.WriteLine("\nInloggning lyckades!");
                        loggedIn = true;
                        await Task.Delay(1000); // liten fördröjning för användarupplevelse
                    }
                    else
                    {
                        Console.WriteLine("\nFel användarnamn eller lösenord! Försök igen.");
                        Console.ReadKey();
                    }
                }
                await using var db = new ZooContext();
                var zooService = new DjurparkService(db);

                bool isRunning = true;

                while (isRunning)
                {
                    Console.Clear();
                    Console.WriteLine(" Välkommen till Djurparken \n");
                    Console.WriteLine("1. Lägg till djur");
                    Console.WriteLine("2. Visa alla djur");
                    Console.WriteLine("3. Uppdatera djurstatus");
                    Console.WriteLine("4. Ta bort djur");
                    Console.WriteLine("5. Lägg till habitat");
                    Console.WriteLine("6. Visa alla habitat");
                    Console.WriteLine("7. Ta bort habitat");
                    Console.WriteLine("8. Lägg till besökare");
                    Console.WriteLine("9. Visa alla besökare");
                    Console.WriteLine("10. Uppdatera besökares kontaktinfo");
                    Console.WriteLine("11. Ta bort besökare");
                    Console.WriteLine("12. Visa alla besök");
                    Console.WriteLine("13. Visa besök för specifik besökare");
                    Console.WriteLine("14. Sök efter djur");
                    Console.WriteLine("15. Visa statistik");
                    Console.WriteLine("16. Simulera 7 dagar");
                    Console.WriteLine("17. Avsluta\n");
                    Console.Write("Val: ");

                    var input = Console.ReadLine();
                    Console.Clear();

                    // Växlar beroende på användarens val
                    switch (input)
                    {
                        case "1":
                            await zooService.LäggTillDjurViaInputAsync();
                            break;
                        case "2":
                            await zooService.VisaAllaDjurAsync();
                            break;
                        case "3":
                            await zooService.UppdateraDjurStatusAsync();
                            break;
                        case "4":
                            await zooService.TaBortDjurAsync();
                            break;
                        case "5":
                            await zooService.LäggTillHabitatAsync();
                            break;
                        case "6":
                            await zooService.VisaAllaHabitatAsync();
                            break;
                        case "7":
                            await zooService.TaBortHabitatAsync();
                            break;
                        case "8":
                            await zooService.LäggTillBesökareAsync();
                            break;
                        case "9":
                            await zooService.VisaAllaBesökareAsync();
                            break;
                        case "10":
                            await zooService.UppdateraBesökareKontaktAsync();
                            break;
                        case "11":
                            await zooService.TaBortBesökareAsync();
                            break;
                        case "12":
                            await zooService.VisaAllaBesökAsync();
                            break;
                        case "13":
                            await zooService.VisaBesökFörBesökareAsync();
                            break;
                        case "14":
                            await zooService.LäggTillBesökAsync();
                            break;
                        case "15":
                            await zooService.VisaStatistikAsync();
                            break;
                        case "16":
                            await zooService.SimuleraSjuDagarAsync();
                            break;
                        case "17":
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Felaktigt val!");
                            break;
                    }

                    if (isRunning)
                    {
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}





