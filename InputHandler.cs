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
            {"init", new List<int>{ 1 } },
            {"create", new List<int>{ 1 } },
            {"get", new List<int>{ 2, 3 } },
            {"set", new List<int>{ 2, 3 } },
            {"delete", new List<int>{ 2 } },
            {"secret", new List<int>{ 1 } },
            {"show", new List<int>{ 1 }}
        };
        private string[] args;
        public string cmd { get; private set; }
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
            RequestMasterPassword();
        }

        private void CreatePopulate()
        {
            RequestMasterPassword();
            RequestSecretKey();
        }

        private void GetPopulate()
        {
            prop = args[1];
            RequestMasterPassword();
        }

        private void SetPopulate()
        {
            prop = args[1];

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
            prop = args[1];
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
