    using System;
    using System.IO;
namespace EjercicioSp
{

    //class Test
    //{
    //    public static void Main()
    //    {
    //        int count = 0;
    //        try
    //        {
    //            // Create an instance of StreamReader to read from a file.
    //            // The using statement also closes the StreamReader.
    //            using (StreamReader sr = new StreamReader("C:\\Users\\Ingrid Medina\\OneDrive\\Documentos\\Visual Studio 2022\\EjercicioSp\\Texto.txt"))
    //            {
    //                string line;
    //                // Read and display lines from the file until the end of
    //                // the file is reached.
    //                while ((line = sr.ReadLine()) != null)
    //                {
    //                    count++;
    //                    Console.WriteLine(line);
    //                    Console.WriteLine(count);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            // Let the user know what went wrong.
    //            Console.WriteLine("The file could not be read:");
    //            Console.WriteLine(e.Message);
    //        }
    //    }

    //}


//class Test
//    {
//        public static void Main()
//        {
//            string path = "C:\\Users\\Ingrid Medina\\OneDrive\\Documentos\\Visual Studio 2022\\EjercicioSp\\Texto.txt"; // archivo a leer y analizar
//            int LOCs = 0;   // código ejecutable
//            int LOCcs = 0;  // comentarios
//            int LOcm = 0;   // modificaciones (simulado 0 para primera versión)
//            int count = 0;  // contador de líneas del archivo

//            try
//            {
//                using (StreamReader sr = new StreamReader(path))
//                {
//                    string line;
//                    while ((line = sr.ReadLine()) != null)
//                    {
//                        count++;
//                        Console.WriteLine(line);  // mostrar línea original
//                        Console.WriteLine($"Línea número: {count}");

//                        string trimmed = line.Trim();

//                        if (trimmed.StartsWith("//") || trimmed == "")
//                        {
//                            LOCcs++;  // línea de comentario o vacía
//                        }
//                        else
//                        {
//                            LOCs++;   // línea de código ejecutable
//                        }
//                    }
//                }

//                Console.WriteLine("\n=== Análisis PSP ===");
//                Console.WriteLine($"LOCs (código ejecutable): {LOCs}");
//                Console.WriteLine($"LOCcs (comentarios o vacías): {LOCcs}");
//                Console.WriteLine($"LOcm (líneas modificadas): {LOcm}");
//                Console.WriteLine($"Total de líneas registradas: {LOCs + LOCcs + LOcm}");
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("El archivo no pudo ser leído:");
//                Console.WriteLine(e.Message);
//            }
//        }
//    }


    using System;
using System.IO;
using System.Collections.Generic;

class Test
    {
        public static void Main()
        {
            // Ruta de tu archivo de texto
            string path = "C:\\Users\\Ingrid Medina\\OneDrive\\Documentos\\Visual Studio 2022\\EjercicioSp\\Texto.txt";
            string snapshotPath = path + ".snapshot"; // archivo con la versión anterior (línea por línea)

            int LOCs = 0;   // código ejecutable
            int LOCcs = 0;  // comentarios o vacías

            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"No se encontró el archivo: {path}");
                    return;
                }

                // Leer archivo actual
                string[] currentLines = File.ReadAllLines(path);

                // === Conteo LOCs / LOCcs del archivo actual ===
                int lineNumber = 0;
                foreach (var rawLine in currentLines)
                {
                    lineNumber++;
                    Console.WriteLine(rawLine);
                    Console.WriteLine($"Línea número: {lineNumber}");

                    string trimmed = rawLine.Trim();
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("//"))
                        LOCcs++;
                    else
                        LOCs++;
                }

                // === Comparación contra snapshot (si existe) ===
                bool firstRun = !File.Exists(snapshotPath);
                List<string> prevLines = firstRun
                    ? new List<string>()
                    : new List<string>(File.ReadAllLines(snapshotPath));

                int modified = 0;
                int added = 0;
                int deleted = 0;

                if (firstRun)
                {
                    Console.WriteLine("\nPrimera ejecución: no hay snapshot previo. Se creará uno ahora.");
                }
                else
                {
                    int minCount = Math.Min(prevLines.Count, currentLines.Length);

                    // Líneas modificadas (mismo índice, contenido diferente)
                    for (int i = 0; i < minCount; i++)
                    {
                        if (!string.Equals(prevLines[i], currentLines[i]))
                        {
                            modified++;
                            Console.WriteLine($"\n⚠ Línea modificada #{i + 1}");
                            Console.WriteLine($"   Antes: {prevLines[i]}");
                            Console.WriteLine($"   Ahora: {currentLines[i]}");
                        }
                    }

                    // Líneas agregadas
                    if (currentLines.Length > prevLines.Count)
                    {
                        added = currentLines.Length - prevLines.Count;
                        for (int i = prevLines.Count; i < currentLines.Length; i++)
                        {
                            Console.WriteLine($"\n➕ Línea nueva #{i + 1}");
                            Console.WriteLine($"   Contenido: {currentLines[i]}");
                        }
                    }

                    // Líneas eliminadas
                    if (prevLines.Count > currentLines.Length)
                    {
                        deleted = prevLines.Count - currentLines.Length;
                        for (int i = currentLines.Length; i < prevLines.Count; i++)
                        {
                            Console.WriteLine($"\n➖ Línea eliminada (posición previa #{i + 1})");
                            Console.WriteLine($"   Contenido anterior: {prevLines[i]}");
                        }
                    }
                }

                int LOcm = modified + added + deleted;

                // === Resumen ===
                Console.WriteLine("\n=== Análisis PSP ===");
                Console.WriteLine($"Total líneas actuales: {currentLines.Length}");
                Console.WriteLine($"LOCs (código ejecutable): {LOCs}");
                Console.WriteLine($"LOCcs (comentarios o vacías): {LOCcs}");
                Console.WriteLine($"LOcm (cambios vs. última ejecución): {LOcm}  (mod:{modified}, +{added}, -{deleted})");

                // Nota: LOCs + LOCcs == total de líneas actuales.
                // LOcm es un conteo de diferencias contra el snapshot anterior (no suma al total actual).

                // === Guardar snapshot para la próxima ejecución ===
                File.WriteAllLines(snapshotPath, currentLines);

                Console.WriteLine($"\nSnapshot actualizado en: {snapshotPath}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrió un error:");
                Console.WriteLine(e.Message);
            }
        }
    }






    //class Example
    //{
    //    static async Task Main()
    //    {
    //        await ReadAndDisplayFilesAsync();
    //    }

    //    static async Task ReadAndDisplayFilesAsync()
    //    {
    //        String filename = "C:\\Users\\Ingrid Medina\\OneDrive\\Documentos\\Visual Studio 2022\\EjercicioSp\\Texto.txt";
    //        Char[] buffer;

    //        using (var sr = new StreamReader(filename))
    //        {
    //            buffer = new Char[(int)sr.BaseStream.Length];
    //            await sr.ReadAsync(buffer, 0, (int)sr.BaseStream.Length);
    //        }

    //        Console.WriteLine(new String(buffer));

    //    }




    //}
}
