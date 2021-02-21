using System;
using System.IO;
using System.Collections;
using XmpCore;
using System.Globalization;

namespace LightroomPresetRenamer
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Processing");
            // if (args[0] == null)
            if (args.Length == 0)

            {
                Console.WriteLine("Please provide a path");
                return;
            }
            else
            {
                DirectoryInfo startDir = new DirectoryInfo(args[0]);
                TraverseDirectory(startDir);
            }

        }
        private static void TraverseDirectory(DirectoryInfo directoryInfo)
        {
            var subdirectories = directoryInfo.EnumerateDirectories();

            foreach (var subdirectory in subdirectories)
            {
                TraverseDirectory(subdirectory);
            }

            var files = directoryInfo.EnumerateFiles();

            foreach (var file in files)
            {
                // 
                if (file.Extension == ".xmp")
                {
                    //   Console.WriteLine(file);
                    // if (file.Name.Contains("Velvia"))
                    //    {
                    HandleFile(file);
                    //   }
                }
            }
        }

        private static void HandleFile(FileInfo file)
        {
            // Console.WriteLine("{0}", file.Name);


            IXmpMeta xmp;
            String curFile = file.FullName.ToString();
            // using (var stream = File.OpenRead(file.DirectoryName + "/" + file.Name))
            using (var stream = File.OpenRead(curFile))
                xmp = XmpMetaFactory.Parse(stream);
            Boolean success = false;
            foreach (var property in xmp.Properties)
            {
                success = false;
                //Console.WriteLine($"Path={property.Path} Namespace={property.Namespace} Value={property.Value}");
                //  Console.WriteLine($"Path={property.Path} Value={property.Value}");

                //  if (property.Path == "crs:Look/crs:Name"|| property.Path == "crs:Name[1]")
                if (property.Path == "crs:Name[1]")
                {
                    string newfilename = property.Value.Trim().Replace('/', '-') + ".xmp";
                    if (string.Compare(newfilename, file.Name.ToString(), CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
                    //  if (newfilename != file.Name)
                    { break; }
                    else
                    {
                          file.CopyTo(file.DirectoryName + "/" + file.Name + "_old", true);

                        file.CopyTo(file.DirectoryName + "/" + newfilename, true);
                        Console.WriteLine(property.Value);
                        success = true;

                        break;
                    }
                }
            }
            if (success == true)
            {
                file.Delete();

            }
        }
    }


}

