// See https://aka.ms/new-console-template for more information
namespace RedRover{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput = "";
            while(userInput!="0"){
                Console.WriteLine("Please enter your input enter 0 to leave");
                userInput = Console.ReadLine();
                
                BaseObject bo = new BaseObject(userInput);
                PrintController pc = new PrintController();
                pc.printAll(bo);
            }
        }
    }

}



