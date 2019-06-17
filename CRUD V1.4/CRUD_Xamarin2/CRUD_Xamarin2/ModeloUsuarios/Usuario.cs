using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Xamarin2.ModeloUsuarios
{
    //Banco de Dados do Usuario
    public class Usuario
    {
        [PrimaryKey, Unique, NotNull]
        public string Cpf { get; set; }
        [NotNull]
        public string Nome { get; set; }
        [NotNull]
        public string Senha { get; set; }
        [NotNull]
        public bool Adm { get; set; }
    }
}
