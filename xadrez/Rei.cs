﻿using System;
using tabuleiro;

namespace xadrez {
    internal class Rei : Peca {

        private PartidaDeXadrez Partida;

        public Rei(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor) {
            Partida = partida;
        }

        public override string ToString() {
            return "R";
        }

        private bool PodeMover(Posicao pos) {
            Peca p = Tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        private bool TesteTorreParaRoque(Posicao pos) {
            Peca p = Tab.peca(pos);
            return p != null && p is Torre && p.Cor == Cor && p.QtdMovimentos == 0;
        }


        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[Tab.Colunas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //acima
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;

            }

            //nordeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;

            }

            //direita
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;

            }

            //sudeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;

            }

            //abaixo
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;

            }

            //sudoeste 
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {
                //
                mat[pos.Linha, pos.Coluna] = true;

            }

            //esquerda
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;
                
            }

            //noroeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos)) {

                mat[pos.Linha, pos.Coluna] = true;

            }

            // #jogada especial roque
            if (QtdMovimentos == 0 && !Partida.Xeque) {
                
                // #jogada especial roque pequeno
                Posicao posT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
                if (TesteTorreParaRoque(posT1)) {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                    
                    if (Tab.peca(p1) == null && Tab.peca(p2) == null) {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }

                }

                // #jogada especial roque grande
                Posicao posT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                if (TesteTorreParaRoque(posT2)) {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);

                    if (Tab.peca(p1) == null && Tab.peca(p2) == null && Tab.peca(p3) == null) {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }

                }

            }


            return mat;

        }
    }
}
