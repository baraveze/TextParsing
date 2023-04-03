using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextParsing
{
    class Program
    {
        static void Main(string[] args)
        {
           
            // declaracón de variables
            string rutaOrigen = ConfigurationManager.AppSettings["rutaOrigen"];
            string rutaDestino = ConfigurationManager.AppSettings["rutaDestino"];// "E:\\ezeba\\Documents\\";
            string nombreArchivoConvertido = "";
            string[] texto = new string[10000];
            char[] caracteresEspeciales = { '!', ',', ';', ';', ':', '?', '/', '(', ')' };
            int reintentos = 0;
            int reintentoNombreArchivo = 0;
            List<string> textoFinal = new List<string>();
            int i = 0;
            List<string> listaRemove = new List<string>();

            // Especificación de la RUTA si no está en el App Config se lo pide al usuario
            if (rutaOrigen == null || rutaOrigen == "" || !System.IO.File.Exists(rutaOrigen))
            {
                if (!System.IO.File.Exists(rutaOrigen))
                    reintentos = 1;

                do
                {

                    if (reintentos > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("\n Hubo un error con la ruta ingresada anteriormente, no existe el archivo especificado. Por favor, Ingrese la ruta del archivo que desea procesar y luego presione enter:");
                    }
                    else
                        Console.WriteLine("Ingrese la ruta del archivo que desea procesar y luego presione enter:");
                    rutaOrigen = Console.ReadLine();
                    reintentos++;

                }
                while (!System.IO.File.Exists(rutaOrigen));


                if (System.IO.File.Exists(rutaOrigen))
                {
                    Console.Clear();
                    Console.WriteLine("Archivo especificado encontrado. La ruta ingresada fue: {0} ", rutaOrigen);
                }
            }

            // RUTA DESTINO

            if (rutaDestino != null && rutaDestino != "" && rutaDestino.Length > 1)
            {

                Console.WriteLine("\nLa ruta predeterminada de R es {0}. ¿Si desea cambiarla?", rutaDestino);
                Console.WriteLine("[S/N] ('S': Sí | 'N': No)");
                string opcion = Console.ReadLine();

                if (opcion.ToLower().Trim() == "s" || opcion.ToLower().Trim() == "si")
                {
                    Console.WriteLine("\nIngrese la ruta del espacio de trabajo del programa R");
                    rutaDestino = Console.ReadLine();
                    Console.WriteLine("\nLa nueva ruta de trabajo de R ingresada es: {0}", rutaDestino);
                    Console.WriteLine("¿Es correcto? [S/N] ('S': Sí | 'N': No)");
                    string opcion2 = Console.ReadLine();
                    if (opcion2.ToLower().Trim() == "n")
                    {
                        Console.WriteLine("\nSi no es correcto entonces averigue la ruta de R y vuelva a empezar. El programa" +
                          " finalizará luego de que oprima cualquier tecla.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            // Setear nombre del archivo

            do
            {
                if (reintentoNombreArchivo > 0)
                {
                    Console.Clear();
                    Console.WriteLine("\n Error en el nombre del archiv. Recordá que no puede tener caractéres especiales. " +
                                          "Si deja espacios en blanco, serán reemplazados por '-'. Reintentá nuevamente. ");
                }
                else
                    Console.WriteLine("Ahora ingrese el nombre que desea colocarle al archivo. Recuerde que solo puede poner letras" +
                                        "o números pero no puede caractéres especiales. Si deja espacios en blanco, " +
                                        "serán reemplazados por '-' ");

                nombreArchivoConvertido = Console.ReadLine();
                reintentoNombreArchivo++;


            } while (nombreArchivoConvertido != "" && nombreArchivoConvertido.IndexOfAny(caracteresEspeciales) > -1);

            nombreArchivoConvertido = nombreArchivoConvertido.Replace(" ", "-");


            //LEER TODO EL ARCHIVO DE TEXTO
            texto = System.IO.File.ReadAllLines(@rutaOrigen);


            Console.WriteLine("-------------------------------------");
            Console.WriteLine("---------LIMPIEZA DEL TEXTO----------");
            Console.WriteLine("-------------------------------------");

            Console.WriteLine("¿Deseas especificar un listado de palabras o caracteres que querés suprimir del texto?. Por" +
                " ejemplo: Prepociones, signos de puntuación, lo que desees.");
            Console.WriteLine("[S/N]");
            string opcion3 = Console.ReadLine();
            string simboloAQuitar = "";
            int countSimbolos = 0;
            if (opcion3.ToLower().Trim() == "s")
            {
                Console.WriteLine(" \n Indicaste que si. Bien, ahora por cada frase, palabra o signo de puntuación que quieras quitar del texto lo vas a ingresar de a uno por vez y luego vas a oprimir ENTER. Luego para salir de esta opción ingresa '@@' y luego ENTER");

                do
                {
                    Console.WriteLine("Ingresá el elemento, luego enter");
                    simboloAQuitar = Console.ReadLine();
                    if (simboloAQuitar.ToLower().Trim() != "@@")
                    {
                        listaRemove.Add(simboloAQuitar);
                        Console.WriteLine("OK, el string {0} sera removido del texto. Ingresá el siguiente", simboloAQuitar);
                    }


                } while (simboloAQuitar.ToLower().Trim() != "@@");
            }


            int longitud = texto.Length;
            string linea = "";
            System.Console.WriteLine("Contenido del archivo = ");


            for (int j = 0; j < texto.Length; j++)
            {
                for (int k = 0; k < listaRemove.Count; k++)
                {
                    texto[j] = texto[j].Replace(listaRemove[k], "");
                }


                if (j % 10 == 0 && j > 0)
                {

                    textoFinal.Add(linea);

                    if (j >= 0 && j < texto.Length)
                    {
                        // textoFinal.Add("\n");
                        linea = texto[j] + " ";
                    }
                    else
                    {
                        if (j == texto.Length - 1)
                            textoFinal.Add(linea);
                    }

                }
                else
                    linea += texto[j] + " ";
            }

            textoFinal.Add(linea);

            foreach (string iter in textoFinal)
                Console.WriteLine(iter);


            System.IO.File.WriteAllLines(@rutaDestino + nombreArchivoConvertido + ".txt", textoFinal);
            Console.WriteLine("Proceso finalizado con éxito. Presione cualquier tecla para salir.");
            System.Console.ReadKey();

            //System.IO.File.WriteAllLines
        }
    }
}

