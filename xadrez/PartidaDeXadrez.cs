using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrex_console;

namespace xadrez {
    internal class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; } 
        public int Turno { get; private set;     }
        public Cor JogadorAtual {  get; private set; }
        public bool Terminada {  get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque {  get; private set; }


        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
            
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino) {

            Peca p = tab.retirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null) {
                Capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tab.retirarPeca(destino);
            p.DecrementarQtdMovimentos();
            if (pecaCapturada != null) {
                tab.colocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);
        }


        public void RealizaJogada(Posicao origem, Posicao destino) {

            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocqar em xeque!");
            }

            if (EstaEmXeque(Adversaria(JogadorAtual))) {
                Xeque = true;
            }
            else {
                Xeque = false;
            }

            if (TesteXequeMate(Adversaria(JogadorAtual))) {
                Terminada = true;
            }
            else {
                Turno++;
                MudaJogador();
            }

        }

        public void ValidarPosicaoOrigem(Posicao pos) {

            if(tab.peca(pos) == null) {
                throw new TabuleiroException($"Não existe peça na \nposição escolhida!");
            }
            if (JogadorAtual != tab.peca(pos).Cor) {
                throw new TabuleiroException($"A peça de origem escolhida \nnão é sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis()) {
                throw new TabuleiroException($"Não há movimentos possíveis para a \npeça de origem escolhida!");
            }

        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino) {
            if (!tab.peca(origem).PodeMoverPara(destino)) {
                throw new TabuleiroException($"Posição de destino inválida!");
            }
        }


        private void MudaJogador() {

            if (JogadorAtual == Cor.Branca) {
                JogadorAtual = Cor.Preta;
            }
            else {
                JogadorAtual = Cor.Branca;
            }

        }

        public HashSet<Peca> PecasCapturadas(Cor cor) {

            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas) { 

                if (x.Cor == cor) {
                    aux.Add(x);
                }

            }

            return aux;

        }

        public HashSet<Peca> PecasEmJogo(Cor cor) {

            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas) {

                if (x.Cor == cor) {
                    aux.Add(x);
                }

            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }


        private Cor Adversaria(Cor cor) {

            if (cor == Cor.Branca) {
                return Cor.Preta;
            }
            else {
                return Cor.Branca;
            }

        }

        private Peca Rei(Cor cor) {

            foreach(Peca x in PecasEmJogo(cor)) {

                if (x is Rei) {
                    return x;
                }

            }
            return null;
        }

        private bool EstaEmXeque(Cor cor) {
            Peca R = Rei(cor);
            if (R == null) {
                throw new TabuleiroException($"Não há rei da cor {cor} no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(Adversaria(cor))) {

                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna]) {
                    return true;
                }

            }
            return false;
        }

        public bool TesteXequeMate(Cor cor) {
            if (!EstaEmXeque(cor)) {
                return false;
            }
            foreach(Peca x in PecasEmJogo(cor)) {
                bool[,] mat = x.MovimentosPossiveis();
                for(int i = 0; i < tab.Linhas; i++) {
                    for (int j = 0; j < tab.Colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {

            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);

        }

        private void ColocarPecas() {

            /*ColocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

            ColocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));*/

            ColocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));
            ColocarNovaPeca('h', 7, new Torre(tab, Cor.Branca));

            ColocarNovaPeca('a', 8, new Rei(tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Torre(tab, Cor.Preta));

        }

    }
}
