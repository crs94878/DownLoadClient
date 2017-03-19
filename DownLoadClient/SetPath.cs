using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace DownLoadClient
{

  public  class SetPath
    {
        public static string AddPath { get; set; }

       
        public  void PathWriter()
        {
            try
            {
                FileStream crPath = new FileStream("PathSetings.ini", FileMode.OpenOrCreate);
                crPath.Close();
                FileStream fsPath = new FileStream("PathSetings.ini", FileMode.Truncate, FileAccess.Write);
                StreamWriter wrPath = new StreamWriter(fsPath);
                wrPath.Write(AddPath);
                wrPath.Close();
                fsPath.Close();
               

            } catch (Exception ex) { Console.Write(ex.Message); }

        }
        public  string PathReader()
        {
            string PathName=null;
            try
            {
                FileStream sfPath = new FileStream("PathSetings.ini", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader rdPath = new StreamReader(sfPath);
                PathName = rdPath.ReadToEnd();
                rdPath.Close();
                sfPath.Close();
            }catch (Exception ex) { Console.Write(ex.Message); }
            return PathName;
        }
    }

}
