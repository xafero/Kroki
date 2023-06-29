using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public class TForm1
    {
        public string Edit1;
        public TButton Button1;
        public TLabel Label1;
        public void Button1Click(object Sender)
        {
            string s = default;
            s = "Hello " + Edit1 + " Delphi welcomes you!";
            Console.WriteLine(s);
        }

        private string FMQServer;
        private string getMQServer()
        {
            return FMQServer;
        }

        private void setMQserver(string Value)
        {
            FMQServer = Value;
        }

        public string MQServer { get; set; }
    }
}
