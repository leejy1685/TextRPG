using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace TextRPG
{
    internal class Program
    {
  
        static void Main(string[] args)
        {
            //저장 데이터 로드 후 실행
            GameManager gameManager = new GameManager();
            //gameManager.loadData();
            gameManager.SetData();
            gameManager.DisplayMainUI();

        }

       
    }

}
