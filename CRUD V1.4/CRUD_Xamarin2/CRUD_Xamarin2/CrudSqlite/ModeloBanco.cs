using CRUD_Xamarin2.Funcionalidades;
using CRUD_Xamarin2.ModeloFilme;
using CRUD_Xamarin2.ModeloSalvarSenha;
using CRUD_Xamarin2.ModeloUsuarios;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Xamarin2.CRUD
{
    class ModeloBanco
    {
        string Pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public string RetornarCaminhoImgs()
        {
            return Pasta;
        }

        //Função para o Usuario que apertou a checkbox de Lembrar da Senha
        public bool CriarBancoDeDadosLembrar()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_LembrarSenha.db")))
                {
                    conexao.CreateTable<SalvarSenha>();
                    InserirSalvar();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



        public bool InserirSalvar()
        {
            SalvarSenha senha = new SalvarSenha();
            senha.Cpf = "000.000.000-00";
            senha.Salvo = true;
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_LembrarSenha.db")))
                {
                    var sucesso = GetUsuarioSalvar();
                    if(!(sucesso.Count > 0))
                    {
                        conexao.Insert(senha);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SalvarSenha> GetUsuarioSalvar()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_LembrarSenha.db")))
                {
                    return conexao.Query<SalvarSenha>("Select * from SalvarSenha");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AlterarSenhaSalva(Usuario user, bool salvo)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_LembrarSenha.db")))
                {
                    var pegarAnterior = GetUsuarioSalvar();
                    conexao.Query<SalvarSenha>("UPDATE SalvarSenha SET salvo=?, cpf =? WHERE cpf =?", salvo, user.Cpf, pegarAnterior[0].Cpf);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CriarBancoDeDadosUsuarios()
        {
            Usuario admin = new Usuario();
            Criptografia criptografar = new Criptografia();
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    conexao.CreateTable<Usuario>();
                   
                    admin.Nome = "MyFilm.contato@gmail.com";
                    admin.Adm = true;
                    admin.Cpf = "000.000.000-00";
                    admin.Senha = "123";
                    var nome = conexao.Query<Usuario>("Select Cpf from Usuario where Cpf = '" + admin.Cpf + "';");
                    if(nome.Count < 1)
                    {
                        InserirUsuario(admin);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CriarBancoDeDadosFilmes()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    conexao.CreateTable<Filme>();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InserirUsuario(Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    var nome = conexao.Query<Usuario>("Select Cpf from Usuario where Cpf = '" + user.Cpf + "';");
                    Filme filme = new Filme();
                    Criptografia criptar = new Criptografia();
                    user.Senha = criptar.Encriptar(user.Senha);

                    if (nome.Count==0)
                    { 
                        filme.Nome = "Vingadores: EndGame";
                        filme.Cpf = user.Cpf;
                        filme.Ano = "2019";
                        filme.Genero = "Ficção";
                        filme.Imagem = "banner.jpg";
                        filme.Cor = "White";
                        InserirFilme(filme);
                        conexao.Insert(user);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InserirFilme(Filme filme)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    conexao.Insert(filme);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Usuario> GetUsuario(Usuario usuario)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    return conexao.Query<Usuario>("Select * from Usuario where cpf =? and senha =?", usuario.Cpf, usuario.Senha);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Usuario> GetUsuarioSemSenha(Usuario usuario)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    return conexao.Query<Usuario>("Select * from Usuario where cpf =?", usuario.Cpf);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Filme> GetFilmes(Usuario usuario)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    return conexao.Query<Filme>("Select * from Filme where cpf =?", usuario.Cpf);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Filme> GetFilme(Usuario usuario, string _nome)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    return conexao.Query<Filme>("Select * from Filme where nome =? and cpf =?", _nome, usuario.Cpf);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AtualizarSenha(Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    conexao.Query<Usuario>("UPDATE Usuario set senha =? Where cpf=?", user.Senha, user.Cpf);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        public bool DeletarFilme(Filme filme, Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    var existe = GetFilme(user, filme.Nome);
                    if(existe.Count < 1)
                    {
                        return false;
                    }
                    else
                    {
                        conexao.Query<Filme>("DELETE FROM Filme WHERE Nome=? and Cpf=?", filme.Nome, user.Cpf);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AtualizarFilme(Filme filme, Usuario usuario, Filme anterior)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    conexao.Query<Filme>("UPDATE Filme set Nome =?, Ano=?, Genero=?, Imagem=? WHERE Cpf=? and Nome =?", filme.Nome, filme.Ano, filme.Genero, filme.Imagem, usuario.Cpf, anterior.Nome);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ApagarTodosUsuarios()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    conexao.Query<Usuario>("DROP TABLE Usuario");
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AtualizarUsuarioFull(Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    conexao.Query<Usuario>("UPDATE Usuario set nome =?, senha =? Where cpf=?", user.Nome, user.Senha, user.Cpf);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        public List<Usuario> GetTodosUsuarios()
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    return conexao.Query<Usuario>("Select * from Usuario ");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeletarUsuario(Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    conexao.Query<Usuario>("DELETE FROM Usuario WHERE Cpf=?",  user.Cpf);
                    DeletarTodosFilmes(user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeletarTodosFilmes(Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Filmes.db")))
                {
                    conexao.Query<Filme>("DELETE FROM Filme WHERE Cpf=?", user.Cpf);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool AtualizarAdmin(Usuario user)
        {
            try
            {
                using (var conexao = new SQLiteConnection(System.IO.Path.Combine(Pasta, "MyFilm_Usuarios.db")))
                {
                    conexao.Query<Usuario>("UPDATE Usuario set Adm =? Where cpf=?", user.Adm, user.Cpf);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

