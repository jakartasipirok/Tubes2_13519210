using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;


namespace Tubes_2_Stima
{
    public class filesAndFolder
    {
        public string parent;
        public string direct;

        public filesAndFolder()
        {
             this.parent = "";
             this.direct = "";
        }

        public filesAndFolder(string parent, string direct)
        {
            this.parent = parent;
            this.direct = direct;
        }

        public filesAndFolder(filesAndFolder fl)
        {
            this.parent = fl.parent;
            this.direct = fl.direct;
        }

        public void setvalue(string parent,string direct)
        {
            this.parent = parent;
            this.direct = direct;
        }
    }
    class BFS : filesAndFolder
    {
        public static string pathBFS = "";

        public Queue<filesAndFolder> graph = new Queue<filesAndFolder>(); // Queue buat output

        public void makeGraph()
        {

        }
        public void SearchBFS(string root, string filename)
        {
            Queue<string> dirs_visited = new Queue<string>(10000);


            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            dirs_visited.Enqueue(root);
            graph.Enqueue(new filesAndFolder("", root));

            while (dirs_visited.Count > 0)
            {
                string currentDir = dirs_visited.Dequeue();
                pathBFS = currentDir + "\\";

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    System.Console.WriteLine(e.Message);
                    continue;
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }
                foreach (string file in files)
                {
                    graph.Enqueue(new filesAndFolder(currentDir, file));
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (fi.Name == filename)
                        {
                            pathBFS += filename;
                            System.Console.WriteLine(pathBFS);
                            return;
                        }
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        System.Console.WriteLine(e.Message);
                        continue;
                    }
                }

                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string str in subDirs)
                {
                    dirs_visited.Enqueue(str);
                    graph.Enqueue(new filesAndFolder(currentDir, str));
                }
            }
        }
    }
}