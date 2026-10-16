using System;
using System.Collections.Generic;

namespace SistemaReservasCanchas
{
    class Program
    {
        // =====================================================
        // VARIABLES GLOBALES (las usan varias funciones, por eso
        // las dejamos arriba de todo, fuera del Main)
        // =====================================================

        // Cantidad máxima de canchas que puede tener el sistema.
        static int MAX_CANCHAS = 10;

        // Arreglos paralelos: la posición [0] de cada uno
        // corresponde a los datos de la misma cancha.
        // Ej: nombresCancha[2], preciosCancha[2] y techada[2]
        // son todos datos de la cancha número 3 (índice 2).
        static string[] nombresCancha = new string[MAX_CANCHAS];
        static decimal[] preciosCancha = new decimal[MAX_CANCHAS];
        static char[] superficieCancha = new char[MAX_CANCHAS];
        static bool[] techadaCancha = new bool[MAX_CANCHAS];

        // Cuenta cuántas canchas hay cargadas realmente
        // (los arreglos de arriba tienen tamaño 10 fijo,
        // pero puede que todavía no estén todas usadas).
        static int cantidadCanchas = 0;

        // Matriz de disponibilidad: una grilla de 7 días x 24 horas
        // PARA CADA CANCHA. Por eso tiene 3 "dimensiones".
        // true = libre, false = ocupado.
        static bool[,,] disponibilidad = new bool[MAX_CANCHAS, 7, 24];

        // Nombres de los días, para no tener que escribirlos
        // a mano cada vez (el índice 0 es Lunes, el 6 es Domingo).
        static string[] nombresDias = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };

        // Acá vamos guardando las reservas que se van haciendo.
        // Usamos una lista (no un arreglo) porque no sabemos de
        // antemano cuántas reservas va a haber.
        static List<string> listaReservas = new List<string>();


        // =====================================================
        // OPCION 1: AGREGAR CANCHA
        // =====================================================
        static void AgregarCancha()
        {
            // Primero nos fijamos si ya llegamos al máximo de canchas.
            if (cantidadCanchas == MAX_CANCHAS)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo se pueden agregar más canchas, está lleno.");
                Console.ResetColor();
                return;
            }

            // Pedimos los datos uno por uno.
            Console.Write("\nNombre de la cancha: ");
            string nombre = Console.ReadLine();

            Console.Write("Precio por hora: ");
            decimal precio = decimal.Parse(Console.ReadLine());

            Console.Write("Tipo de superficie (S = sintetico, C = cemento): ");
            char superficie = char.Parse(Console.ReadLine());

            Console.Write("Es techada? (s/n): ");
            string respuestaTechada = Console.ReadLine();
            bool techada = (respuestaTechada == "s"); // si escribe "s" queda true, sino false

            // Guardamos todo en la posición "cantidadCanchas" de cada arreglo.
            nombresCancha[cantidadCanchas] = nombre;
            preciosCancha[cantidadCanchas] = precio;
            superficieCancha[cantidadCanchas] = superficie;
            techadaCancha[cantidadCanchas] = techada;

            // Como agregamos una cancha nueva, al principio todos sus
            // horarios están libres. Recorremos toda su grilla semanal
            // y la dejamos en true.
            for (int dia = 0; dia < 7; dia++)
            {
                for (int hora = 0; hora < 24; hora++)
                {
                    disponibilidad[cantidadCanchas, dia, hora] = true;
                }
            }

            // Aumentamos el contador porque ahora hay una cancha más.
            cantidadCanchas++;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCancha agregada correctamente.");
            Console.ResetColor();
        }


        // =====================================================
        // OPCION 2: MODIFICAR CANCHA
        // (por ahora solo dejamos modificar el precio, para no
        // complicar mucho la función)
        // =====================================================
        static void ModificarCancha()
        {
            if (cantidadCanchas == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTodavia no hay canchas cargadas.");
                Console.ResetColor();
                return;
            }

            // Mostramos las canchas numeradas para que el usuario
            // sepa qué índice tiene que elegir.
            ListarCanchas();

            Console.Write("\nQue cancha queres modificar? (numero de la lista): ");
            int indice = int.Parse(Console.ReadLine()) - 1; // restamos 1 porque en la lista mostramos desde el 1

            // Validamos que el índice elegido exista.
            if (indice < 0 || indice >= cantidadCanchas)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nEse numero no corresponde a ninguna cancha.");
                Console.ResetColor();
                return;
            }

            Console.Write("Nuevo precio para " + nombresCancha[indice] + ": ");
            decimal nuevoPrecio = decimal.Parse(Console.ReadLine());

            preciosCancha[indice] = nuevoPrecio;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPrecio actualizado.");
            Console.ResetColor();
        }


        // =====================================================
        // OPCION 3: ELIMINAR CANCHA
        // =====================================================
        static void EliminarCancha()
        {
            if (cantidadCanchas == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTodavia no hay canchas cargadas.");
                Console.ResetColor();
                return;
            }

            ListarCanchas();

            Console.Write("\nQue cancha queres eliminar? (numero de la lista): ");
            int indice = int.Parse(Console.ReadLine()) - 1;

            if (indice < 0 || indice >= cantidadCanchas)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nEse numero no corresponde a ninguna cancha.");
                Console.ResetColor();
                return;
            }

            // Para "borrar" del medio de un arreglo, corremos todos los
            // datos que estaban despues una posicion hacia atras.
            // Hay que hacerlo en los 4 arreglos a la vez porque son
            // paralelos (si no, se desordenan los datos entre si).
            for (int i = indice; i < cantidadCanchas - 1; i++)
            {
                nombresCancha[i] = nombresCancha[i + 1];
                preciosCancha[i] = preciosCancha[i + 1];
                superficieCancha[i] = superficieCancha[i + 1];
                techadaCancha[i] = techadaCancha[i + 1];
            }

            cantidadCanchas--;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCancha eliminada.");
            Console.ResetColor();

            // Ojo: esto no borra las reservas que ya se hicieron para
            // esa cancha. Lo dejamos asi por ahora, es algo que
            // podriamos mejorar despues.
        }


        // =====================================================
        // OPCION 4: LISTAR CANCHAS
        // =====================================================
        static void ListarCanchas()
        {
            if (cantidadCanchas == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTodavia no hay canchas cargadas.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();
            for (int i = 0; i < cantidadCanchas; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write((i + 1) + ". "); // le sumamos 1 para que se vea desde el 1 y no desde el 0
                Console.ResetColor();

                Console.Write(nombresCancha[i] + " - $" + preciosCancha[i] + " - ");
                Console.Write(superficieCancha[i] == 'S' ? "Sintetico" : "Cemento");

                if (techadaCancha[i])
                {
                    Console.Write(" - Techada");
                }

                Console.WriteLine();
            }
        }


        // =====================================================
        // OPCION 5: HACER UNA RESERVA
        // =====================================================
        static void HacerReserva()
        {
            if (cantidadCanchas == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTodavia no hay canchas cargadas.");
                Console.ResetColor();
                return;
            }

            ListarCanchas();
            Console.Write("\nQue cancha queres reservar? (numero de la lista): ");
            int cancha = int.Parse(Console.ReadLine()) - 1;

            if (cancha < 0 || cancha >= cantidadCanchas)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nEse numero no corresponde a ninguna cancha.");
                Console.ResetColor();
                return;
            }

            // Mostramos los dias con su numero para elegir.
            Console.WriteLine();
            for (int i = 0; i < nombresDias.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + nombresDias[i]);
            }
            Console.Write("Elegi el dia: ");
            int dia = int.Parse(Console.ReadLine()) - 1;

            Console.Write("Elegi el horario (0 a 23): ");
            int hora = int.Parse(Console.ReadLine());

            // Validamos que el dia y la hora esten dentro de los rangos posibles.
            if (dia < 0 || dia > 6 || hora < 0 || hora > 23)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nDia u horario invalido.");
                Console.ResetColor();
                return;
            }

            // Nos fijamos en la matriz si ese horario esta libre.
            if (disponibilidad[cancha, dia, hora] == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nEse horario ya esta ocupado.");
                Console.ResetColor();
                return;
            }

            Console.Write("Nombre del cliente: ");
            string cliente = Console.ReadLine();

            // Marcamos el horario como ocupado en la matriz.
            disponibilidad[cancha, dia, hora] = false;

            // Armamos un texto con todos los datos de la reserva
            // y lo agregamos a la lista.
            string datosReserva = cliente + " - " + nombresCancha[cancha] + " - " + nombresDias[dia] + " - " + hora + ":00";
            listaReservas.Add(datosReserva);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nReserva hecha con exito:");
            Console.WriteLine(datosReserva);
            Console.ResetColor();
        }


        // =====================================================
        // OPCION 6: VER DISPONIBILIDAD SEMANAL
        // =====================================================
        static void VerDisponibilidadSemanal()
        {
            if (cantidadCanchas == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTodavia no hay canchas cargadas.");
                Console.ResetColor();
                return;
            }

            ListarCanchas();
            Console.Write("\nQue cancha queres ver? (numero de la lista): ");
            int cancha = int.Parse(Console.ReadLine()) - 1;

            if (cancha < 0 || cancha >= cantidadCanchas)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nEse numero no corresponde a ninguna cancha.");
                Console.ResetColor();
                return;
            }

            // Mostrar las 24 horas completas no entra bien en la
            // pantalla, asi que mostramos solo el horario en el
            // que realmente se usa una cancha (18 a 22hs).
            int horaInicio = 18;
            int horaFin = 22;

            Console.WriteLine("\nDisponibilidad de " + nombresCancha[cancha] + ":\n");

            // Encabezado con los nombres de los dias.
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("       ");
            for (int dia = 0; dia < 7; dia++)
            {
                Console.Write(nombresDias[dia].Substring(0, 3) + "  "); // mostramos solo las primeras 3 letras
            }
            Console.ResetColor();
            Console.WriteLine();

            // Recorremos horas y dias para completar la grilla.
            for (int hora = horaInicio; hora <= horaFin; hora++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(hora + ":00  ");
                Console.ResetColor();

                for (int dia = 0; dia < 7; dia++)
                {
                    if (disponibilidad[cancha, dia, hora])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("OK   ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X    ");
                    }
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }


        // =====================================================
        // FUNCION QUE DIBUJA EL MENU
        // =====================================================
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

            Console.Write("\nElegi una opcion: ");
        }


        // =====================================================
        // MAIN: ACA EMPIEZA A CORRER EL PROGRAMA
        // =====================================================
        static void Main(string[] args)
        {
            int opcion;

            // El menu se repite hasta que se elija la opcion 0.
            do
            {
                MostrarMenu();
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        AgregarCancha();
                        break;

                    case 2:
                        ModificarCancha();
                        break;

                    case 3:
                        EliminarCancha();
                        break;

                    case 4:
                        ListarCanchas();
                        break;

                    case 5:
                        HacerReserva();
                        break;

                    case 6:
                        VerDisponibilidadSemanal();
                        break;

                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nCerrando el sistema...");
                        Console.ResetColor();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOpcion invalida, proba de nuevo.");
                        Console.ResetColor();
                        break;
                }

                if (opcion != 0)
                {
                    Console.WriteLine("\nPresiona una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 0);
        }
    }
}