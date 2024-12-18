﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace tabuleiro {
    internal abstract class Peca {

        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca (Tabuleiro tab, Cor cor) {

            Posicao = null;
            Tab = tab;
            Cor = cor;
            QtdMovimentos = 0;

        }

        public void IncrementarQtdMovimentos() {
            QtdMovimentos++;
        }
         
        public abstract bool[,] MovimentosPossiveis();


    }
}
