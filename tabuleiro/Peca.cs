using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrex_console.tabuleiro {
    internal class Peca {

        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca (Posicao posicao, Tabuleiro tab, Cor cor) {

            Posicao = posicao;
            Tab = tab;
            Cor = cor;
            QtdMovimentos = 0;

        }

    }
}
