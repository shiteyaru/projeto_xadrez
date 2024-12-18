﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xadrex_console.tabuleiro;

namespace tabuleiro {
    internal class Tabuleiro {

        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] pecas;

        public Tabuleiro (int linhas, int colunas) {

            Linhas = linhas;
            Colunas = colunas;
            pecas = new Peca[linhas, colunas];

        }


    }
}
