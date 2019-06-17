using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.Funcionalidades;
using CRUD_Xamarin2.ModeloUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Esq_Senha : ContentPage
	{
        //inicializa objetos auxiliares
        ModeloBanco banco = new ModeloBanco();
        Usuario user = new Usuario();
        public Esq_Senha ()
		{
			InitializeComponent ();
            //esconde o modal de trocar senha
            alterarSenhas.IsVisible = false;
            alterarSenhas.IsEnabled = false;
        }


        //funçao para validar o cpf
        private void validaCPF(object sender, EventArgs e)
        {
            try
            {
                //valida o campo do cpf
                if (cpf.Text.Length < 11)
                {
                    DisplayAlert("Aviso!", "Por - favor, insira um CPF válido!", "Ok");
                }
                else
                {
                    // declara o cpf do usuario auxiliar de pesquisa igual ao text da label cpf
                    user.Cpf = cpf.Text;
                    //ve se o cpf existe
                    var usuario = banco.GetUsuarioSemSenha(user);
                    //se o cpf for igual ao cpf do admnistrador raiz, não poderá trocar a senha
                    if (cpf.Text == "000.000.000-00")
                    {
                        DisplayAlert("Erro!", "Você NÃO TEM PERMISSÃO para mudar a senha desse USUÁRIO!", "Entendi!");
                    }
                    else if (usuario.Count>0)
                    {
                        //se o cpf existir ele abre o modal para a troca de senha
                        alterarSenhas.IsVisible = true;
                        alterarSenhas.IsEnabled = true;
                    }
                    else
                    {
                        //se nao existir ele apenas exibe uma mensagem
                        alterarSenhas.IsVisible = false;
                        alterarSenhas.IsEnabled = false;
                        DisplayAlert("Erro!", "Usuário não encontrado!", "Ok");
                    }
                }
            }
            catch (Exception)
            {
                DisplayAlert("Aviso!", "Por - favor, insira um CPF válido!", "Ok");
            }
        }

        //função para atualizar o usuario
        private async void atualizarUsuario(object sender, EventArgs e)
        {
            //valida o campo da senha
            if (senha.Text != c_senha.Text)
            {
                DisplayAlert("Aviso!", "As senhas NÃO SÃO IGUAIS!", "Entendi!");
            }
            else
            {
                //atribui a senha para o objeto auxiliar
                user.Senha = senha.Text;
                Criptografia cript = new Criptografia();
                //criptografa a senha
                user.Senha = cript.Encriptar(user.Senha);

                //atualiza a senha do usuario no banco de dados
                if (banco.AtualizarSenha(user))
                {
                    DisplayAlert("Parabéns", "Sua senha foi alterada com sucesso!", "Entendi!");
                    Navigation.PopModalAsync();

                }
                else
                {
                    DisplayAlert("Erro!", "Não foi possível alterar sua senha, entre em contato com os desenvolvedores", "Ok");
                }
                
            }
        }
    }
}