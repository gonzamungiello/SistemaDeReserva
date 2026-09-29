using System;

namespace SistemaReservasCanchas
{
    class Program
    {
        static void MostrarMenu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=====================================");
            Console.WriteLine("   SISTEMA DE RESERVAS DE CANCHAS");
            Console.WriteLine("=====================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("1. ");
            Console.ResetColor();
            Console.WriteLine("Agregar cancha");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("2. ");
            Console.ResetColor();
            Console.WriteLine("Modificar cancha");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("3. ");
            Console.ResetColor();
            Console.WriteLine("Eliminar cancha");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("4. ");
            Console.ResetColor();
            Console.WriteLine("Listar canchas");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("5. ");
            Console.ResetColor();
            Console.WriteLine("Hacer una reserva");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("6. ");
            Console.ResetColor();
            Console.WriteLine("Ver disponibilidad semanal");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0. Salir");
            Console.ResetColor();

            Console.Write("\nElegí una opción: ");
        }
        static void Main(string[] args)
        {
            // Variable que guarda la opción que elige el usuario.
            int opcion;

            // do while: repite el menú hasta que se elija la opción 0.
            do
            {
                // Mostramos el menu(está en su propia función más arriba).
                MostrarMenu();

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = -1; // Si el usuario pone una opcion no valida, se asigna -1 para que entre en el default del switch.
                }

                // switch: según el número, hacemos una cosa distinta.
                switch (opcion)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nAgregar cancha (todavía falta programarlo)");
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nModificar cancha (todavía falta programarlo)");
                        break;

                    case 3:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nEliminar cancha (todavía falta programarlo)");
                        break;

                    case 4:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nListar canchas (todavía falta programarlo)");
                        break;

                    case 5:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nHacer una reserva (todavía falta programarlo)");
                        break;

                    case 6:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nVer disponibilidad semanal (todavía falta programarlo)");
                        break;

                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nCerrando el sistema...");
                        break;

                    default:
                        // Se ejecuta si el número no coincide con ningún case de arriba.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOpción inválida, probá de nuevo.");
                        break;
                }

                // Volvemos al color normal de la consola.
                Console.ResetColor();

                // Pausa para poder leer el mensaje antes de que se repita el menú.
                if (opcion != 0)
                {
                    Console.WriteLine("\nPresioná una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 0);
        }
    }
}