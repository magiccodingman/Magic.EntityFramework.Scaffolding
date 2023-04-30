﻿using Magic.EntityFramework.Scaffolding;

Console.ForegroundColor = ConsoleColor.White;
if (CheckSystem.PerformChecks())
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(@"                             /\
                            /  \
                           |    |
                         --:'''':--
                           :'_' :
                           _:"":\___
            ' '      ____.' :::     '._
           . *=====<<=)           \    :
            .  '      '-'-'\_      /'._.'
                             \====:_ ""
                            .'     \\
                           :       :
                          /   :    \
                         :   .      '.
         ,. _        snd :  : :      :
      '-'    ).          :__:-:__.;--'
    (        '  )        '-'   '-'
 ( -   .00.   - _
(    .'  _ )     )
'-  ()_.\,\,   -");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(@"
███╗░░░███╗░█████╗░░██████╗░██╗░█████╗░
████╗░████║██╔══██╗██╔════╝░██║██╔══██╗
██╔████╔██║███████║██║░░██╗░██║██║░░╚═╝
██║╚██╔╝██║██╔══██║██║░░╚██╗██║██║░░██╗
██║░╚═╝░██║██║░░██║╚██████╔╝██║╚█████╔╝
╚═╝░░░░░╚═╝╚═╝░░╚═╝░╚═════╝░╚═╝░╚════╝░");
    Console.WriteLine(@"
██████╗░░█████╗░████████╗░█████╗░██████╗░░█████╗░░██████╗███████╗
██╔══██╗██╔══██╗╚══██╔══╝██╔══██╗██╔══██╗██╔══██╗██╔════╝██╔════╝
██║░░██║███████║░░░██║░░░███████║██████╦╝███████║╚█████╗░█████╗░░
██║░░██║██╔══██║░░░██║░░░██╔══██║██╔══██╗██╔══██║░╚═══██╗██╔══╝░░
██████╔╝██║░░██║░░░██║░░░██║░░██║██████╦╝██║░░██║██████╔╝███████╗
╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░╚═╝░░╚═╝╚═════╝░╚═╝░░╚═╝╚═════╝░╚══════╝");
    Console.WriteLine(@"
░██████╗░█████╗░░█████╗░███████╗███████╗░█████╗░██╗░░░░░██████╗░██╗███╗░░██╗░██████╗░
██╔════╝██╔══██╗██╔══██╗██╔════╝██╔════╝██╔══██╗██║░░░░░██╔══██╗██║████╗░██║██╔════╝░
╚█████╗░██║░░╚═╝███████║█████╗░░█████╗░░██║░░██║██║░░░░░██║░░██║██║██╔██╗██║██║░░██╗░
░╚═══██╗██║░░██╗██╔══██║██╔══╝░░██╔══╝░░██║░░██║██║░░░░░██║░░██║██║██║╚████║██║░░╚██╗
██████╔╝╚█████╔╝██║░░██║██║░░░░░██║░░░░░╚█████╔╝███████╗██████╔╝██║██║░╚███║╚██████╔╝
╚═════╝░░╚════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝░░░░░░╚════╝░╚══════╝╚═════╝░╚═╝╚═╝░░╚══╝░╚═════╝░");
    Console.ForegroundColor = ConsoleColor.White;

    for (; ; )
    {
        new CliMenu().MainMenu();
    }
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(Environment.NewLine);
    Console.WriteLine(@"
███████╗██████╗░██████╗░░█████╗░██████╗░██╗
██╔════╝██╔══██╗██╔══██╗██╔══██╗██╔══██╗██║
█████╗░░██████╔╝██████╔╝██║░░██║██████╔╝██║
██╔══╝░░██╔══██╗██╔══██╗██║░░██║██╔══██╗╚═╝
███████╗██║░░██║██║░░██║╚█████╔╝██║░░██║██╗
╚══════╝╚═╝░░╚═╝╚═╝░░╚═╝░╚════╝░╚═╝░░╚═╝╚═╝");
    Console.WriteLine(Environment.NewLine);
    Console.WriteLine("Program is not available until you resolve the issues logged above.");
    Console.WriteLine("Type anything to exit the application:");
    Console.WriteLine(Environment.NewLine);
    Console.ReadLine();
    Console.ForegroundColor = ConsoleColor.White;
}