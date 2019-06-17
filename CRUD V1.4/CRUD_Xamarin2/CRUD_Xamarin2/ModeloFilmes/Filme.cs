using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Xamarin2.ModeloFilme
{
    //Banco de Dados para Inserir Filmes
    public class Filme
    {
        [AutoIncrement, NotNull, PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public string Nome { get; set; }
        [NotNull]
        public string Ano { get; set; }
        [NotNull]
        public string Genero { get; set; }
        [NotNull]
        public string Imagem { get; set; }
        [NotNull]
        public string Cpf { get; set; }
        [NotNull]
        public string Cor { get; set; }
    }
}
