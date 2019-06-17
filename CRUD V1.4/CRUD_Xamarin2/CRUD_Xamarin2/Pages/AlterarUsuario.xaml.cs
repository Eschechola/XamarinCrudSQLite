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

namespace CRUD_Xamarin2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AlterarUsuario : ContentPage
	{
        //variaveis auxiliares
        bool _noturno;
        Usuario _user;
        ModeloBanco banco = new ModeloBanco();
        Criptografia criptar = new Criptografia();
        

		public AlterarUsuario (Usuario user, bool noturno)
		{
			InitializeComponent ();

            _user = user;
            _noturno = noturno;
            
            //pega o usuario do banco e mostra seus dados na tela (SENHA JA DESCRIPTOGRAFADA)
            var usuarioPego = banco.GetUsuarioSemSenha(_user);
            lbl_CPF.Text = usuarioPego[0].Cpf;
            lbl_mail.Text = usuarioPego[0].Nome;
            lbl_senha.Text = criptar.Decrypt(usuarioPego[0].Senha);
            lbl_csenha.Text = criptar.Decrypt(usuarioPego[0].Senha);
        }

        //função para alterar os dados do usuario
        private void AlterarDadosUsuario(object sender, EventArgs e)
        {
            //Tratamento de Erros apra diversas ações

            //Espaço Vazio
            if (string.IsNullOrEmpty(lbl_mail.Text)|| string.IsNullOrWhiteSpace(lbl_mail.Text))
            {
                DisplayAlert("Erro!", "Insira um E - MAIL!", "Entendi");
            }
            //Se o E-mail não possui '@' e nem '.'
            else if (lbl_mail.Text.IndexOf('@')==-1|| lbl_mail.Text.IndexOf('.') == -1)
            {
                DisplayAlert("Erro!", "Insira um E - Mail VÁLIDO!", "Entendi");
            }

            //Espaço Vazio
            else if (string.IsNullOrEmpty(lbl_senha.Text)||string.IsNullOrWhiteSpace(lbl_senha.Text))
            {
                DisplayAlert("Erro!", "Insira uma SENHA!", "Entendi");
            }


            //Espaço Vazio
            else if (string.IsNullOrEmpty(lbl_csenha.Text) || string.IsNullOrWhiteSpace(lbl_csenha.Text))
            {
                DisplayAlert("Erro!", "Insira uma CONFIRMAÇÃO DE SENHA!", "Entendi");
            }

            //Senha Diferente 
            else if (lbl_senha.Text != lbl_csenha.Text)
            {
                DisplayAlert("Erro!", "As senhas NÃO SÃO IGUAIS!", "Entendi");
            }
            //Realizações das operações 
            else
            {
                _user.Nome = lbl_mail.Text;
                //Criptar Senha
                _user.Senha = criptar.Encriptar(lbl_senha.Text);
                //atualiza os dadaos do usuario no banco e volta a pagina
                var sucesso = banco.AtualizarUsuarioFull(_user);
                if (sucesso)
                {
                    DisplayAlert("Parabéns", "Usuário atualizdo com sucesso!", "Ok");
                    Navigation.PopModalAsync();
                }
                else
                {
                    DisplayAlert("Erro!", "Aconteceu algum erro! Por - favor tente novamente.", "Ok");
                }
            }
            
        }
    }
}