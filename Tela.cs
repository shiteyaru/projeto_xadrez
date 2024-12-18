using System;
using tabuleiro;

namespace xadrex_console {
    internal class Tela {

        public static void ImprimirTabuleiro(Tabuleiro tab) {

            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write($"{8 - i}  ");
                for (int j = 0; j < tab.Colunas; j++) {

                    if (tab.peca(i, j) == null) {
                        Console.Write($"- ");
                    }
                    else {
                        Tela.ImprimirPeca(tab.peca(i, j));
                        Console.Write(" ");
                    }
                    
                }
                Console.WriteLine();


            }
            Console.WriteLine("\n  a b c d e f g h");

        }

        public static void ImprimirPeca(Peca peca) {

            if (peca.Cor == Cor.Branca) {
                Console.Write($"{peca}");
            }
            else {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }

        }
    }
}
