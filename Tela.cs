﻿using Course;
using System;
using tabuleiro;
using xadrez;

namespace xadrex_console {
    internal class Tela {

        public static void ImprimirPartida(PartidaDeXadrez partida) {

            ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine($"\nTurno: {partida.Turno}");
            Console.WriteLine($"Aguardando jogada: {partida.JogadorAtual}");
            if (partida.Xeque) {
                Console.WriteLine("XEQUE!");
            }

        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida) {
            
            Console.WriteLine($"Peças capturadas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.Write($"Brancas: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.ForegroundColor = aux;

            
            
            Console.Write($"Pretas: ");
            Console.ForegroundColor = ConsoleColor.Red;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;

        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto) {

            Console.Write("[");
            foreach (Peca x in conjunto) {
                Console.Write($"{x} ");
            }
            Console.WriteLine("]");
        }


        public static void ImprimirTabuleiro(Tabuleiro tab) {

            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write($"{8 - i}  ");
                for (int j = 0; j < tab.Colunas; j++) {

                    ImprimirPeca(tab.peca(i, j));
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine($"\n    a  b  c  d  e  f  g  h");
        }

        

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis) {

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;


            for (int i = 0; i < tab.Linhas; i++) {
                Console.Write($"{8 - i}  ");
                for (int j = 0; j < tab.Colunas; j++) {
                    
                    
                    if (posicoesPossiveis[i, j]) {

                        Console.BackgroundColor = fundoAlterado;

                    }
                    else {

                        Console.BackgroundColor = fundoOriginal;

                    }

                    ImprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;

                }
                Console.WriteLine();
            }
            Console.WriteLine($"\n    a  b  c  d  e  f  g  h");
            Console.BackgroundColor = fundoOriginal;
        }



        public static PosicaoXadrez LerPosicaoXadrez() {

            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse($"{s[1]}");
            return new PosicaoXadrez(coluna, linha);

        }


        public static void ImprimirPeca(Peca peca) {

            if (peca == null) {
                Console.Write($" - ");
            }
            else {
                Console.Write(" ");
                if (peca.Cor == Cor.Branca) {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                else {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }

        }

        public static string Espaco() {
            return "                         ";
        }
    }
}
