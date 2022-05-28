using System;
using System.Collections.Generic;
using System.IO;

namespace PasswordVault
{

    public class InputHandler
    {
        //number of accepted arguments for each command
        private readonly Dictionary<string, List<int>> ACCEPTED_PARAMS = new Dictionary<string, List<int>>()
        {
            {"init", new List<int>{ 3 } },
            {"create", new List<int>{ 3 } },
            {"get", new List<int>{ 3, 4 } },
            {"set", new List<int>{ 4, 5 } },
            {"delete", new List<int>{ 4 } },
            {"secret", new List<int>{ 2 } }
        };
        private string[] args;
        public string cmd { get; private set; }
        public string clPath { get; private set; }
        public string sPath { get; private set; } = "";
        public string mPwd { get; private set; }
        public string secretKey { get; private set; }
        public bool generatePassword { get; private set; } = false;
        public string prop { get; private set; } = "";
        public string propPwd { get; set; }

        public InputHandler (string[] args)
        {
            this.args = args;
            ArgumentsCorrect();
            SetProperties();
        }

        public void PrintTextToConsole(string s)
        {
            Console.WriteLine(s);
        }

        public void PrintErrorToConsoleAndExit(string s)
        {
            Console.WriteLine("ERROR: " + s + "!");
            Environment.Exit(0);
        }

        public void ConfirmCommandSuccesful()
        {
            Console.WriteLine(cmd.ToUpper() + " successful!");
        }


        public bool ArgumentsCorrect()
        {
            //checks if the command doesn't have more or less than the parameters the command requires
            if (args.Length == 0 || 
                !ACCEPTED_PARAMS.ContainsKey(args[0].ToLower()) ||
                !ACCEPTED_PARAMS[args[0].ToLower()].Contains(args.Length))
            {
                //ERROR MESSAGE: Incorrect params, display errormessage
                PrintErrorToConsoleAndExit("Arguments incorrectly formatted");
                return false;
            } 
            else
            {
                return true;
            }
        }

        public void SetProperties()
        {
            cmd = args[0].ToLower(); ;
            clPath = args[1];
            switch (cmd)
            {
                case "init":
                    InitPopulate();
                    break;
                case "create":
                    CreatePopulate();
                    break;
                case "get":
                    GetPopulate();
                    break;
                case "set":
                    SetPopulate();
                    break;
                case "delete":
                    DeletePopulate();
                    break;
                case "secret":
                    break;
                default:
                    Console.WriteLine("This line should not be reachable!");
                    break;
            }
        }

        private void InitPopulate()
        {
            sPath = args[2];
            RequestMasterPassword();
        }

        private void CreatePopulate()
        {
            sPath = args[2];
            RequestMasterPassword();
            RequestSecretKey();
        }

        private void GetPopulate()
        {
            sPath = args[2];
            if (args.Length == 4)
            {
                prop = args[3];
            }
            RequestMasterPassword();
        }

        private void SetPopulate()
        {
            sPath = args[2];
            prop = args[3];

            RequestMasterPassword();

            if (args[args.Length - 1] == "-g" || args[args.Length - 1] == "--generate")
            {
                generatePassword = true;
            }
            else
            {
                RequestPropPassword();
            }
        }

        private void DeletePopulate()
        {
            sPath = args[2];
            prop = args[3];
            RequestMasterPassword();
        }
        private void RequestMasterPassword()
        {
            Console.WriteLine("Enter master password:");
            mPwd = Console.ReadLine();
        }

        private void RequestSecretKey()
        {
            Console.WriteLine("Enter your secret key:");
            secretKey = Console.ReadLine();
        }

        private void RequestPropPassword()
        {
            Console.WriteLine("Enter password for " + prop + ":");
            propPwd = Console.ReadLine();
        }
    }
}
