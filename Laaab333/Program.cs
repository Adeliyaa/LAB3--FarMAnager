using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace Week_3
{
    class FarManager
    {
        //variables or parametres of class
        public int cursor;
        public string path;
        public int sz;
        public bool ok;
        DirectoryInfo directory = null; //empty
        FileSystemInfo currentFs = null; 

        /*public FarManager()
        {
            cursor = 0;
        }*/

           
        public FarManager(string path)
            //Constructor 
        {
            this.path = path; //get the value from parametres of the class
            cursor = 0;
            directory = new DirectoryInfo(path);
            sz = directory.GetFileSystemInfos().Length; 
            //create the array which are contain all the files and directories in a current path and get the lenght of that array
            ok = true;
        }

        public void Color(FileSystemInfo fs, int index)
        {
            if (cursor == index)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                currentFs = fs;
            }
            else if (fs.GetType() == typeof(DirectoryInfo))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
        }

        public void Show()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            directory = new DirectoryInfo(path);
            FileSystemInfo[] fs = directory.GetFileSystemInfos();
            for (int i = 0, k = 0; i < fs.Length; i++)
            {
                if (ok == false && fs[i].Name[0] == '.')
                    //if the file's first symbol is . then just ignore or hide it 
                {
                    continue;
                }
                Color(fs[i], k);
                Console.WriteLine(fs[i].Name);
                k++;
            }
        }
        public void Up()
        {
            cursor--;
            if (cursor < 0)
                cursor = sz - 1;
        }
        public void Down()
        {
            cursor++;
            if (cursor == sz)
                cursor = 0;
        }


        public void CalcSz()
        //count the length directory
        {
            directory = new DirectoryInfo(path);
            FileSystemInfo[] fs = directory.GetFileSystemInfos();
            //whole files and directories in path
            sz = fs.Length;
            //length of current directory(path)
            if (ok == false)
                //we have 2 mode, hide files 
                for (int i = 0; i < fs.Length; i++)
                    if (fs[i].Name[0] == '.')
                        //.Name is string , string is array, [0] is first element
                        //if(first element) is .(point)
                        sz--;
        }

        public static void DeleteDirectory(string deld)
        {
            string[] files = Directory.GetFiles(deld);
            string[] dirs = Directory.GetDirectories(deld);

            foreach (string file in files)
            {
                //File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
                //recursion
            }

            Directory.Delete(deld, false);
        }

        public void Start()
        {
            ConsoleKeyInfo consoleKey = Console.ReadKey();
            while (consoleKey.Key != ConsoleKey.Escape) //end of cycle
            {
                CalcSz();
                Show();
                consoleKey = Console.ReadKey();
                if (consoleKey.Key == ConsoleKey.UpArrow) 
                    //In a case of pressing the upArrow key, call "Up" function
                    Up();
                if (consoleKey.Key == ConsoleKey.DownArrow)
                    Down();
                if (consoleKey.Key == ConsoleKey.RightArrow)
                {
                    ok = false;
                    cursor = 0;
                }
                if (consoleKey.Key == ConsoleKey.LeftArrow)
                {
                    cursor = 0;
                    ok = true;
                }
                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    if (currentFs.GetType() == typeof(DirectoryInfo))
                        //the same as is method
                    {
                        cursor = 0;
                        path = currentFs.FullName;
                    }
                    else
                    {
                        Process.Start(currentFs.FullName);
                    }
                }
                if (consoleKey.Key == ConsoleKey.Backspace)
                {
                    cursor = 0;
                    path = directory.Parent.FullName;
                }
                if (consoleKey.Key == ConsoleKey.D)
                {
                    //if the key D is pressed and if the current file is directory then call the method DeleteDirectory
                    if (currentFs is DirectoryInfo)
                    {
                        DeleteDirectory(currentFs.FullName);
                    }
                    else // in another case it is file then file will be just deleted 
                    {
                        currentFs.Delete();
                    }
                    cursor = 0;

                }
                if (consoleKey.Key == ConsoleKey.P)
                    //if the key P is pressed, then do the renaming 
                {
                    string oldpath = currentFs.FullName;
                    //fullname is contain full path and name included
                    string newname = Console.ReadLine();
                    //read the var newname

                    Directory.Move(oldpath, path + "/" + newname);
                    //rename oldpath to (path+newname=)fulname of new file  

                }
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\doo";
            //give the path of directory to variable 
            FarManager farManager = new FarManager(path);
            //create the object for class farManager
            farManager.Start();
            //call function Start
        }
    }
}