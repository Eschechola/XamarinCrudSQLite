using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Xamarin2.ModeloSalvarSenha
{
    //Classe para Salvar Senha do Usuario
    class SalvarSenha
    {
        [PrimaryKey, NotNull]
        public int Id { get; set; }
        [NotNull]
        public bool Salvo { get; set; }
        [NotNull]
        public string Cpf { get; set; }
    }
}
